// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarkupExtensionTests.cs" company="Xamlr">
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
//   Defines the MarkupExtensionTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Test.Markup
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Xamlr.Core.Test;

    [TestClass]
    public class MarkupExtensionTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void Format_SingleUnnestedMarkupExtension_FormatsOnOneLine()
        {
            // Arrange
            var source = @"<Element1 Markup=""{Binding}"" />";
            var expected = @"<Element1 Markup=""{Binding}"" />";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_SingleUnnestedNonEmptyMarkupExtension_FormatsOnOneLine()
        {
            // Arrange
            var source = @"<Element1 Markup=""{Binding Path='Foo'}"" />";
            var expected = @"<Element1 Markup=""{Binding Path='Foo'}"" />";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_SingleNestedMarkupExtension_FormatsOnMultipleLines()
        {
            // Arrange
            var source = @"<Element1 Markup=""{Binding Source={StaticResource Items}}"" />";
            var expected =
@"<Element1
    Markup=""{Binding
        Source={StaticResource Items}}"" />";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_ComplexMarkupExtension1_Formats()
        {
            // Arrange
            var source = 
@"<Element1 
Markup=""{Binding 
Path=Foo, 
ElementName={Binding Path=DataContext.Girrafe, Converter={StaticResource GirrafeConverter}},
Source={StaticResource Items}}"" />";
            var expected =
@"<Element1
    Markup=""{Binding
        Path=Foo,
        ElementName={Binding
            Path=DataContext.Girrafe,
            Converter={StaticResource GirrafeConverter}},
        Source={StaticResource Items}}"" />";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_ComplexMarkupExtension2_Formats()
        {
            // Arrange
            var source =
@"<Element1 
Markup=""{Binding 
Path=Foo, 
ElementName={Binding Path=DataContext.Girrafe, StringFormat={} Moo {0} ABC {1}, Converter={StaticResource GirrafeConverter}},
Source={StaticResource Items}}"" />";
            var expected =
@"<Element1
    Markup=""{Binding
        Path=Foo,
        ElementName={Binding
            Path=DataContext.Girrafe,
            StringFormat={} Moo {0} ABC {1},
            Converter={StaticResource GirrafeConverter}},
        Source={StaticResource Items}}"" />";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }
    }
}