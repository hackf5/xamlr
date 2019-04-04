// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarkupExtensionParser.cs" company="Xamlr">
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
//   Defines the MarkupExtensionParser type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Markup
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Irony.Parsing;

    /// <summary>
    /// The markup extension parser.
    /// </summary>
    public class MarkupExtensionParser : IMarkupExtensionParser
    {
        /// <summary>
        /// The markup extension grammar.
        /// </summary>
        private static readonly Grammar Grammar = new MarkupExtensionGrammar();

        /// <summary>
        /// The language defined by the grammar.
        /// </summary>
        private static readonly LanguageData Language = new LanguageData(MarkupExtensionParser.Grammar);

        /// <summary>
        /// Parses <paramref name="sourceText"/> to a <see cref="IMarkupExtensionInfo"/>.
        /// </summary>
        /// <param name="sourceText">
        /// The source text.
        /// </param>
        /// <returns>
        /// A <see cref="IMarkupExtensionInfo"/> representing the source text.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the source text is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the source text is white space.
        /// </exception>
        /// <exception cref="FormatException">
        /// Thrown when the source text is not a well formed markup extension.
        /// </exception>
        public IMarkupExtensionInfo Parse(string sourceText)
        {
            if (sourceText == null)
            {
                throw new ArgumentNullException(nameof(sourceText));
            }

            if (string.IsNullOrWhiteSpace(sourceText))
            {
                throw new ArgumentException("Source cannot be white space");
            }

            var parser = new Parser(MarkupExtensionParser.Language);
            var tree = parser.Parse(sourceText);

            return MarkupExtensionParser.Detokenize(tree);
        }

        /// <summary>
        /// Tries to parse <paramref name="sourceText"/> and returns a value indicating success. 
        /// </summary>
        /// <param name="sourceText">
        /// The source text.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// A value indicating whether the source was successfully parsed.
        /// </returns>
        public bool TryParse(string sourceText, out IMarkupExtensionInfo result)
        {
            result = null;

            if (string.IsNullOrWhiteSpace(sourceText))
            {
                return false;
            }

            var parser = new Parser(MarkupExtensionParser.Language);
            var tree = parser.Parse(sourceText);
            if (tree.Status != ParseTreeStatus.Parsed)
            {
                return false;
            }

            result = MarkupExtensionParser.Detokenize(tree);
            return true;
        }

        /// <summary>
        /// Detokenizes the parse tree <paramref name="tree"/> created by the irony parser.
        /// </summary>
        /// <param name="tree">
        /// The irony parse tree.
        /// </param>
        /// <returns>
        /// A <see cref="MarkupExtensionInfo"/> representing the parse tree.
        /// </returns>
        /// <exception cref="FormatException">
        /// Thrown when the parse tree is invalid.
        /// </exception>
        private static MarkupExtensionInfo Detokenize(ParseTree tree)
        {
            if (tree.Status != ParseTreeStatus.Parsed)
            {
                throw new FormatException("The source text was badly formed.");
            }

            var typeName = MarkupExtensionParser.DetokenizeTypeName(tree);
            var arguments = MarkupExtensionParser.DetokenizeArguments(tree);

            return new MarkupExtensionInfo(typeName, arguments);
        }

        /// <summary>
        /// Detokenizes the argument components from a parse tree.
        /// </summary>
        /// <param name="tree">
        /// The parse tree.
        /// </param>
        /// <returns>
        /// A collection of <see cref="IMarkupExtensionArgumentInfo"/> representing the arguments in the parse tree.
        /// </returns>
        private static IEnumerable<IMarkupExtensionArgumentInfo> DetokenizeArguments(ParseTree tree)
        {
            var positionalArguments = MarkupExtensionParser.ExtractPositionalArguments(tree);
            var namedArguments = MarkupExtensionParser.ExtractNamedArguments(tree);

            var arguments = positionalArguments
                .Concat(namedArguments)
                .Select(
                    item =>
                        new MarkupExtensionArgumentInfo(
                            item.Item1 != null ? (string)item.Item1.Value : null,
                            item.Item1 != null ? (MarkupExtensionTokenDetails)item.Item1.Details : null,
                            item.Item2.Value is ParseTree ? MarkupExtensionParser.Detokenize((ParseTree)item.Item2.Value) : item.Item2.Value,
                            (MarkupExtensionTokenDetails)item.Item2.Details))
                .ToArray();

            if (arguments.Length == 1 && arguments[0].IsEmpty)
            {
                arguments = new MarkupExtensionArgumentInfo[] { };
            }

            return arguments;
        }

        /// <summary>
        /// Detokenizes the type name component froma parse tree.
        /// </summary>
        /// <param name="tree">
        /// The parse tree.
        /// </param>
        /// <returns>
        /// The type name.
        /// </returns>
        private static string DetokenizeTypeName(ParseTree tree)
        {
            return (string)tree.Tokens
                .First(
                    candidate =>
                        candidate.Terminal.Name.Equals(
                            MarkupExtensionGrammar.TypeNameIdentifier, StringComparison.OrdinalIgnoreCase))
                .Value;
        }

        /// <summary>
        /// Extracts the named argument components from a parse tree.
        /// </summary>
        /// <param name="tree">
        /// The parse tree.
        /// </param>
        /// <returns>
        /// A collection of <see cref="Tuple"/> where the first item is the MEMBERNAME of the argument and the second
        /// item is the STRING literal that represents the positional argument.
        /// </returns>
        private static IEnumerable<Tuple<Token, Token>> ExtractNamedArguments(ParseTree tree)
        {
            return tree.Tokens
                .SkipWhile(
                    candidate =>
                        !candidate.Terminal.Name.Equals(
                            MarkupExtensionGrammar.MemberNameIdentifier, StringComparison.OrdinalIgnoreCase))
                .Where(
                    candidate =>
                        candidate.Terminal.Name.Equals(
                            MarkupExtensionGrammar.MemberNameIdentifier, StringComparison.OrdinalIgnoreCase)
                        || candidate.Terminal.Name.Equals(
                            MarkupExtensionGrammar.StringIdentifier, StringComparison.OrdinalIgnoreCase))
                .Select((token, index) => new { index, token })
                .GroupBy(item => item.index / 2)
                .Select(group => Tuple.Create(@group.ElementAt(0).token, @group.ElementAt(1).token));
        }

        /// <summary>
        /// Extracts the positional argument components from a parse tree.
        /// </summary>
        /// <param name="tree">
        /// The parse tree.
        /// </param>
        /// <returns>
        /// A collection of <see cref="Tuple"/> where the second item is the STRING literal that represents the 
        /// positional argument.
        /// </returns>
        private static IEnumerable<Tuple<Token, Token>> ExtractPositionalArguments(ParseTree tree)
        {
            return tree.Tokens
                .TakeWhile(
                    candidate =>
                        candidate.KeyTerm == null || !candidate.KeyTerm.Text.Equals("=", StringComparison.Ordinal))
                .Where(
                    candidate =>
                        candidate.Terminal.Name.Equals(
                            MarkupExtensionGrammar.StringIdentifier, StringComparison.OrdinalIgnoreCase))
                .Select(token => Tuple.Create(default(Token), token));
        }
    }
}