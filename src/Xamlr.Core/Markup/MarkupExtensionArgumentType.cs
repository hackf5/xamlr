// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarkupExtensionArgumentType.cs" company="Xamlr">
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
//   Defines the MarkupExtensionArgumentType type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Markup
{
    /// <summary>
    /// The markup extension argument type.
    /// </summary>
    public enum MarkupExtensionArgumentType
    {
        /// <summary>
        /// The argument is positional: meaning that it has a STRING component, but no MEMBERNAME.
        /// </summary>
        Positional,

        /// <summary>
        /// The argument is named: meaning that it has both MEMBERNAME and STRING components.
        /// </summary>
        Named
    }
}