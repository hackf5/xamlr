// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextTests.cs" company="Xamlr">
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
//   Defines the TextTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Test.Xaml
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Xamlr.Core.Test;

    /// <summary>
    /// This class contain tests that validates the behaviour of the formatter with respect to text.
    /// </summary>
    [TestClass]
    public class TextTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void Format_SingleLineText_TextPreserved()
        {
            // Arrange
            var source =
@"<Element1>


                Some text between two          element tags           



</Element1>";
            var expected =
@"<Element1>Some text between two          element tags</Element1>";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_MultilineText_TextPreserved()
        {
            // Arrange
            var source =
@"<Element1>


    Some text

                Some text


            Some    text

</Element1>";
            var expected =
@"<Element1>
    Some text
    Some text
    Some    text
</Element1>";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_MultilineTextNested_TextPreserved()
        {
            // Arrange
            var source =
@"<Element1>
    <Element2>
Some text

                Some text


            Some    text
    </Element2>
</Element1>";
            var expected =
@"<Element1>
    <Element2>
        Some text
        Some text
        Some    text
    </Element2>
</Element1>";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_SingleLineTextNested_TextPreserved()
        {
            // Arrange
            var source =
@"<Element1>
    <Element2>

Some text


    </Element2>
</Element1>";
            var expected =
@"<Element1>
    <Element2>Some text</Element2>
</Element1>";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_SingleLineTextWithInlineAtributeNested_TextPreserved()
        {
            // Arrange
            var source =
@"<Element1>
    <Element2 Attr=""Foo"">

Some text


    </Element2>
</Element1>";
            var expected =
@"<Element1>
    <Element2 Attr=""Foo"">Some text</Element2>
</Element1>";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_SingleLineTextWithInlineAtributeAndCommentNested_FormatsWell()
        {
            // Arrange
            var source =
@"<Element1>
    <Element2 Attr=""Foo"">

Some text<!-- Wierd Comment -->


    </Element2>
</Element1>";
            var expected =
@"<Element1>
    <Element2 Attr=""Foo"">
        Some text
        <!-- Wierd Comment -->

    </Element2>
</Element1>";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_SingleLineTextWithNonInlineAtributeNested_TextPreserved()
        {
            // Arrange
            var source =
@"<Element1>
    <Element2 Attr1=""Foo1"" Attr2=""Foo1"">

Some text


    </Element2>
</Element1>";
            var expected =
@"<Element1>
    <Element2
        Attr1=""Foo1""
        Attr2=""Foo1"">
        Some text
    </Element2>
</Element1>";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_Issue1334EscapedCharactersAreCorruptInElementBody_CharactersNoLongerCorrupt()
        {
            // Arrange
            var source =
@"<TextBlock>
    <Run>&lt;</Run>
</TextBlock>";
            var expected =
@"<TextBlock>
    <Run>&lt;</Run>
</TextBlock>";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }
    }
}