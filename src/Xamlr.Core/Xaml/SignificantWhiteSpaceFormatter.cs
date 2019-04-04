// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignificantWhiteSpaceFormatter.cs" company="Xamlr">
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
//   Defines the SignificantWhiteSpaceFormatter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Xaml
{
    using System;
    using System.Text;

    /// <summary>
    /// A formatter that formats a <seealso cref="FragmentType.SignificantWhiteSpace"/> fragment.
    /// </summary>
    [FragmentFormatter(FragmentType.SignificantWhiteSpace)]
    public class SignificantWhiteSpaceFormatter : IFormatter
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly FormattingContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignificantWhiteSpaceFormatter"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public SignificantWhiteSpaceFormatter(FormattingContext context)
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

            this.context.Element.HasContent = true;
            builder.Append(token.ContentString.ToNormalizedLineEndings());
        }
    }
}