// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormattingExtensions.cs" company="Xamlr">
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
//   Defines the FormattingExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Xaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml;
    using JetBrains.Annotations;
    using Xamlr.Core.Markup;

    /// <summary>
    /// Extension methods used for formatting.
    /// </summary>
    /// <remarks>
    /// This is a bit of a ragbag of methods, but there are not enough of them at the moment to add any value by 
    /// separating them out.
    /// </remarks>
    public static class FormattingExtensions
    {
        /// <summary>
        /// A dictionary of character values to remplace in XML.
        /// </summary>
        private static readonly Dictionary<char, string> XmlReplace = new Dictionary<char, string>
        {
            { '&', "&amp;" }, { '<', "&lt;" }, { '>', "&gt;" }, { '"', "&quot;" },
        };

        /// <summary>
        /// TAppends a line selectively to the builder depending on the current state of the formatting context.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The original <see cref="StringBuilder"/>.
        /// </returns>
        public static StringBuilder AppendLine(this StringBuilder builder, FormattingContext context) => context.Element.PreserveSpace ? builder : builder.AppendLine();

        /// <summary>
        /// Gets a value indicating whether the markup exension can be inlined.
        /// </summary>
        /// <param name="markup">
        /// The markup extension.
        /// </param>
        /// <returns>
        /// A value indicating whether the markup exension can be inlined.
        /// </returns>
        public static bool CanInline([NotNull] this IMarkupExtensionInfo markup)
        {
            if (markup == null)
            {
                throw new ArgumentNullException(nameof(markup));
            }

            return !markup.HasNestedArguments && markup.Arguments.Count() < 2;
        }

        /// <summary>
        /// Gets a blank string that will indent to the current formatting depth.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="respectWhiteSpace">
        /// A value indicating whether to respect white space.
        /// </param>
        /// <returns>
        /// A blank string that will indent to the current formatting depth.
        /// </returns>
        public static string GetIndent(this FormattingContext context, bool respectWhiteSpace = true)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return respectWhiteSpace && context.Element.PreserveSpace ? string.Empty : context.GetIndent(context.Depth);
        }

        /// <summary>
        /// Gets a blank string that will indent to depth <paramref name="depth"/>.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="depth">
        /// The depth.
        /// </param>
        /// <returns>
        /// A blank string that will indent to depth <paramref name="depth"/>.
        /// </returns>
        public static string GetIndent(this FormattingContext context, int depth)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (depth < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(depth), depth, @"Must not be less than zero.");
            }

            return new string(' ', context.Options.IndentSize * depth);
        }

        /// <summary>
        /// Gets the lines from a string where each line is trimmed and duplicate blank lines are compressed.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <returns>
        /// The lines from a string where each line is trimmed and duplicate blank lines are compressed.
        /// </returns>
        public static IEnumerable<string> GetLines([NotNull] this string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var lines = new List<string>();
            foreach (var element in text.Trim().Split('\n').Select(x => x.Trim()))
            {
                if (!lines.Any() || !(string.Empty.Equals(element) && lines.Last() == element))
                {
                    lines.Add(element);
                }
            }

            return lines;
        }

        /// <summary>
        /// Indents the builder to the current formatting depth.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="respectWhiteSpace">
        /// A value indicating whether to respect white space.
        /// </param>
        /// <returns>
        /// The original <see cref="StringBuilder"/>.
        /// </returns>
        public static StringBuilder Indent(
            this StringBuilder builder, FormattingContext context, bool respectWhiteSpace = true) =>
            builder.Append(context.GetIndent(respectWhiteSpace));

        /// <summary>
        /// Removes characters from the end of the <paramref name="builder"/> while the 
        /// <paramref name="predicate"/> is satisfied.
        /// </summary>
        /// <param name="builder">
        /// The builder from which to remove.
        /// </param>
        /// <param name="predicate">
        /// The predicate to satisfy.
        /// </param>
        /// <returns>
        /// The original <see cref="StringBuilder"/>.
        /// </returns>
        public static StringBuilder RemoveEndWhile(this StringBuilder builder, Func<char, bool> predicate)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            while (builder.Length > 0 && predicate(builder[builder.Length - 1]))
            {
                builder.Remove(builder.Length - 1, 1);
            }

            return builder;
        }

        /// <summary>
        /// Converts a <see cref="XmlNodeType"/> to a <see cref="FragmentType"/>.
        /// </summary>
        /// <param name="nodeType">
        /// The node type.
        /// </param>
        /// <returns>
        /// The <see cref="FragmentType"/> that corresponds to <paramref name="nodeType"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when there is no corresponding <see cref="FragmentType"/>.
        /// </exception>
        public static FragmentType ToFragmentType(this XmlNodeType nodeType)
        {
            FragmentType fragmentType;
            if (!Enum.TryParse(nodeType.ToString(), true, out fragmentType))
            {
                throw new InvalidOperationException(
                    $"No fragment type for a node of type '{nodeType}' exists");
            }

            return fragmentType;
        }

        /// <summary>
        /// Normalizes the line endings of a string.
        /// </summary>
        /// <param name="raw">
        /// The raw string.
        /// </param>
        /// <returns>
        /// The normalized string.
        /// </returns>
        public static string ToNormalizedLineEndings(this string raw) => Regex.Replace(raw, @"\r\n|\n\r|\n|\r", "\r\n", RegexOptions.Compiled);

        /// <summary>
        /// Converts a raw string to an XML encoded string.
        /// </summary>
        /// <param name="raw">
        /// The raw string.
        /// </param>
        /// <param name="preserveWhiteSpace">
        /// A value indicating whether to preserve white space.
        /// </param>
        /// <returns>
        /// The XML encoded string.
        /// </returns>
        public static string ToXmlEncoded(this string raw, bool preserveWhiteSpace = true)
        {
            if (raw == null)
            {
                throw new ArgumentNullException(nameof(raw));
            }

            var builder = new StringBuilder(raw.Length);
            var inWhiteSpace = false;
            foreach (var @char in raw)
            {
                if (!preserveWhiteSpace && char.IsWhiteSpace(@char))
                {
                    if (!inWhiteSpace)
                    {
                        inWhiteSpace = true;
                        builder.Append(' ');
                    }

                    continue;
                }

                if (FormattingExtensions.XmlReplace.ContainsKey(@char))
                {
                    builder.Append(FormattingExtensions.XmlReplace[@char]);
                }
                else
                {
                    builder.Append(@char);
                }

                inWhiteSpace = false;
            }

            return builder.ToString();
        }
    }
}