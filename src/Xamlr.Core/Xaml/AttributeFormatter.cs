// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeFormatter.cs" company="Xamlr">
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
//   Defines the AttributeFormatter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Xaml
{
    using System;
    using System.Text;

    using Xamlr.Core.Markup;

    /// <summary>
    /// A formatter that formats a <seealso cref="FragmentType.Attribute"/> fragment.
    /// </summary>
    [FragmentFormatter(FragmentType.Attribute)]
    public class AttributeFormatter : IFormatter
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly FormattingContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeFormatter"/> class. 
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public AttributeFormatter(FormattingContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Formats a <see cref="FragmentToken"/>.
        /// </summary>
        /// <param name="obj">
        /// The object to format.
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

            var token = obj as FragmentToken;
            if (token == null)
            {
                throw new ArgumentException(@"Parameter must be of type FragmentToken", nameof(obj));
            }

            if (token.Content is IMarkupExtensionInfo markup)
            {
                this.WriteStart(builder, token.Name, markup.CanInline());
                this.context.Create(FragmentType.MarkupExtension).Format(markup, builder);
                this.context.LocalDepth = 0;
            }
            else
            {
                this.WriteStart(builder, token.Name, true);
                builder.Append(token.ContentString.ToXmlEncoded());
            }

            builder.Append(@"""").AppendLine();
        }

        /// <summary>
        /// Writes the start of the attribute.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        /// <param name="name">
        /// The name of the attribute.
        /// </param>
        /// <param name="inline">
        /// A value indicating whether the attribute will be written inline.
        /// </param>
        private void WriteStart(StringBuilder builder, string name, bool inline)
        {
            if (inline && this.context.Element.AttributeCount == 1)
            {
                builder.RemoveEndWhile(char.IsWhiteSpace);
                builder.AppendFormat(@" {0}=""", name);
            }
            else
            {
                this.context.Element.AttributesInline = false;
                builder.Indent(this.context, false).AppendFormat(@"{0}=""", name);
            }
        }
    }
}