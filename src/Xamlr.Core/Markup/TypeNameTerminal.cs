// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeNameTerminal.cs" company="Xamlr">
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
//   Defines the TypeNameTerminal type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Markup
{
    using System;
    using Irony.Parsing;
    using JetBrains.Annotations;

    /// <summary>
    /// The TYPENAME terminal.
    /// </summary>
    public class TypeNameTerminal : Terminal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeNameTerminal"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the terminal.
        /// </param>
        public TypeNameTerminal(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Tries to match the current position in the <paramref name="source"/> with the rules for a TYPENAME terminal;
        /// returning null if there is no match, returning an  error token if the TYPENAME is malformed or there is no
        /// match and returning a TYPENAME token 
        /// otherwise.
        /// </summary>
        /// <param name="context">
        /// The context in which the match is occuring.
        /// </param>
        /// <param name="source">
        /// The source to try to match.
        /// </param>
        /// <returns>
        /// An error token if the TYPENAME is malformed or there is no match and returning a TYPENAME token 
        /// otherwise.
        /// </returns>
        public override Token TryMatch([NotNull] ParsingContext context, [NotNull] ISourceStream source)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            while (true)
            {
                if (source.EOF())
                {
                    return context.CreateErrorToken("Malformed MEMBERNAME or STRING: no terminal encountered");
                }

                if (source.PreviewChar == '}' || char.IsWhiteSpace(source.PreviewChar))
                {
                    var token = source.CreateToken(this);
                    if (string.IsNullOrWhiteSpace(token.ValueString))
                    {
                        return context.CreateErrorToken("Malformed MEMBERNAME or STRING: no terminal encountered");
                    }

                    return token;
                }

                ++source.PreviewPosition;
            }
        }
    }
}