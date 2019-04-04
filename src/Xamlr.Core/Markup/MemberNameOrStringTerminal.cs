// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberNameOrStringTerminal.cs" company="Xamlr">
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
//   A custom terminal that identifies and parses a Markup Extension STRING element.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Markup
{
    using System;
    using System.Diagnostics;
    using System.Text;
    using Irony.Parsing;
    using JetBrains.Annotations;

    /// <summary>
    /// A custom terminal that identifies and parses a Markup Extension MEMBERNAME or STRING element.
    /// </summary>
    /// <remarks>
    /// <para>
    /// There are two types of M/S terminal, quoted and unquoted. Quoted terminals are identified by being enclosed 
    /// in single quotes "'" (with those quotes being escaped as "\'"); if the end of file is encountered
    /// before this condition is met then an error is returned.
    /// Unquoted terminals terminate when either a comma "," is encountered, or the curly brace count, "{" = +1 and 
    /// "}" = -1, becomes strictly negative; if an equals sign "=" is encountered before this condition is met then
    /// terminal is not a STRING, and is instead a MEMBERNAME; if the end of file is encountered before any of these 
    /// conditions are met then an error is returned.
    /// </para>
    /// <para>
    /// A STRING token can itself be a Markup Extension; thus the value that is found is itself parsed: if this is
    /// successful the <see cref="ParseTree"/> is returned, otherwise the plain string value is returned.
    /// </para>
    /// </remarks>
    public class MemberNameOrStringTerminal : Terminal
    {
        /// <summary>
        /// The position when 
        /// <seealso cref="TryMatch"/>
        /// was entered.
        /// </summary>
        private int start;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberNameOrStringTerminal"/> class.
        /// </summary>
        /// <param name="name">
        /// The name that identifies the literal.
        /// </param>
        /// <param name="terminalType">
        /// The literal Type.
        /// </param>
        public MemberNameOrStringTerminal(string name, MemberNameOrStringTerminalType terminalType)
            : base(name)
        {
            this.TerminalType = terminalType;
        }

        /// <summary>
        /// Gets or sets the other terminal.
        /// </summary>
        /// <remarks>
        /// Since a value can only be identified as a MEMBERNAME or STRING once it has been parsed, then to save from
        /// double parsing certain values the other terminal is stored and returned.
        /// </remarks>
        public MemberNameOrStringTerminal OtherTerminal { get; set; }

        /// <summary>
        /// Gets the literal type.
        /// </summary>
        public MemberNameOrStringTerminalType TerminalType { get; private set; }

        /// <summary>
        /// Tries to match the current position in the <paramref name="source"/> with the rules for a S/M terminal;
        /// returning null if there is no match, returning an error if the terminal is malformed and returning the 
        /// the appropriate MEMBERNAME or STRING terminal otherwise.
        /// </summary>
        /// <param name="context">
        /// The context in which the match is occuring.
        /// </param>
        /// <param name="source">
        /// The source to try to match.
        /// </param>
        /// <returns>
        /// Null if there is no match, an error token if the terminal is malformed and returning the appropriate 
        /// MEMBERNAME or STRING terminal otherwise.
        /// otherwise.
        /// </returns>
        public override Token TryMatch([NotNull] ParsingContext context, [NotNull] ISourceStream source)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            Debug.Assert(this.OtherTerminal != null, "Other terminal has not been set");
            this.start = source.PreviewPosition;

            var isQuoted = false;
            Token token;
            if (source.PreviewChar == '\'')
            {
                token = this.TryMatchQuoted(context, source);
                isQuoted = true;
            }
            else
            {
                token = this.TryMatchUnquoted(context, source);
            }

            if (token != null && !token.IsError())
            {
                token.Details =
                    new MarkupExtensionTokenDetails(
                        source.Text.Substring(this.start, source.PreviewPosition - this.start), isQuoted);
            }

            return token;
        }

        /// <summary>
        /// Creates a MEMBERNAME token.
        /// </summary>
        /// <param name="source">
        /// The source from which to create the token.
        /// </param>
        /// <param name="builder">
        /// The builder that contains the MEMBERNAME value.
        /// </param>
        /// <returns>
        /// The <see cref="Token"/>.
        /// </returns>
        private Token CreateMemberNameToken(ISourceStream source, StringBuilder builder)
        {
            var terminal = this.TerminalType == MemberNameOrStringTerminalType.String
                ? this.OtherTerminal.OutputTerminal
                : this.OutputTerminal;

            return source.CreateToken(terminal, builder.ToString().Trim());
        }

        /// <summary>
        /// Creates a STRING token.
        /// </summary>
        /// <param name="context">
        /// The context in which the parse is occuring.
        /// </param>
        /// <param name="source">
        /// The source from which to create the token.
        /// </param>
        /// <param name="builder">
        /// The builder that contains the STRING value.
        /// </param>
        /// <returns>
        /// The STRING <see cref="Token"/>.
        /// </returns>
        private Token CreateStringToken(ParsingContext context, ISourceStream source, StringBuilder builder)
        {
            var terminal = this.TerminalType == MemberNameOrStringTerminalType.MemberName
                ? this.OtherTerminal.OutputTerminal
                : this.OutputTerminal;

            var text = builder.ToString();
            var parseTree = new Parser(context.Language).Parse(text);
            var value = parseTree.Status == ParseTreeStatus.Parsed ? (object)parseTree : text;

            return source.CreateToken(terminal, value);
        }

        /// <summary>
        /// Tries to match a quoted string from the source: this has been identified because the first character is a 
        /// single quote.
        /// </summary>
        /// <param name="context">
        /// The context in which the match is occuring.
        /// </param>
        /// <param name="source">
        /// The source to try to match.
        /// </param>
        /// <returns>
        /// Null if there is no match, an error token if the terminal is malformed and the appropriate token otherwise.
        /// </returns>
        private Token TryMatchQuoted(ParsingContext context, ISourceStream source)
        {
            ++source.PreviewPosition;
            var isClosed = false;
            var builder = new StringBuilder();
            Token token = null;
            while (token == null)
            {
                if (source.EOF())
                {
                    return context.CreateErrorToken("Malformed MEMBERNAME or STRING: no terminal encountered");
                }

                switch (source.PreviewChar)
                {
                    case '\\':
                        ++source.PreviewPosition;
                        builder.Append(source.PreviewChar);
                        break;
                    case '\'':
                        isClosed = true;
                        break;
                    default:
                        if (!isClosed)
                        {
                            builder.Append(source.PreviewChar);
                            break;
                        }

                        if (char.IsWhiteSpace(source.PreviewChar))
                        {
                            break;
                        }

                        if (source.PreviewChar == '=')
                        {
                            token = this.CreateMemberNameToken(source, builder);
                            break;
                        }

                        token = this.CreateStringToken(context, source, builder);
                        break;
                }

                source.PreviewPosition++;
            }

            return token;
        }

        /// <summary>
        /// Tries to match an unquoted string from the source: this has been identified because the first character is
        /// not a single quote.
        /// </summary>
        /// <param name="context">
        /// The context in which the match is occuring.
        /// </param>
        /// <param name="source">
        /// The source to try to match.
        /// </param>
        /// <returns>
        /// Null if there is no match, an error token if the terminal is malformed and the appropriate token otherwise.
        /// </returns>
        private Token TryMatchUnquoted(ParsingContext context, ISourceStream source)
        {
            var openBraceCount = 0;
            var builder = new StringBuilder();
            while (true)
            {
                if (source.EOF())
                {
                    return context.CreateErrorToken("Malformed MEMBERNAME or STRING: no terminal encountered");
                }

                switch (source.PreviewChar)
                {
                    case '\\':
                        ++source.PreviewPosition;
                        break;
                    case '\'':
                        return context.CreateErrorToken("Malformed MEMBERNAME or STRING: unescaped quote encountered");
                    case '=':
                        if (openBraceCount <= 0)
                        {
                            return this.CreateMemberNameToken(source, builder);
                        }

                        break;
                    case ',':
                        if (openBraceCount <= 0)
                        {
                            return this.CreateStringToken(context, source, builder);
                        }

                        break;
                    case '{':
                        ++openBraceCount;
                        break;
                    case '}':
                        if (openBraceCount-- <= 0)
                        {
                            return this.CreateStringToken(context, source, builder);
                        }

                        break;
                }

                builder.Append(source.PreviewChar);
                ++source.PreviewPosition;
            }
        }
    }
}