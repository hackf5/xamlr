﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFormatter.cs" company="Xamlr">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 Brian Tyler
//   
//    Copyright (c) 2019 Brian Tylerfree of charge, to any person obtaining a copy
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
//   Defines the IFormatter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core
{
    using System.Text;

    /// <summary>
    /// An object formatter.
    /// </summary>
    public interface IFormatter
    {
        /// <summary>
        /// Formats an object.
        /// </summary>
        /// <param name="obj">
        /// The object to format.
        /// </param>
        /// <param name="builder">
        /// The string builder to which the formatter outputs.
        /// </param>
        void Format(object obj, StringBuilder builder);
    }
}