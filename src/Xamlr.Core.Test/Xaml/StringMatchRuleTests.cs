// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringMatchRuleTests.cs" company="Xamlr">
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
//   Defines the StringMatchRuleTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Test.Xaml
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Xamlr.Core.Test;
    using Xamlr.Core.Xaml;

    /// <summary>
    /// This class tests the <see cref="StringMatchRule"/> class.
    /// </summary>
    [TestClass]
    public class StringMatchRuleTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_AssignedToProperties()
        {
            // Arrange
            var pattern = "Name";
            var type = StringMatchType.Prefix;

            // Act
            var target = new StringMatchRule(pattern, type);

            // Assert
            Assert.AreEqual(pattern, target.Pattern);
            Assert.AreEqual(type, target.Type);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_NullPattern_AssignedAsEmptyString()
        {
            // Arrange
            var type = StringMatchType.Prefix;

            // Act
            var target = new StringMatchRule(null, type);

            // Assert
            Assert.AreEqual(string.Empty, target.Pattern);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CreateInstance_InvalidType_Throws()
        {
            // Arrange

            // Act
            new StringMatchRule(null, (StringMatchType)78231).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void IsMatch_ExactAndMatches_True()
        {
            // Arrange
            var target = new StringMatchRule("Name", StringMatchType.Exact);

            // Act
            var result = target.IsMatch("Name");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void IsMatch_PrefixAndMatches_True()
        {
            // Arrange
            var target = new StringMatchRule("Son", StringMatchType.Prefix);

            // Act
            var result = target.IsMatch("SonOfSam");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void IsMatch_SuffixAndMatches_True()
        {
            // Arrange
            var target = new StringMatchRule("Sam", StringMatchType.Suffix);

            // Act
            var result = target.IsMatch("SonOfSam");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void IsMatch_ExactAndNotMatches_False()
        {
            // Arrange
            var target = new StringMatchRule("Name", StringMatchType.Exact);

            // Act
            var result = target.IsMatch(" Name ");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void IsMatch_PrefixAndNotMatches_False()
        {
            // Arrange
            var target = new StringMatchRule("Name", StringMatchType.Prefix);

            // Act
            var result = target.IsMatch(" Name");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void IsMatch_SuffixAndNotMatches_False()
        {
            // Arrange
            var target = new StringMatchRule("Name", StringMatchType.Suffix);

            // Act
            var result = target.IsMatch("Name ");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Equals_ToSelf_Equal()
        {
            // Arrange
            var target = new StringMatchRule("Name", StringMatchType.Suffix);

            // Act
            var result = target.Equals(target);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Equals_ToIdentical_Equal()
        {
            // Arrange
            var target1 = new StringMatchRule("Name", StringMatchType.Suffix);
            var target2 = new StringMatchRule("Name", StringMatchType.Suffix);

            // Act
            var result = target1.Equals(target2);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Equals_ToDifferentPattern_Unequal()
        {
            // Arrange
            var target1 = new StringMatchRule("Name", StringMatchType.Suffix);
            var target2 = new StringMatchRule("Name1", StringMatchType.Suffix);

            // Act
            var result = target1.Equals(target2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Equals_ToDifferentType_Unequal()
        {
            // Arrange
            var target1 = new StringMatchRule("Name", StringMatchType.Suffix);
            var target2 = new StringMatchRule("Name", StringMatchType.Prefix);

            // Act
            var result = target1.Equals(target2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetHashCode_ToSelf_Equal()
        {
            // Arrange
            var target = new StringMatchRule("Name", StringMatchType.Suffix);

            // Act
            var hash1 = target.GetHashCode();
            var hash2 = target.GetHashCode();

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetHashCode_ToIdentical_Equal()
        {
            // Arrange
            var target1 = new StringMatchRule("Name", StringMatchType.Suffix);
            var target2 = new StringMatchRule("Name", StringMatchType.Suffix);

            // Act
            var hash1 = target1.GetHashCode();
            var hash2 = target2.GetHashCode();

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetHashCode_ToDifferentPattern_Unequal()
        {
            // Arrange
            var target1 = new StringMatchRule("Name", StringMatchType.Suffix);
            var target2 = new StringMatchRule("Name1", StringMatchType.Suffix);

            // Act
            var hash1 = target1.GetHashCode();
            var hash2 = target2.GetHashCode();

            // Assert
            Assert.AreNotEqual(hash1, hash2);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetHashCode_ToDifferentType_Unequal()
        {
            // Arrange
            var target1 = new StringMatchRule("Name", StringMatchType.Suffix);
            var target2 = new StringMatchRule("Name", StringMatchType.Prefix);

            // Act
            var hash1 = target1.GetHashCode();
            var hash2 = target2.GetHashCode();

            // Assert
            Assert.AreNotEqual(hash1, hash2);
        }
    }
}