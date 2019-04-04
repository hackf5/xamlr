// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMarkupExtensionInfo.cs" company="Xamlr">
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
//   Defines the IMarkupExtensionInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Markup
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents information about a markup extension.
    /// </summary>
    public interface IMarkupExtensionInfo
    {
        /// <summary>
        /// Gets the type name.
        /// </summary>
        string TypeName { get; }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        IEnumerable<IMarkupExtensionArgumentInfo> Arguments { get; }

        /// <summary>
        /// Gets a value indicating whether the markup extension has nested arguments.
        /// </summary>
        bool HasNestedArguments { get; }
    }
}