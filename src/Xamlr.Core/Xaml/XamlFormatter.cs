// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XamlFormatter.cs" company="Xamlr">
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
//   Defines the XamlFormatter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Xaml
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml;

    /// <summary>
    /// A formatter that formats a <seealso cref="FragmentType.Xaml"/> fragment.
    /// </summary>
    [FragmentFormatter(FragmentType.Xaml)]
    public class XamlFormatter : IFormatter
    {
        private static readonly Regex Ignore = new Regex(
            @"\sxamlr:ignore\s",
            RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// The context.
        /// </summary>
        private readonly FormattingContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="XamlFormatter"/> class. 
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public XamlFormatter(FormattingContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Formats a XAML document.
        /// </summary>
        /// <param name="obj">
        /// A string containing the XAML document to format.
        /// </param>
        /// <param name="builder">
        /// The string builder to which the formatter outputs.
        /// </param>
        public void Format(object obj, StringBuilder builder)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var xaml = obj as string;
            if (xaml == null)
            {
                throw new ArgumentException(@"Parameter must be of type string", nameof(obj));
            }

            using (var stringReader = new StringReader(xaml))
            {
                try
                {
                    using (var xmlReader = XmlReader.Create(stringReader, new XmlReaderSettings { CloseInput = false }))
                    {
                        var tokens = new XmlTokenizer()
                            .Tokenize(xmlReader)
                            .SkipWhile(candidate => candidate.Type == FragmentType.WhiteSpace);

                        this.context.TokenSource = new FragmentTokenSource(tokens);

                        var ignore = this.context.TokenSource
                            .LookAhead
                            .TakeWhile(item => item.Type == FragmentType.Comment || item.Type == FragmentType.WhiteSpace)
                            .Any(item => XamlFormatter.Ignore.IsMatch(item.ContentString));

                        if (ignore)
                        {
                            throw new WarningException("Document has been ignored");
                        }

                        while (this.context.TokenSource.MoveNext())
                        {
                            this.context
                                .Create(this.context.TokenSource.Token.Type)
                                .Format(this.context.TokenSource.Token, builder);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Failed to format the document: {0}{1}", ex.Message, null);

                    throw;
                }
            }

            builder.RemoveEndWhile(char.IsWhiteSpace);
        }
    }
}