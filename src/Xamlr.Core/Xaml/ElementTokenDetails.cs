// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElementTokenDetails.cs" company="Xamlr">
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
//   Defines the ElementTokenDetails type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Xaml
{
    using System.Xml;

    /// <summary>
    /// Represents details about an element token.
    /// </summary>
    public class ElementTokenDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementTokenDetails"/> class.
        /// </summary>
        /// <param name="space">
        /// The space.
        /// </param>
        /// <param name="isEmptyElement">
        /// The is empty element.
        /// </param>
        public ElementTokenDetails(XmlSpace space, bool isEmptyElement)
        {
            this.Space = space;
            this.IsEmptyElement = isEmptyElement;
        }

        /// <summary>
        /// Gets a value indicating how space should be treated in the element.
        /// </summary>
        public XmlSpace Space { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the element is empty (i.e. no child nodes, even white space).
        /// </summary>
        public bool IsEmptyElement { get; private set; }

        /// <summary>
        /// Gets or sets the number of attributes in the element.
        /// </summary>
        public int AttributeCount { get; set; }
    }
}