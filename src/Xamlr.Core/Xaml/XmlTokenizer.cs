// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlTokenizer.cs" company="Xamlr">
//   The MIT License (MIT)
//   
//    Copyright (c) 2019 Brian Tyler
//   
//   Permission is hereby granted, free of charge, to any person obtaining a copy
//   of this software and associated documentation files (the "Software"), to deal
//   in the Software without restriction, including without limitation the rights
//   to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//   copies of the Software, and to permit persons to whom the Software is
//   furnished to do so, subject to the following conditions:
//   
//   The above copyright notice and this permission notice shall be included in
//   all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//   OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//   THE SOFTWARE.
// </copyright>
// <summary>
//   Defines the XmlTokenizer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Xaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using Xamlr.Core.Markup;

    /// <summary>
    /// A class that tokenizes the XML in a <see cref="XmlReader"/> to a sequence of <see cref="FragmentToken"/>.
    /// </summary>
    public class XmlTokenizer
    {
        /// <summary>
        /// The attribute name comparer that determines how to order attribute tokens.
        /// </summary>
        private static readonly IComparer<string> AttributeNameComparer = new AttributeNameComparer();

        /// <summary>
        /// The markup parser.
        /// </summary>
        private static readonly IMarkupExtensionParser MarkupExtensionParser = new MarkupExtensionParser();

        /// <summary>
        /// Overrides to the attribute ordering.
        /// </summary>
        private readonly List<string> attributeOrderOverride = new List<string>();

        /// <summary>
        /// Tokenizes the XML in a <see cref="XmlReader"/> to a sequence of <see cref="FragmentToken"/>.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="FragmentToken"/> representing the XML.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// An unknown node type is encountered.
        /// </exception>
        public IEnumerable<FragmentToken> Tokenize(XmlReader reader)
        {
            var result = new List<FragmentToken>();
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Comment:
                        result.Add(this.CreateCommentToken(reader));
                        break;
                    case XmlNodeType.Whitespace:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Text:
                    case XmlNodeType.EndElement:
                    case XmlNodeType.ProcessingInstruction:
                    case XmlNodeType.XmlDeclaration:
                        result.Add(XmlTokenizer.CreateToken(reader));
                        break;
                    case XmlNodeType.Element:
                        result.AddRange(this.CreateElementTokens(reader));
                        break;
                    default:
                        throw new InvalidOperationException($"Unknown XmlNodeType '{reader.NodeType}'");
                }
            }

            return result;
        }

        /// <summary>
        /// Creates a token that describe an attribute from a <see cref="XmlReader"/> is positioned on an attribute 
        /// node.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <returns>
        /// The newly created <see cref="FragmentToken"/>.
        /// </returns>
        private static FragmentToken CreateAttributeToken(XmlReader reader)
        {
            IMarkupExtensionInfo markup;
            if (XmlTokenizer.MarkupExtensionParser.TryParse(reader.Value, out markup))
            {
                return new FragmentToken(FragmentType.Attribute, reader.Name, markup);
            }

            return XmlTokenizer.CreateToken(reader);
        }

        /// <summary>
        /// Creates a token from a <see cref="XmlReader"/> that assigns Name and Value.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <returns>
        /// The newly created <see cref="FragmentToken"/>.
        /// </returns>
        private static FragmentToken CreateToken(XmlReader reader) => new FragmentToken(reader.NodeType.ToFragmentType(), reader.Name, reader.Value);

        /// <summary>
        /// Creates an attribute token from a <see cref="XmlReader"/> that assigns Name and Value.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <returns>
        /// The newly created <see cref="FragmentToken"/>.
        /// </returns>
        private FragmentToken CreateCommentToken(XmlReader reader)
        {
            var token = new FragmentToken(reader.NodeType.ToFragmentType(), reader.Name, reader.Value);

            var clean = token.ContentString.Trim();
            if (!clean.StartsWith("xamlr:order"))
            {
                return token;
            }

            clean = clean.Remove(0, "xamlr:order".Length).Trim();
            var parts = clean.Split(',').Select(item => item.Trim()).ToArray();
            if (parts.Length < 2)
            {
                return token;
            }

            this.attributeOrderOverride.Clear();
            this.attributeOrderOverride.AddRange(parts);

            return token;
        }

        /// <summary>
        /// Creates the tokens that describe an element from a <see cref="XmlReader"/> is positioned on an element 
        /// node.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <returns>
        /// The newly created <see cref="FragmentToken"/>.
        /// </returns>
        private IEnumerable<FragmentToken> CreateElementTokens(XmlReader reader)
        {
            var elementName = reader.Name;
            var tokenDetails = new ElementTokenDetails(reader.XmlSpace, reader.IsEmptyElement);
            yield return new FragmentToken(FragmentType.Element, elementName, tokenDetails);

            var attributeTokens = new List<FragmentToken>();
            while (reader.MoveToNextAttribute())
            {
                ++tokenDetails.AttributeCount;
                attributeTokens.Add(XmlTokenizer.CreateAttributeToken(reader));
            }

            var comparer = this.attributeOrderOverride.Any()
                ? new AttributeNameComparer(this.attributeOrderOverride)
                : XmlTokenizer.AttributeNameComparer;

            foreach (var token in attributeTokens.OrderBy(item => item.Name, comparer))
            {
                yield return token;
            }

            this.attributeOrderOverride.Clear();

            yield return new FragmentToken(FragmentType.CloseElement, elementName, tokenDetails);

            if (tokenDetails.IsEmptyElement)
            {
                yield return new FragmentToken(FragmentType.EndElement, elementName, string.Empty);
            }
        }
    }
}