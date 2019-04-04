// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMarkupExtensionArgumentInfo.cs" company="Xamlr">
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
//   Defines the IMarkupExtensionArgumentInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Markup
{
    /// <summary>
    /// Represents information about an argument in a <see cref="MarkupExtensionInfo"/>.
    /// </summary>
    public interface IMarkupExtensionArgumentInfo
    {
        /// <summary>
        /// Gets the type of the argument.
        /// </summary>
        MarkupExtensionArgumentType ArgumentType { get; }

        /// <summary>
        /// Gets the value of the MEMBERNAME component.
        /// </summary>
        string MemberNameValue { get; }

        /// <summary>
        /// Gets the details associated with the MEMBERNAME component.
        /// </summary>
        IMarkupExtensionTokenDetails MemberNameDetails { get; }

        /// <summary>
        /// Gets the value of the STRING component. This is either a <see cref="string"/> or a 
        /// <see cref="MarkupExtensionInfo"/>. 
        /// </summary>
        object StringValue { get; }

        /// <summary>
        /// Gets the details associated with the STRING component.
        /// </summary>
        IMarkupExtensionTokenDetails StringDetails { get; }

        /// <summary>
        /// Gets a value indicating whether is the STRING component is itself an <see cref="IMarkupExtensionInfo"/>.
        /// </summary>
        bool IsNested { get; }

        /// <summary>
        /// Gets a value indicating whether the argument is empty. The criterion for emptiness is that in the source
        /// string it consisted entirely of whitespace.
        /// </summary>
        bool IsEmpty { get; }
    }
}