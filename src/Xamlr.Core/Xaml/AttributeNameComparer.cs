// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeNameComparer.cs" company="Xamlr">
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
//   Defines the AttributeNameComparer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Xaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The attribute name comparer.
    /// </summary>
    public class AttributeNameComparer : IComparer<string>
    {
        /// <summary>
        /// The default string comparer.
        /// </summary>
        private static readonly StringComparer Comparer = StringComparer.Ordinal;

        /// <summary>
        /// The rules to use for comparison.
        /// </summary>
        private static readonly List<StringMatchRule> Rules = new List<StringMatchRule>
        {
            new StringMatchRule("x:Class", StringMatchType.Exact), 
            new StringMatchRule("x:Name", StringMatchType.Exact),
            new StringMatchRule("x:Key", StringMatchType.Exact),
            new StringMatchRule("Name", StringMatchType.Exact), 
            new StringMatchRule("Title", StringMatchType.Exact), 
            new StringMatchRule("xml:", StringMatchType.Prefix), 
            new StringMatchRule("x:", StringMatchType.Prefix), 
            new StringMatchRule("xmlns", StringMatchType.Exact), 
            new StringMatchRule("xmlns:x", StringMatchType.Exact), 
            new StringMatchRule("xmlns:mc", StringMatchType.Exact), 
            new StringMatchRule("xmlns:d", StringMatchType.Exact), 
            new StringMatchRule("xmlns:", StringMatchType.Prefix), 
            new StringMatchRule("Grid.", StringMatchType.Prefix), 
            new StringMatchRule("DockPanel.", StringMatchType.Prefix), 
            new StringMatchRule("Canvas.", StringMatchType.Prefix), 
            new StringMatchRule("Height", StringMatchType.Exact), 
            new StringMatchRule("MinHeight", StringMatchType.Exact), 
            new StringMatchRule("MaxHeight", StringMatchType.Exact), 
            new StringMatchRule("Width", StringMatchType.Exact), 
            new StringMatchRule("MinWidth", StringMatchType.Exact), 
            new StringMatchRule("MaxWidth", StringMatchType.Exact), 
            new StringMatchRule("Margin", StringMatchType.Exact),
            new StringMatchRule("Padding", StringMatchType.Exact), 
            new StringMatchRule("HorizontalAlignment", StringMatchType.Exact), 
            new StringMatchRule("HorizontalContentAlignment", StringMatchType.Exact), 
            new StringMatchRule("VerticalAlignment", StringMatchType.Exact), 
            new StringMatchRule("VerticalContentAlignment", StringMatchType.Exact), 
            new StringMatchRule(".ZIndex", StringMatchType.Suffix)
        };

        private readonly IList<StringMatchRule> rules;

        public AttributeNameComparer()
        {
            this.rules = Rules;
        }

        public AttributeNameComparer(IEnumerable<string> overrides)
        {
            this.rules = new List<StringMatchRule>(
                overrides.Select(item => new StringMatchRule(item, StringMatchType.Exact)).Concat(Rules));
        }

        /// <summary>
        /// Compares <paramref name="x"/> with <paramref name="y"/>.
        /// </summary>
        /// <param name="x">
        /// The x value.
        /// </param>
        /// <param name="y">
        /// The y value.
        /// </param>
        /// <returns>
        /// An integer, the sign of which determines the ordering of <paramref name="x"/> and <paramref name="y"/>.
        /// </returns>
        public int Compare(string x, string y)
        {
            var xorder = this.GetOrder(x);
            var yorder = this.GetOrder(y);

            return xorder == yorder ? Comparer.Compare(x, y) : Math.Sign(xorder - yorder);
        }

        /// <summary>
        /// Gets the order of the first rule that applies to <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The order of the first rule that applies to <paramref name="value"/>.
        /// </returns>
        private int GetOrder(string value)
        {
            for (var order = 0; order < this.rules.Count; ++order)
            {
                if (this.rules[order].IsMatch(value))
                {
                    return order;
                }
            }

            return int.MaxValue;
        }
    }
}