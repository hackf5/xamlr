// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarkupExensionInfoBuilder.cs" company="Xamlr">
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
//   Defines the MarkupExensionInfoBuilder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Test.Markup
{
    using System;
    using System.Collections.Generic;
    using Xamlr.Core.Markup;

    /// <summary>
    /// A utility class for building <see cref="IMarkupExtensionInfo"/> instances in code.
    /// </summary>
    internal class MarkupExensionInfoBuilder
    {
        /// <summary>
        /// The current trail of nested markup extensions.
        /// </summary>
        private readonly Stack<Tuple<string, List<IMarkupExtensionArgumentInfo>>> trail =
            new Stack<Tuple<string, List<IMarkupExtensionArgumentInfo>>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkupExensionInfoBuilder"/> class.
        /// </summary>
        /// <param name="typeName">
        /// The type name of the markup extension.
        /// </param>
        public MarkupExensionInfoBuilder(string typeName)
        {
            if (typeName == null)
            {
                throw new ArgumentNullException(nameof(typeName));
            }

            this.BeginNested(typeName);
        }

        /// <summary>
        /// Adds a positional argument to the current level.
        /// </summary>
        /// <param name="string">
        /// The string value of the argument.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the markup extension.
        /// </returns>
        public MarkupExensionInfoBuilder AddArgument(object @string) => this.AddArgument(null, @string, false);

        /// <summary>
        /// Adds a named argument to the current level.
        /// </summary>
        /// <param name="memberName">
        /// The member name of the argument.
        /// </param>
        /// <param name="string">
        /// The string value of the argument.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the markup extension.
        /// </returns>
        public MarkupExensionInfoBuilder AddArgument(string memberName, object @string) => this.AddArgument(memberName, @string, false);

        /// <summary>
        /// Adds a named argument to the current level.
        /// </summary>
        /// <param name="memberName">
        /// The member name of the argument.
        /// </param>
        /// <param name="string">
        /// The string value of the argument.
        /// </param>
        /// <param name="isQuoted">
        /// A value indicating whether the string is quoted.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the markup extension.
        /// </returns>
        public MarkupExensionInfoBuilder AddArgument(string memberName, object @string, bool isQuoted)
        {
            var argument = new MarkupExtensionArgumentInfo(
                memberName,
                new MarkupExtensionTokenDetails(string.Empty, false),
                @string,
                new MarkupExtensionTokenDetails(string.Empty, isQuoted));

            this.trail.Peek().Item2.Add(argument);

            return this;
        }

        /// <summary>
        /// Begins a new nested markup extension.
        /// </summary>
        /// <param name="typeName">
        /// The type name of the nested markup extension. 
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the markup extension.
        /// </returns>
        public MarkupExensionInfoBuilder BeginNested(string typeName)
        {
            this.trail.Push(Tuple.Create(typeName, new List<IMarkupExtensionArgumentInfo>()));

            return this;
        }

        /// <summary>
        /// Builds the <see cref="IMarkupExtensionInfo"/> configured by this builder.
        /// </summary>
        /// <returns>
        /// The <see cref="IMarkupExtensionInfo"/> configured by this builder.
        /// </returns>
        public IMarkupExtensionInfo Build()
        {
            if (this.trail.Count != 1)
            {
                throw new InvalidOperationException("Unbalanced number of Begin and End statements.");
            }

            return this.BuildCrumb();
        }

        /// <summary>
        /// Ends the current nested markup extension.
        /// </summary>
        /// <param name="memberName">
        /// The member name with which the markup extension should be added to the set of parent arguments.
        /// </param>
        /// <param name="isQuoted">
        /// A value indicating whether the argument should be quoted.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the markup extension.
        /// </returns>
        public MarkupExensionInfoBuilder EndNested(string memberName = null, bool isQuoted = false)
        {
            this.AddArgument(memberName, this.BuildCrumb(), isQuoted);

            return this;
        }

        /// <summary>
        /// Builds the <see cref="IMarkupExtensionInfo"/> configured by this builder at the current level.
        /// </summary>
        /// <returns>
        /// The <see cref="IMarkupExtensionInfo"/> configured by this builder at the current level.
        /// </returns>
        private IMarkupExtensionInfo BuildCrumb()
        {
            var nested = this.trail.Pop();
            var typeName = nested.Item1;
            var arguments = nested.Item2;

            return new MarkupExtensionInfo(typeName, arguments);
        }
    }
}