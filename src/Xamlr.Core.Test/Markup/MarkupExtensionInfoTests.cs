// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarkupExtensionInfoTests.cs" company="Xamlr">
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
//   Defines the MarkupExtensionInfoTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ObjectCreationAsStatement
namespace Xamlr.Core.Test.Markup
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Xamlr.Core.Markup;

    /// <summary>
    /// This class tests the <see cref="MarkupExtensionInfo"/> class.
    /// </summary>
    [TestClass]
    public class MarkupExtensionInfoTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_Parameters_AssignedToProperties()
        {
            // Arrange
            var typeName = "TypeName";
            var arguments = new IMarkupExtensionArgumentInfo[0];

            // Act
            var target = new MarkupExtensionInfo(typeName, arguments);

            // Assert
            Assert.AreEqual(typeName, target.TypeName);
            Assert.IsTrue(arguments.SequenceEqual(target.Arguments));
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateInstance_TypeNameParameterNull_Throws()
        {
            // Arrange

            // Act
            new MarkupExtensionInfo(null, new IMarkupExtensionArgumentInfo[0]);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateInstance_ArgumentsParameterNull_Throws()
        {
            // Arrange

            // Act
            new MarkupExtensionInfo("TypeName", null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ArgumentsParameter_CopiesArguments()
        {
            // Arrange
            var arguments = new[]
            {
                MarkupExtensionInfoTests.CreateArgument(null, "S1"),
                MarkupExtensionInfoTests.CreateArgument(null, "S2"),
                MarkupExtensionInfoTests.CreateArgument("M1", "S3"),
                MarkupExtensionInfoTests.CreateArgument("M2", "S4")
            }.ToList();

            // Act
            var target = new MarkupExtensionInfo("TypeName", arguments);

            // Assert
            Assert.AreNotEqual(arguments, target.Arguments);
            Assert.IsTrue(arguments.SequenceEqual(target.Arguments));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Equals_ToSelf_Equal()
        {
            // Arrange
            var @this = new MarkupExtensionInfo("TypeName", new IMarkupExtensionArgumentInfo[0]);

            // Act
// ReSharper disable EqualExpressionComparison
            var equal = @this.Equals(@this);
// ReSharper restore EqualExpressionComparison

            // Assert
            Assert.IsTrue(equal);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetHashCode_ToSelf_Equal()
        {
            // Arrange
            var @this = new MarkupExtensionInfo("TypeName", new IMarkupExtensionArgumentInfo[0]);

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
            var @this = new MarkupExtensionInfo("TypeName", new IMarkupExtensionArgumentInfo[0]);
            var other = new MarkupExtensionInfo("TypeName", new IMarkupExtensionArgumentInfo[0]);

            // Act
            var equal = @this.Equals(other);

            // Assert
            Assert.IsTrue(equal);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetHashCode_ToIdentical_Equal()
        {
            // Arrange
            var @this = new MarkupExtensionInfo("TypeName", new IMarkupExtensionArgumentInfo[0]);
            var other = new MarkupExtensionInfo("TypeName", new IMarkupExtensionArgumentInfo[0]);

            // Act
            var hash1 = @this.GetHashCode();
            var hash2 = other.GetHashCode();

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Equals_ToIdenticalWithArguments_Equal()
        {
            // Arrange
            var arguments1 = new[]
            {
                MarkupExtensionInfoTests.CreateArgument(null, "S1"), 
                MarkupExtensionInfoTests.CreateArgument(null, "S2"), 
                MarkupExtensionInfoTests.CreateArgument("M1", "S3"),
                MarkupExtensionInfoTests.CreateArgument("M2", "S4")
            };

            var arguments2 = new[]
            {
                MarkupExtensionInfoTests.CreateArgument(null, "S1"), 
                MarkupExtensionInfoTests.CreateArgument(null, "S2"), 
                MarkupExtensionInfoTests.CreateArgument("M1", "S3"),
                MarkupExtensionInfoTests.CreateArgument("M2", "S4")
            };

            var @this = new MarkupExtensionInfo("TypeName", arguments1);
            var other = new MarkupExtensionInfo("TypeName", arguments2);

            // Act
            var equal = @this.Equals(other);

            // Assert
            Assert.IsTrue(equal);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetHashCode_ToIdenticalWithArguments_Equal()
        {
            // Arrange
            var arguments1 = new[]
            {
                MarkupExtensionInfoTests.CreateArgument(null, "S1"), 
                MarkupExtensionInfoTests.CreateArgument(null, "S2"), 
                MarkupExtensionInfoTests.CreateArgument("M1", "S3"),
                MarkupExtensionInfoTests.CreateArgument("M2", "S4")
            };

            var arguments2 = new[]
            {
                MarkupExtensionInfoTests.CreateArgument(null, "S1"), 
                MarkupExtensionInfoTests.CreateArgument(null, "S2"), 
                MarkupExtensionInfoTests.CreateArgument("M1", "S3"),
                MarkupExtensionInfoTests.CreateArgument("M2", "S4")
            };

            var @this = new MarkupExtensionInfo("TypeName",  arguments1);
            var other = new MarkupExtensionInfo("TypeName", arguments2);

            // Act
            var hash1 = @this.GetHashCode();
            var hash2 = other.GetHashCode();

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Equals_DifferentTypeName_Unequal()
        {
            // Arrange
            var @this = new MarkupExtensionInfo("TypeName1", new IMarkupExtensionArgumentInfo[0]);
            var other = new MarkupExtensionInfo("TypeName2", new IMarkupExtensionArgumentInfo[0]);

            // Act
            var equal = @this.Equals(other);

            // Assert
            Assert.IsFalse(equal);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetHashCode_DifferentTypeName_Unequal()
        {
            // Arrange
            var @this = new MarkupExtensionInfo("TypeName1", new IMarkupExtensionArgumentInfo[0]);
            var other = new MarkupExtensionInfo("TypeName2", new IMarkupExtensionArgumentInfo[0]);

            // Act
            var hash1 = @this.GetHashCode();
            var hash2 = other.GetHashCode();

            // Assert
            Assert.AreNotEqual(hash1, hash2);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Equals_DifferentArguments_Unequal()
        {
            // Arrange
            var arguments1 = new[]
            {
                MarkupExtensionInfoTests.CreateArgument(null, "S1"), 
                MarkupExtensionInfoTests.CreateArgument(null, "S2"), 
                MarkupExtensionInfoTests.CreateArgument("M1", "S3"),
                MarkupExtensionInfoTests.CreateArgument("M2", "S4")
            };

            var arguments2 = new[]
            {
                MarkupExtensionInfoTests.CreateArgument(null, "S1"), 
                MarkupExtensionInfoTests.CreateArgument(null, "S2"), 
                MarkupExtensionInfoTests.CreateArgument("M3", "S3"),
                MarkupExtensionInfoTests.CreateArgument("M2", "S4")
            };

            var @this = new MarkupExtensionInfo("TypeName", arguments1);
            var other = new MarkupExtensionInfo("TypeName", arguments2);

            // Act
            var equal = @this.Equals(other);

            // Assert
            Assert.IsFalse(equal);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetHashCode_DifferentArguments_Unequal()
        {
            // Arrange
            var arguments1 = new[]
            {
                MarkupExtensionInfoTests.CreateArgument(null, "S1"), 
                MarkupExtensionInfoTests.CreateArgument(null, "S2"), 
                MarkupExtensionInfoTests.CreateArgument("M1", "S3"),
                MarkupExtensionInfoTests.CreateArgument("M2", "S4")
            };

            var arguments2 = new[]
            {
                MarkupExtensionInfoTests.CreateArgument(null, "S1"), 
                MarkupExtensionInfoTests.CreateArgument(null, "S2"), 
                MarkupExtensionInfoTests.CreateArgument("M3", "S3"),
                MarkupExtensionInfoTests.CreateArgument("M2", "S4")
            };

            var @this = new MarkupExtensionInfo("TypeName", arguments1);
            var other = new MarkupExtensionInfo("TypeName", arguments2);

            // Act
            var hash1 = @this.GetHashCode();
            var hash2 = other.GetHashCode();

            // Assert
            Assert.AreNotEqual(hash1, hash2);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Equals_NestedEqualMarkupExtension_Equal()
        {
            // Arrange
            var arguments1 = new[]
            {
                MarkupExtensionInfoTests.CreateArgument(null, "S1"), 
                MarkupExtensionInfoTests.CreateArgument(null, new MarkupExtensionInfo("TypeName", new[] { MarkupExtensionInfoTests.CreateArgument("M", "S") })), 
                MarkupExtensionInfoTests.CreateArgument("M1", "S3"),
                MarkupExtensionInfoTests.CreateArgument("M2", "S4")
            };

            var arguments2 = new[]
            {
                MarkupExtensionInfoTests.CreateArgument(null, "S1"), 
                MarkupExtensionInfoTests.CreateArgument(null, new MarkupExtensionInfo("TypeName", new[] { MarkupExtensionInfoTests.CreateArgument("M", "S") })), 
                MarkupExtensionInfoTests.CreateArgument("M1", "S3"),
                MarkupExtensionInfoTests.CreateArgument("M2", "S4")
            };

            var @this = new MarkupExtensionInfo("TypeName", arguments1);
            var other = new MarkupExtensionInfo("TypeName", arguments2);

            // Act
            var equal = @this.Equals(other);

            // Assert
            Assert.IsTrue(equal);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetHashCode_NestedEqualMarkupExtension_Equal()
        {
            // Arrange
            var arguments1 = new[]
            {
                MarkupExtensionInfoTests.CreateArgument(null, "S1"), 
                MarkupExtensionInfoTests.CreateArgument(null, new MarkupExtensionInfo("TypeName", new[] { MarkupExtensionInfoTests.CreateArgument("M", "S") })), 
                MarkupExtensionInfoTests.CreateArgument("M1", "S3"),
                MarkupExtensionInfoTests.CreateArgument("M2", "S4")
            };

            var arguments2 = new[]
            {
                MarkupExtensionInfoTests.CreateArgument(null, "S1"), 
                MarkupExtensionInfoTests.CreateArgument(null, new MarkupExtensionInfo("TypeName", new[] { MarkupExtensionInfoTests.CreateArgument("M", "S") })), 
                MarkupExtensionInfoTests.CreateArgument("M1", "S3"),
                MarkupExtensionInfoTests.CreateArgument("M2", "S4")
            };

            var @this = new MarkupExtensionInfo("TypeName", arguments1);
            var other = new MarkupExtensionInfo("TypeName", arguments2);

            // Act
            var hash1 = @this.GetHashCode();
            var hash2 = other.GetHashCode();

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Equals_NestedUnequalMarkupExtension_Unequal()
        {
            // Arrange
            var arguments1 = new[]
            {
                MarkupExtensionInfoTests.CreateArgument(null, "S1"), 
                MarkupExtensionInfoTests.CreateArgument(null, new MarkupExtensionInfo("TypeName", new[] { MarkupExtensionInfoTests.CreateArgument("M", "S") })), 
                MarkupExtensionInfoTests.CreateArgument("M1", "S3"),
                MarkupExtensionInfoTests.CreateArgument("M2", "S4")
            };

            var arguments2 = new[]
            {
                MarkupExtensionInfoTests.CreateArgument(null, "S1"), 
                MarkupExtensionInfoTests.CreateArgument(null, new MarkupExtensionInfo("TypeName", new[] { MarkupExtensionInfoTests.CreateArgument("N", "S") })), 
                MarkupExtensionInfoTests.CreateArgument("M1", "S3"),
                MarkupExtensionInfoTests.CreateArgument("M2", "S4")
            };

            var @this = new MarkupExtensionInfo("TypeName", arguments1);
            var other = new MarkupExtensionInfo("TypeName", arguments2);

            // Act
            var equal = @this.Equals(other);

            // Assert
            Assert.IsFalse(equal);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetHashCode_NestedUnequalMarkupExtension_Unequal()
        {
            // Arrange
            var arguments1 = new[]
            {
                MarkupExtensionInfoTests.CreateArgument(null, "S1"), 
                MarkupExtensionInfoTests.CreateArgument(null, new MarkupExtensionInfo("TypeName", new[] { MarkupExtensionInfoTests.CreateArgument("M", "S") })), 
                MarkupExtensionInfoTests.CreateArgument("M1", "S3"),
                MarkupExtensionInfoTests.CreateArgument("M2", "S4")
            };

            var arguments2 = new[]
            {
                MarkupExtensionInfoTests.CreateArgument(null, "S1"), 
                MarkupExtensionInfoTests.CreateArgument(null, new MarkupExtensionInfo("TypeName", new[] { MarkupExtensionInfoTests.CreateArgument("N", "S") })), 
                MarkupExtensionInfoTests.CreateArgument("M1", "S3"),
                MarkupExtensionInfoTests.CreateArgument("M2", "S4")
            };

            var @this = new MarkupExtensionInfo("TypeName", arguments1);
            var other = new MarkupExtensionInfo("TypeName", arguments2);

            // Act
            var hash1 = @this.GetHashCode();
            var hash2 = other.GetHashCode();

            // Assert
            Assert.AreNotEqual(hash1, hash2);
        }

        private static IMarkupExtensionArgumentInfo CreateArgument(string memberName, object @string, bool isQuoted = false)
        {
            return new MarkupExtensionArgumentInfo(
                memberName,
                memberName == null ? null : new MarkupExtensionTokenDetails(memberName, false), 
                @string,
                new MarkupExtensionTokenDetails(@string.ToString(), isQuoted));
        }
    }
}