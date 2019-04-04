// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarkupExtensionParserTests.cs" company="Xamlr">
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
//   Defines the MarkupExtensionParserTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Test.Markup
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Xamlr.Core.Markup;

    /// <summary>
    /// This class tests the <see cref="MarkupExtensionParser"/> class.
    /// <para>
    /// The testing strategy here is pure integration. Underneath the parser are a lot of moving parts and
    /// core to these is the Irony parser. Given the blackbox nature of Irony there is little value in having lots
    /// of low level tests that mock Irony out; a more effective testing strategy is to execute a number of simple
    /// tests that hit the obvious cases and then to throw some really nasty bindings at the parser in an attempt 
    /// to break it.
    /// </para> 
    /// </summary>
    [TestClass]
    public class MarkupExtensionParserTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Parse_NullSource_Throws()
        {
            // Arrange
            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            // ReSharper disable AssignNullToNotNullAttribute
            target.Parse(null);
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [TestMethod]
        [TestCategory("Integration")]
        [ExpectedException(typeof(ArgumentException))]
        public void Parse_EmptySource_Throws()
        {
            // Arrange
            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            target.Parse(string.Empty);
        }

        [TestMethod]
        [TestCategory("Integration")]
        [ExpectedException(typeof(ArgumentException))]
        public void Parse_WhitespaceSource_Throws()
        {
            // Arrange
            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            target.Parse(new string(' ', 10));
        }

        [TestMethod]
        [TestCategory("Integration")]
        [ExpectedException(typeof(FormatException))]
        public void Parse_SingleOpenBrace_Throws()
        {
            // Arrange
            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            target.Parse("{");
        }

        [TestMethod]
        [TestCategory("Integration")]
        [ExpectedException(typeof(FormatException))]
        public void Parse_SingleCloseBrace_Throws()
        {
            // Arrange
            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            target.Parse("}");
        }

        [TestMethod]
        [TestCategory("Integration")]
        [ExpectedException(typeof(FormatException))]
        public void Parse_NoTypeName_Throws()
        {
            // Arrange
            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            target.Parse("{}");
        }

        [TestMethod]
        [TestCategory("Integration")]
        [ExpectedException(typeof(FormatException))]
        public void Parse_NoTypeNameWithWhiteSpace_Throws()
        {
            // Arrange
            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            target.Parse("{  }");
        }

        [TestMethod]
        [TestCategory("Integration")]
        [ExpectedException(typeof(FormatException))]
        public void Parse_ArbitraryString_Throws()
        {
            // Arrange
            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            target.Parse("arbitrary string");
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void TryParse_NullSource_False()
        {
            // Arrange
            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            // ReSharper disable AssignNullToNotNullAttribute
            IMarkupExtensionInfo info;
            var result = target.TryParse(null, out info);
            // ReSharper restore AssignNullToNotNullAttribute

            // Assert
            Assert.IsFalse(result);
        }
        
        [TestMethod]
        [TestCategory("Integration")]
        public void TryParse_EmptySource_False()
        {
            // Arrange
            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            IMarkupExtensionInfo info;
            var result = target.TryParse(string.Empty, out info);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void TryParse_WhitespaceSource_False()
        {
            // Arrange
            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            IMarkupExtensionInfo info;
            var result = target.TryParse(new string(' ', 10), out info);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void TryParse_ArbitraryString_False()
        {
            // Arrange
            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            IMarkupExtensionInfo info;
            var result = target.TryParse("arbitrary string", out info);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Parse_TypeNameOnlyWithLeadingWhitespace_ParsesAsExpected()
        {
            // Arrange
            var markup = "{      Binding}";
            var expected = new MarkupExensionInfoBuilder("Binding").Build();
            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            var actual = target.Parse(markup);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Parse_TypeNameOnlyWithTrailingWhitespace_ParsesAsExpected()
        {
            // Arrange
            var markup = "{Binding   }";
            var expected = new MarkupExensionInfoBuilder("Binding").Build();
            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            var actual = target.Parse(markup);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Parse_SinglePositionalQuotedEmptyArgument_ParsesAsExpected()
        {
            // Arrange
            var markup = "{Binding ''}";
            var expected = new MarkupExensionInfoBuilder("Binding").AddArgument(null, string.Empty, true).Build();
            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            var actual = target.Parse(markup);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Parse_SinglePositionalQuotedArgument_ParsesAsExpected()
        {
            // Arrange
            var markup = "{Binding '.'}";
            var expected = new MarkupExensionInfoBuilder("Binding").AddArgument(null, ".", true).Build();
            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            var actual = target.Parse(markup);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Parse_SinglePositionalStringArgument_ParsesAsExpected()
        {
            // Arrange
            var markup = "{Binding .}";
            var expected = new MarkupExensionInfoBuilder("Binding").AddArgument(".").Build();
            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            var actual = target.Parse(markup);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Parse_SinglePositionalQuotedStringArgument_ParsesAsExpected()
        {
            // Arrange
            var markup = "{Binding '.'}";
            var expected = new MarkupExensionInfoBuilder("Binding").AddArgument(null, ".", true).Build();
            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            var actual = target.Parse(markup);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Parse_SinglePositionalNestedStringArgument_ParsesAsExpected()
        {
            // Arrange
            var markup = "{Binding {Nested}}";
            var expected = new MarkupExensionInfoBuilder("Binding").BeginNested("Nested").EndNested().Build();
            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            var actual = target.Parse(markup);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Parse_SinglePositionalQuotedNestedStringArgument_ParsesAsExpected()
        {
            // Arrange
            var markup = "{Binding '{Nested}'}";
            var expected = new MarkupExensionInfoBuilder("Binding").BeginNested("Nested").EndNested(null, true).Build();
            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            var actual = target.Parse(markup);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Parse_SinglePositionalQuotedNestedStringArgumentWithWhitespace_ParsesAsExpected()
        {
            // Arrange
            var markup = "{     Binding   '   {    Nested    }     '}";
            var expected = new MarkupExensionInfoBuilder("Binding").BeginNested("Nested").EndNested(null, true).Build();
            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            var actual = target.Parse(markup);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Parse_StringFormat_ParsesAsExpected()
        {
            // Arrange
            var markup = "{Binding StringFormat={}{0} Foobar}";
            var expected = new MarkupExensionInfoBuilder("Binding").AddArgument("StringFormat", "{}{0} Foobar").Build();
            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            var actual = target.Parse(markup);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Parse_MultipleUnNestedBindings_ParsesAsExpected()
        {
            // Arrange
            var markup =
@"{Binding
    Arg1,
    Arg2,
    {}{0} Ooo what a nasty string,
    'stob your gabbin\' boy',
    StringFormat={     }{0} Foobar,
    'QuotedMember=Name'=Jake & Juno}";

            var expected = new MarkupExensionInfoBuilder("Binding")
                .AddArgument("Arg1")
                .AddArgument("Arg2")
                .AddArgument("{}{0} Ooo what a nasty string")
                .AddArgument(null, "stob your gabbin' boy", true)
                .AddArgument("StringFormat", "{     }{0} Foobar")
                .AddArgument("QuotedMember=Name", "Jake & Juno")
                .Build();

            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            var actual = target.Parse(markup);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Parse_TypeNameHasColon_ParsesAsExpected()
        {
            // Arrange
            var markup = @"{d:DesignInstance}";

            var expected = new MarkupExensionInfoBuilder("d:DesignInstance").Build();

            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            IMarkupExtensionInfo actual;
            var result = target.TryParse(markup, out actual);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Parse_ComplexBinding1_ParsesAsExpected()
        {
            // Arrange
            var markup =
@"{Binding
    Path=Foo,
    ElementName={Binding
        Path=DataContext.Girrafe,
        Converter={StaticResource GirrafeConverter}},
    Source={StaticResource Items}}";

            var expected = new MarkupExensionInfoBuilder("Binding")
                .AddArgument("Path", "Foo")
                .BeginNested("Binding")
                    .AddArgument("Path", "DataContext.Girrafe")
                    .BeginNested("StaticResource")
                        .AddArgument("GirrafeConverter")
                    .EndNested("Converter")
                .EndNested("ElementName")
                .BeginNested("StaticResource")
                    .AddArgument("Items")
                .EndNested("Source")
                .Build();

            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            var actual = target.Parse(markup);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void TryParse_ComplexBinding1_ParsesAsExpected()
        {
            // Arrange
            var markup =
@"{Binding
    Path=Foo,
    ElementName={Binding
        Path=DataContext.Girrafe,
        Converter={StaticResource GirrafeConverter}},
    Source={StaticResource Items}}";

            var expected = new MarkupExensionInfoBuilder("Binding")
                .AddArgument("Path", "Foo")
                .BeginNested("Binding")
                    .AddArgument("Path", "DataContext.Girrafe")
                    .BeginNested("StaticResource")
                        .AddArgument("GirrafeConverter")
                    .EndNested("Converter")
                .EndNested("ElementName")
                .BeginNested("StaticResource")
                    .AddArgument("Items")
                .EndNested("Source")
                .Build();

            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            IMarkupExtensionInfo actual;
            var result = target.TryParse(markup, out actual);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void TryParse_ComplexBinding2_ParsesAsExpected()
        {
            // Arrange
            var markup =
@"{Binding 
    Items, 
    Source={d:DesignInstance
        IsDesignTimeCreatable=True,
        Type=ViewModels:AppHubViewModel}}";

            var expected = new MarkupExensionInfoBuilder("Binding")
                .AddArgument("Items")
                .BeginNested("d:DesignInstance")
                    .AddArgument("IsDesignTimeCreatable", "True")
                    .AddArgument("Type", "ViewModels:AppHubViewModel")
                .EndNested("Source")
                .Build();

            var target = MarkupExtensionParserTests.CreateTarget();

            // Act
            IMarkupExtensionInfo actual;
            var result = target.TryParse(markup, out actual);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(expected, actual);
        }

        private static MarkupExtensionParser CreateTarget()
        {
            return new MarkupExtensionParser();
        }
    }
}