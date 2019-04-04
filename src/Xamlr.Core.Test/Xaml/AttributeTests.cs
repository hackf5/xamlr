// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeTests.cs" company="Xamlr">
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
//   Defines the AttributeTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Test.Xaml
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Xamlr.Core.Test;

    /// <summary>
    /// This class contain tests that validates the behaviour of the formatter with respect to attributes.
    /// </summary>
    [TestClass]
    public class AttributeTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void Format_SingleAttribute_FormatsOnOneLine()
        {
            // Arrange
            var source = @"<Element1 Attribute=""abc"" />";
            var expected = @"<Element1 Attribute=""abc"" />";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_SingleAttribute_SpacesInAttributesAreNotTrimmed()
        {
            // Arrange
            var source = @"<Element1 Attribute=""  abc  "" />";
            var expected = @"<Element1 Attribute=""  abc  "" />";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_TwoAttributes_FormatsOnTwoLines()
        {
            // Arrange
            var source = @"<Element1 Attribute1=""abc"" Attribute2=""def"" />";
            var expected =
@"<Element1
    Attribute1=""abc""
    Attribute2=""def"" />";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_ThreeAttributes_FormatsOnThreeLines()
        {
            // Arrange
            var source = @"<Element1 Attribute1=""abc"" Attribute2=""def"" Attribute3=""ghi"" />";
            var expected =
@"<Element1
    Attribute1=""abc""
    Attribute2=""def""
    Attribute3=""ghi"" />";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_SingleAttributeInNonRoot_FormatsOnOneLine()
        {
            // Arrange
            var source = @"<Element1><Element2 Attribute=""abc"" /></Element1>";
            var expected =
@"<Element1>
    <Element2 Attribute=""abc"" />
</Element1>";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_TwoAttributesInNonRoot_FormatsOnTwoLines()
        {
            // Arrange
            var source = @"<Element1><Element2 Attribute1=""abc""  Attribute2=""def"" /></Element1>";
            var expected =
@"<Element1>
    <Element2
        Attribute1=""abc""
        Attribute2=""def"" />
</Element1>";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_AttributesWithOrderInstruction_OrdersAsPerInstruction()
        {
            // Arrange
            var source = @"<!-- xamlr:order Attribute2, Attribute1 --><Element1 Attribute1=""abc"" Attribute2=""def"" />";
            var expected =
@"<!-- xamlr:order Attribute2, Attribute1 -->
<Element1
    Attribute2=""def""
    Attribute1=""abc"" />";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_AttributesWithOrderInstructionIncludingPredefined_OrdersAsPerInstruction()
        {
            // Arrange
            var source =
@"<!-- xamlr:order Z2, X1, Grid.Row, x:Name, Y3 -->
<Element1
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    x:Name=""n""
    Grid.Row=""g""
    A1=""a""
    Z2=""z""
    Y3=""y""
    X1=""x"" />";

            var expected =
@"<!-- xamlr:order Z2, X1, Grid.Row, x:Name, Y3 -->
<Element1
    Z2=""z""
    X1=""x""
    Grid.Row=""g""
    x:Name=""n""
    Y3=""y""
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    A1=""a"" />";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }
    }
}