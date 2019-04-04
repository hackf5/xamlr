// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarkupExtensionArgumentInfoTests.cs" company="Xamlr">
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
//   Defines the MarkupExtensionArgumentInfoTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ObjectCreationAsStatement
namespace Xamlr.Core.Test.Markup
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Xamlr.Core.Markup;

    /// <summary>
    /// This class tests the <see cref="MarkupExtensionArgumentInfo"/> class.
    /// </summary>
    [TestClass]
    public class MarkupExtensionArgumentInfoTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateInstance_StringNull_Throws()
        {
            // Arrange
            var memberNameDetails = new MarkupExtensionTokenDetails("A", false);
            var stringDetails = new MarkupExtensionTokenDetails("A", false);

            // Act
            new MarkupExtensionArgumentInfo("Member", memberNameDetails, null, stringDetails);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateInstance_StringDetailsNull_Throws()
        {
            // Arrange
            var memberNameDetails = new MarkupExtensionTokenDetails("A", false);

            // Act
            new MarkupExtensionArgumentInfo("Member", memberNameDetails, "String", null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_Parameters_AreAssignedToProperties()
        {
            // Arrange
            var memberNameDetails = new MarkupExtensionTokenDetails("A", false);
            var stringDetails = new MarkupExtensionTokenDetails("B", false);

            // Act
            var target = new MarkupExtensionArgumentInfo("Member", memberNameDetails, "String", stringDetails);

            // Assert
            Assert.AreEqual("Member", target.MemberNameValue);
            Assert.AreEqual(memberNameDetails, target.MemberNameDetails);
            Assert.AreEqual("String", target.StringValue);
            Assert.AreEqual(stringDetails, target.StringDetails);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_MemberNameParameter_CanBeNull()
        {
            // Arrange
            var memberNameDetails = new MarkupExtensionTokenDetails("A", false);
            var stringDetails = new MarkupExtensionTokenDetails("B", false);

            // Act
            var target = new MarkupExtensionArgumentInfo(null, memberNameDetails, "String", stringDetails);

            // Assert
            Assert.IsNull(target.MemberNameValue);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_MemberNameDetailsParameter_CanBeNull()
        {
            // Arrange
            var stringDetails = new MarkupExtensionTokenDetails("B", false);

            // Act
            var target = new MarkupExtensionArgumentInfo("Member", null, "String", stringDetails);

            // Assert
            Assert.IsNull(target.MemberNameDetails);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_MemberNameParameterNull_Positional()
        {
            // Arrange
            var stringDetails = new MarkupExtensionTokenDetails("B", false);

            // Act
            var target = new MarkupExtensionArgumentInfo(null, null, "String", stringDetails);

            // Assert
            Assert.AreEqual(MarkupExtensionArgumentType.Positional, target.ArgumentType);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_MemberNameParameterNotNull_Named()
        {
            // Arrange
            var memberNameDetails = new MarkupExtensionTokenDetails("A", false);
            var stringDetails = new MarkupExtensionTokenDetails("B", false);

            // Act
            var target = new MarkupExtensionArgumentInfo("Member", memberNameDetails, "String", stringDetails);

            // Assert
            Assert.AreEqual(MarkupExtensionArgumentType.Named, target.ArgumentType);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_IsEmpty_WhenEmpty()
        {
            // Arrange
            var stringDetails = new MarkupExtensionTokenDetails(string.Empty, false);

            // Act
            var target = new MarkupExtensionArgumentInfo(null, null, string.Empty, stringDetails);

            // Assert
            Assert.IsTrue(target.IsEmpty);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_IsNotEmpty_WhenQuoted()
        {
            // Arrange
            var stringDetails = new MarkupExtensionTokenDetails(string.Empty, true);

            // Act
            var target = new MarkupExtensionArgumentInfo(null, null, string.Empty, stringDetails);

            // Assert
            Assert.IsFalse(target.IsEmpty);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_IsNotEmpty_WhenStringNotEmpty()
        {
            // Arrange
            var stringDetails = new MarkupExtensionTokenDetails(".", true);

            // Act
            var target = new MarkupExtensionArgumentInfo(null, null, ".", stringDetails);

            // Assert
            Assert.IsFalse(target.IsEmpty);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_IsNotEmpty_WhenNamed()
        {
            // Arrange
            var memberNameDetails = new MarkupExtensionTokenDetails(".", true);
            var stringDetails = new MarkupExtensionTokenDetails(string.Empty, true);

            // Act
            var target = new MarkupExtensionArgumentInfo("Member", memberNameDetails, string.Empty, stringDetails);

            // Assert
            Assert.IsFalse(target.IsEmpty);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_IsNotEmpty_Nested()
        {
            // Arrange
            var stringDetails = new MarkupExtensionTokenDetails("{A}", true);

            // Act
            var target = new MarkupExtensionArgumentInfo(
                null, null, new MarkupExtensionInfo("A", new IMarkupExtensionArgumentInfo[0]), stringDetails);

            // Assert
            Assert.IsFalse(target.IsEmpty);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_IsNotNested_WhenStringIsString()
        {
            // Arrange
            var stringDetails = new MarkupExtensionTokenDetails(string.Empty, true);

            // Act
            var target = new MarkupExtensionArgumentInfo(null, null, string.Empty, stringDetails);

            // Assert
            Assert.IsFalse(target.IsNested);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_IsNotNested_WhenStringIsMarkupExtension()
        {
            // Arrange
            var stringDetails = new MarkupExtensionTokenDetails("{A}", true);

            // Act
            var target = new MarkupExtensionArgumentInfo(
                null, null, new MarkupExtensionInfo("A", new IMarkupExtensionArgumentInfo[0]), stringDetails);

            // Assert
            Assert.IsTrue(target.IsNested);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Equals_ToSame_Equal()
        {
            // Arrange
            var stringDetails = new MarkupExtensionTokenDetails(string.Empty, true);
            var @this = new MarkupExtensionArgumentInfo(null, null, string.Empty, stringDetails);

            // Act
// ReSharper disable EqualExpressionComparison
            var equal = @this.Equals(@this);
// ReSharper restore EqualExpressionComparison

            // Assert
            Assert.IsTrue(equal);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetHashCode_ToSame_Equal()
        {
            // Arrange
            var stringDetails = new MarkupExtensionTokenDetails(string.Empty, true);
            var @this = new MarkupExtensionArgumentInfo(null, null, string.Empty, stringDetails);

            // Act
            var hash1 = @this.GetHashCode();
            var hash2 = @this.GetHashCode();

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Equals_ToIdentical_Equal()
        {
            // Arrange
            var stringDetails = new MarkupExtensionTokenDetails(string.Empty, true);
            var @this = new MarkupExtensionArgumentInfo(null, null, string.Empty, stringDetails);
            var other = new MarkupExtensionArgumentInfo(null, null, string.Empty, stringDetails);

            // Act
            // ReSharper disable EqualExpressionComparison
            var equal = @this.Equals(other);
            // ReSharper restore EqualExpressionComparison

            // Assert
            Assert.IsTrue(equal);
        }
        
        [TestMethod]
        [TestCategory("Unit")]
        public void GetHashCode_ToIdentical_Equal()
        {
            // Arrange
            var stringDetails = new MarkupExtensionTokenDetails(string.Empty, true);
            var @this = new MarkupExtensionArgumentInfo(null, null, string.Empty, stringDetails);
            var other = new MarkupExtensionArgumentInfo(null, null, string.Empty, stringDetails);

            // Act
            var hash1 = @this.GetHashCode();
            var hash2 = other.GetHashCode();

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Equals_ToIdenticalDifferentStringDetails_Equal()
        {
            // Arrange
            var stringDetails1 = new MarkupExtensionTokenDetails(string.Empty, true);
            var stringDetails2 = new MarkupExtensionTokenDetails(string.Empty, true);
            var @this = new MarkupExtensionArgumentInfo(null, null, string.Empty, stringDetails1);
            var other = new MarkupExtensionArgumentInfo(null, null, string.Empty, stringDetails2);

            // Act
            // ReSharper disable EqualExpressionComparison
            var equal = @this.Equals(other);
            // ReSharper restore EqualExpressionComparison

            // Assert
            Assert.IsTrue(equal);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetHashCode_ToIdenticalDifferentStringDetails_Equal()
        {
            // Arrange
            var stringDetails1 = new MarkupExtensionTokenDetails(string.Empty, true);
            var stringDetails2 = new MarkupExtensionTokenDetails(string.Empty, true);
            var @this = new MarkupExtensionArgumentInfo(null, null, string.Empty, stringDetails1);
            var other = new MarkupExtensionArgumentInfo(null, null, string.Empty, stringDetails2);

            // Act
            var hash1 = @this.GetHashCode();
            var hash2 = other.GetHashCode();

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Equals_ToIdenticalWithMemberName_Equal()
        {
            // Arrange
            var stringDetails1 = new MarkupExtensionTokenDetails("String", false);
            var stringDetails2 = new MarkupExtensionTokenDetails("String", false);
            var memberNameDetails1 = new MarkupExtensionTokenDetails("Member", false);
            var memberNameDetails2 = new MarkupExtensionTokenDetails("Member", false);
            var @this = new MarkupExtensionArgumentInfo("Member", memberNameDetails1, "String", stringDetails1);
            var other = new MarkupExtensionArgumentInfo("Member", memberNameDetails2, "String", stringDetails2);

            // Act
            // ReSharper disable EqualExpressionComparison
            var equal = @this.Equals(other);
            // ReSharper restore EqualExpressionComparison

            // Assert
            Assert.IsTrue(equal);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetHashCode_ToIdenticalWithMemberName_Equal()
        {
            // Arrange
            var stringDetails1 = new MarkupExtensionTokenDetails("String", false);
            var stringDetails2 = new MarkupExtensionTokenDetails("String", false);
            var memberNameDetails1 = new MarkupExtensionTokenDetails("Member", false);
            var memberNameDetails2 = new MarkupExtensionTokenDetails("Member", false);
            var @this = new MarkupExtensionArgumentInfo("Member", memberNameDetails1, "String", stringDetails1);
            var other = new MarkupExtensionArgumentInfo("Member", memberNameDetails2, "String", stringDetails2);

            // Act
            var hash1 = @this.GetHashCode();
            var hash2 = other.GetHashCode();

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Equals_DifferentStringName_Unequal()
        {
            // Arrange
            var stringDetails1 = new MarkupExtensionTokenDetails("String1", false);
            var stringDetails2 = new MarkupExtensionTokenDetails("String2", false);
            var memberNameDetails1 = new MarkupExtensionTokenDetails("Member", false);
            var memberNameDetails2 = new MarkupExtensionTokenDetails("Member", false);
            var @this = new MarkupExtensionArgumentInfo("Member", memberNameDetails1, "String1", stringDetails1);
            var other = new MarkupExtensionArgumentInfo("Member", memberNameDetails2, "String2", stringDetails2);

            // Act
            // ReSharper disable EqualExpressionComparison
            var equal = @this.Equals(other);
            // ReSharper restore EqualExpressionComparison

            // Assert
            Assert.IsFalse(equal);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetHashCode_DifferentStringName_Unequal()
        {
            // Arrange
            var stringDetails1 = new MarkupExtensionTokenDetails("String1", false);
            var stringDetails2 = new MarkupExtensionTokenDetails("String2", false);
            var memberNameDetails1 = new MarkupExtensionTokenDetails("Member", false);
            var memberNameDetails2 = new MarkupExtensionTokenDetails("Member", false);
            var @this = new MarkupExtensionArgumentInfo("Member", memberNameDetails1, "String1", stringDetails1);
            var other = new MarkupExtensionArgumentInfo("Member", memberNameDetails2, "String2", stringDetails2);

            // Act
            var hash1 = @this.GetHashCode();
            var hash2 = other.GetHashCode();

            // Assert
            Assert.AreNotEqual(hash1, hash2);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Equals_DifferentMemberName_Unequal()
        {
            // Arrange
            var stringDetails1 = new MarkupExtensionTokenDetails("String", false);
            var stringDetails2 = new MarkupExtensionTokenDetails("String", false);
            var memberNameDetails1 = new MarkupExtensionTokenDetails("Member1", false);
            var memberNameDetails2 = new MarkupExtensionTokenDetails("Member2", false);
            var @this = new MarkupExtensionArgumentInfo("Member1", memberNameDetails1, "String", stringDetails1);
            var other = new MarkupExtensionArgumentInfo("Member2", memberNameDetails2, "String", stringDetails2);

            // Act
            // ReSharper disable EqualExpressionComparison
            var equal = @this.Equals(other);
            // ReSharper restore EqualExpressionComparison

            // Assert
            Assert.IsFalse(equal);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetHashCode_DifferentMemberName_Unequal()
        {
            // Arrange
            var stringDetails1 = new MarkupExtensionTokenDetails("String", false);
            var stringDetails2 = new MarkupExtensionTokenDetails("String", false);
            var memberNameDetails1 = new MarkupExtensionTokenDetails("Member1", false);
            var memberNameDetails2 = new MarkupExtensionTokenDetails("Member2", false);
            var @this = new MarkupExtensionArgumentInfo("Member1", memberNameDetails1, "String", stringDetails1);
            var other = new MarkupExtensionArgumentInfo("Member2", memberNameDetails2, "String", stringDetails2);

            // Act
            var hash1 = @this.GetHashCode();
            var hash2 = other.GetHashCode();

            // Assert
            Assert.AreNotEqual(hash1, hash2);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Equals_DifferentQuotedString_Unequal()
        {
            // Arrange
            var stringDetails1 = new MarkupExtensionTokenDetails("String", true);
            var stringDetails2 = new MarkupExtensionTokenDetails("String", false);
            var memberNameDetails1 = new MarkupExtensionTokenDetails("Member", false);
            var memberNameDetails2 = new MarkupExtensionTokenDetails("Member", false);
            var @this = new MarkupExtensionArgumentInfo("Member", memberNameDetails1, "String", stringDetails1);
            var other = new MarkupExtensionArgumentInfo("Member", memberNameDetails2, "String", stringDetails2);

            // Act
            // ReSharper disable EqualExpressionComparison
            var equal = @this.Equals(other);
            // ReSharper restore EqualExpressionComparison

            // Assert
            Assert.IsFalse(equal);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetHashCode_DifferentQuotedString_Unequal()
        {
            // Arrange
            var stringDetails1 = new MarkupExtensionTokenDetails("String", true);
            var stringDetails2 = new MarkupExtensionTokenDetails("String", false);
            var memberNameDetails1 = new MarkupExtensionTokenDetails("Member", false);
            var memberNameDetails2 = new MarkupExtensionTokenDetails("Member", false);
            var @this = new MarkupExtensionArgumentInfo("Member", memberNameDetails1, "String", stringDetails1);
            var other = new MarkupExtensionArgumentInfo("Member", memberNameDetails2, "String", stringDetails2);

            // Act
            var hash1 = @this.GetHashCode();
            var hash2 = other.GetHashCode();

            // Assert
            Assert.AreNotEqual(hash1, hash2);
        }
    }
}