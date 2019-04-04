// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormattingContext.cs" company="Xamlr">
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
//   Defines the FormattingContext type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Xaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    /// <summary>
    /// The formatting context.
    /// </summary>
    public class FormattingContext
    {
        /// <summary>
        /// The elements.
        /// </summary>
        private readonly Stack<ElementInfo> elements;

        /// <summary>
        /// The formatter factory.
        /// </summary>
        private readonly FormatterFactory formatterFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormattingContext"/> class. 
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public FormattingContext(XamlFormatterOptions options)
        {
            this.Options = options ?? throw new ArgumentNullException(nameof(options));
            this.formatterFactory = new FormatterFactory(this);
            this.elements = new Stack<ElementInfo>();
            this.elements.Push(new ElementInfo("Root", 0));
        }

        /// <summary>
        /// Gets the depth.
        /// </summary>
        public int Depth => this.Element.Depth + this.LocalDepth;

        /// <summary>
        /// Gets the current element.
        /// </summary>
        public ElementInfo Element => this.elements.Peek();

        /// <summary>
        /// Gets or sets the current local depth.
        /// </summary>
        public int LocalDepth { get; set; }

        /// <summary>
        /// Gets the options.
        /// </summary>
        public XamlFormatterOptions Options { get; private set; }

        /// <summary>
        /// Gets or sets the tokens.
        /// </summary>
        public FragmentTokenSource TokenSource { get; set; }

        /// <summary>
        /// Creates a formatter that can format a given <see cref="FragmentType"/>.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// A formatter that can format a given <see cref="FragmentType"/>.
        /// </returns>
        public IFormatter Create(FragmentType type) => this.formatterFactory.Create(type);

        /// <summary>
        /// Enters the element described by <paramref name="token"/>.
        /// </summary>
        /// <param name="token">
        /// The token describing the element.
        /// </param>
        public void EnterElement(FragmentToken token)
        {
            if (this.elements.Any())
            {
                this.Element.HasContent = true;
                this.Element.InlineContent = false;
            }

            var details = (ElementTokenDetails)token.Content;
            var element = new ElementInfo(token.Name, this.Element.Depth + 1)
            {
                PreserveSpace =
                    details.Space == XmlSpace.Preserve
                    || details.Space == XmlSpace.None && this.Element.PreserveSpace,
                AttributeCount = details.AttributeCount,
            };

            this.elements.Push(element);
            this.LocalDepth = 0;
        }

        /// <summary>
        /// Instructs the context to leave the current element.
        /// </summary>
        public void LeaveElement()
        {
            this.elements.Pop();
        }
    }
}