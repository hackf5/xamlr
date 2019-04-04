// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarkupExtensionFormatter.cs" company="Xamlr">
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
//   Defines the MarkupExtensionFormatter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Xaml
{
    using System;
    using System.Text;
    using JetBrains.Annotations;
    using Xamlr.Core.Markup;

    /// <summary>
    /// A formatter that formats a <seealso cref="FragmentType.MarkupExtension"/> fragment.
    /// </summary>
    [FragmentFormatter(FragmentType.MarkupExtension)]
    public class MarkupExtensionFormatter : IFormatter
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly FormattingContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkupExtensionFormatter"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public MarkupExtensionFormatter(FormattingContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Formats a <see cref="IMarkupExtensionInfo"/>.
        /// </summary>
        /// <param name="obj">
        /// The object to format.
        /// </param>
        /// <param name="builder">
        /// The string builder to which the formatter outputs.
        /// </param>
        public void Format([NotNull] object obj, [NotNull] StringBuilder builder)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var markup = obj as IMarkupExtensionInfo;
            if (markup == null)
            {
                throw new ArgumentException(@"Parameter must be of type IMarkupExtensionInfo", nameof(obj));
            }

            ++this.context.LocalDepth;
            builder.Append('{').Append(markup.TypeName);

            if (markup.CanInline())
            {
                builder.Append(' ');
            }
            else
            {
                builder.AppendLine().Indent(this.context, false);
            }

            foreach (var argument in markup.Arguments)
            {
                if (argument.ArgumentType == MarkupExtensionArgumentType.Named)
                {
                    builder.AppendFormat("{0}=", argument.MemberNameValue);
                }

                if (!argument.IsNested)
                {
                    var value = ((string)argument.StringValue).ToXmlEncoded();
                    builder.Append(argument.StringDetails.IsQuoted ? "'" + value + "'" : value);
                }
                else
                {
                    this.context.Create(FragmentType.MarkupExtension).Format(argument.StringValue, builder);
                }

                builder.Append(',').AppendLine().Indent(this.context, false);
            }

            builder.RemoveEndWhile(x => x == ',' || char.IsWhiteSpace(x)).Append('}');
            --this.context.LocalDepth;
        }
    }
}