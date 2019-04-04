// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FragmentTokenSource.cs" company="Xamlr">
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
//   Defines the FragmentTokenSource type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Xaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;

    /// <summary>
    /// The source of <see cref="FragmentToken"/> objects that are to be formatted.
    /// </summary>
    public class FragmentTokenSource
    {
        /// <summary>
        /// The fragment tokens.
        /// </summary>
        private readonly FragmentToken[] tokens;

        /// <summary>
        /// The current index.
        /// </summary>
        private int index;

        /// <summary>
        /// Initializes a new instance of the <see cref="FragmentTokenSource"/> class.
        /// </summary>
        /// <param name="tokens">
        /// The tokens that will form the source.
        /// </param>
        public FragmentTokenSource([NotNull] IEnumerable<FragmentToken> tokens)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            this.tokens = tokens.ToArray();
            this.index = -1;
        }

        /// <summary>
        /// Gets a value indicating whether the source is at the end.
        /// </summary>
        public bool IsEnd => this.tokens.Length == 0 || this.index >= this.tokens.Length;

        /// <summary>
        /// Gets the collection of all tokens starting immediately after and not including the current token.
        /// </summary>
        public IEnumerable<FragmentToken> LookAhead =>
            this.index + 1 < this.tokens.Length
                ? this.tokens.Skip(this.index + 1)
                : new FragmentToken[0];

        /// <summary>
        /// Gets the current token.
        /// </summary>
        public FragmentToken Token => this.tokens[this.index];

        /// <summary>
        /// Moves to the next token and returns a value indicating the success of the move.
        /// </summary>
        /// <returns>
        /// A value indicating whether the move was successful.
        /// </returns>
        public bool MoveNext()
        {
            ++this.index;
            return !this.IsEnd;
        }
    }
}