// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FragmentType.cs" company="Xamlr">
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
//   Defines the FragmentType type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Xaml
{
    /// <summary>
    /// The fragment type.
    /// </summary>
    public enum FragmentType
    {
        /// <summary>
        /// The start of an element.
        /// </summary>
        Element,

        /// <summary>
        /// The close of element immediately after the last attribute.
        /// </summary>
        CloseElement,

        /// <summary>
        /// The end of an element immediately after the last child.
        /// </summary>
        EndElement,

        /// <summary>
        /// A comment.
        /// </summary>
        Comment,

        /// <summary>
        /// A block of text.
        /// </summary>
        Text,

        /// <summary>
        /// A block of white space.
        /// </summary>
        WhiteSpace,

        /// <summary>
        /// A block of significant white space.
        /// </summary>
        SignificantWhiteSpace,

        /// <summary>
        /// An attribute.
        /// </summary>
        Attribute,

        /// <summary>
        /// A processing instruction.
        /// </summary>
        ProcessingInstruction,

        /// <summary>
        /// The XML declaration.
        /// </summary>
        XmlDeclaration,

        /// <summary>
        /// A markup extension.
        /// </summary>
        MarkupExtension,

        /// <summary>
        /// Xaml itself.
        /// </summary>
        Xaml
    }
}