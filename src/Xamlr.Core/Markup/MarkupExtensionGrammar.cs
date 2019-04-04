// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarkupExtensionGrammar.cs" company="Xamlr">
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
//   The Markup Extension grammar.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Markup
{
    using Irony.Parsing;

    /// <summary>
    /// The Markup Extension grammar.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The grammar is defined in ABNF notation as:.
    /// </para>
    /// <para>
    /// MarkupExtension = "{" TYPENAME 0*1Arguments "}"
    ///     Arguments       = (NamedArgs / (PositionalArgs 0*1("," NamedArgs))
    ///     NamedArgs       = NamedArg *("," NamedArg)
    ///     NamedArg        = MEMBERNAME "=" STRING
    ///     PositionalArgs  = NamedArg / (STRING 0*1("," PositionalArgs)).
    /// </para>
    /// <para>
    /// Additionally there are some further rules on the identification of the TYPENAME, MEMBERNAME and STRING tokens
    /// that can be found here: http://msdn.microsoft.com/en-us/library/ee200269.aspx. In relation to the grammar
    /// definition it says that a STRING is identified by not being a MEMBERNAME and both have slightly complex parsing
    /// rules associated with them, by contrast a TYPENAME has a very permisve set of rules; as such it has been
    /// necessary to define a two custom terminals to parse them and emit the appropriate token.
    /// </para>
    /// </remarks>
    [Language("XamlMarkupExtension", "1.0", "Xaml Markup Extension")]
    public class MarkupExtensionGrammar : Grammar
    {
        /// <summary>
        /// The member name component identifier.
        /// </summary>
        public const string MemberNameIdentifier = "MEMBERNAME";

        /// <summary>
        /// The string component identifier.
        /// </summary>
        public const string StringIdentifier = "STRING";

        /// <summary>
        /// The type name component identifier.
        /// </summary>
        public const string TypeNameIdentifier = "TYPENAME";

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkupExtensionGrammar"/> class.
        /// </summary>
        public MarkupExtensionGrammar()
        {
            var markupExtension = new NonTerminal("MarkupExtension");
            var arguments = new NonTerminal("Arguments");
            var namedArgs = new NonTerminal("NamedArgs");
            var namedArg = new NonTerminal("NamedArg");
            var positionalArgs = new NonTerminal("PositionalArgs");

            var typeName = new TypeNameTerminal(MarkupExtensionGrammar.TypeNameIdentifier);
            var memberName = new MemberNameOrStringTerminal(MarkupExtensionGrammar.MemberNameIdentifier, MemberNameOrStringTerminalType.MemberName);
            var @string = new MemberNameOrStringTerminal(MarkupExtensionGrammar.StringIdentifier, MemberNameOrStringTerminalType.String);

            memberName.OtherTerminal = @string;
            @string.OtherTerminal = memberName;

            markupExtension.Rule = "{" + typeName + arguments.Q() + "}";
            arguments.Rule = namedArgs | (positionalArgs + ("," + namedArgs).Q());
            namedArgs.Rule = this.MakeStarRule(namedArgs, this.ToTerm(","), namedArg);
            namedArg.Rule = memberName + "=" + @string;
            positionalArgs.Rule = namedArg | (@string + ("," + positionalArgs).Q());

            this.Root = markupExtension;
        }
    }
}