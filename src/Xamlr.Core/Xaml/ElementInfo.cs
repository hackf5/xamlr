// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElementInfo.cs" company="Xamlr">
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
//   Defines the ElementInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Xaml
{
    /// <summary>
    /// Encapsulates information about an XML element.
    /// </summary>
    public class ElementInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementInfo"/> class. 
        /// </summary>
        /// <param name="name">
        /// The name of the element.
        /// </param>
        /// <param name="depth">
        /// The depth of the element.
        /// </param>
        public ElementInfo(string name, int depth)
        {
            this.Name = name;
            this.Depth = depth;
            this.AttributesInline = true;
        }

        /// <summary>
        /// Gets the name of the element.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the depth of the element.
        /// </summary>
        public int Depth { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the element has content.
        /// </summary>
        public bool HasContent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to preserve space.
        /// </summary>
        public bool PreserveSpace { get; set; }
        
        /// <summary>
        /// Gets or sets the number of attributes on the element.
        /// </summary>
        public int AttributeCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the attributes are inline.
        /// </summary>
        public bool AttributesInline { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether content is rendered inline.
        /// </summary>
        public bool InlineContent { get; set; }
    }
}