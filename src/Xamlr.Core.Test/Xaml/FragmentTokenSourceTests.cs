// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FragmentTokenSourceTests.cs" company="Xamlr">
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
//   Defines the FragmentTokenSourceTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable 168
namespace Xamlr.Core.Test.Xaml
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Xamlr.Core.Test;
    using Xamlr.Core.Xaml;

    /// <summary>
    /// This class tests the <see cref="FragmentTokenSource"/> class.
    /// </summary>
    [TestClass]
    public class FragmentTokenSourceTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateInstance_TokensNull_Throws()
        {
            // Arrange

            // Act
            new FragmentTokenSource(null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TokensEmpty_IsAtEnd()
        {
            // Arrange

            // Act
            var target = new FragmentTokenSource(new FragmentToken[0]);

            // Assert
            Assert.IsTrue(target.IsEnd);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TokensNotEmpty_IsNotAtEnd()
        {
            // Arrange

            // Act
            var target = new FragmentTokenSource(new[] { FragmentTokenSourceTests.CreateFragmentToken("E") });

            // Assert
            Assert.IsFalse(target.IsEnd);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void CreateInstance_TokensNotEmpty_TokenThrows()
        {
            // Arrange
            var tokens = new[] { FragmentTokenSourceTests.CreateFragmentToken("A"), FragmentTokenSourceTests.CreateFragmentToken("B") };
            var target = new FragmentTokenSource(tokens);

            // Act
            var t = target.Token;

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_LookAhead_ContainsAllTokens()
        {
            // Arrange
            var tokens = new[] { FragmentTokenSourceTests.CreateFragmentToken("A"), FragmentTokenSourceTests.CreateFragmentToken("B") };
            var target = new FragmentTokenSource(tokens);

            // Act
            var lookAhead = target.LookAhead.ToArray();

            // Assert
            Assert.IsTrue(tokens.SequenceEqual(lookAhead));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void MoveNext_FirstCall_TokenIsFirstToken()
        {
            // Arrange
            var tokens = new[] { FragmentTokenSourceTests.CreateFragmentToken("A"), FragmentTokenSourceTests.CreateFragmentToken("B") };
            var target = new FragmentTokenSource(tokens);

            // Act
            target.MoveNext();

            // Assert
            Assert.AreEqual(tokens[0], target.Token);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void MoveNext_FirstCall_IsEndIsFalse()
        {
            // Arrange
            var tokens = new[] { FragmentTokenSourceTests.CreateFragmentToken("A"), FragmentTokenSourceTests.CreateFragmentToken("B") };
            var target = new FragmentTokenSource(tokens);

            // Act
            target.MoveNext();

            // Assert
            Assert.IsFalse(target.IsEnd);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void MoveNext_FirstCall_LookAheadSkipsFirstToken()
        {
            // Arrange
            var tokens = new[] { FragmentTokenSourceTests.CreateFragmentToken("A"), FragmentTokenSourceTests.CreateFragmentToken("B") };
            var target = new FragmentTokenSource(tokens);

            // Act
            target.MoveNext();
            var lookAhead = target.LookAhead.ToArray();

            // Assert
            Assert.IsTrue(tokens.Skip(1).SequenceEqual(lookAhead));
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void FragmentTokenSource_IntegrationTest_BehavesAsExpected()
        {
            // Arrange
            var tokens = new[]
            {
                FragmentTokenSourceTests.CreateFragmentToken("A"), 
                FragmentTokenSourceTests.CreateFragmentToken("B"),
                FragmentTokenSourceTests.CreateFragmentToken("C"),
                FragmentTokenSourceTests.CreateFragmentToken("D")
            };
            var target = new FragmentTokenSource(tokens);

            // Integration
            Assert.IsFalse(target.IsEnd);
            Assert.AreEqual(tokens.Length, target.LookAhead.Count());

            Assert.IsTrue(target.MoveNext());
            Assert.IsFalse(target.IsEnd);
            Assert.AreEqual(tokens[0], target.Token);
            Assert.AreEqual(tokens.Length - 1, target.LookAhead.Count());

            Assert.IsTrue(target.MoveNext());
            Assert.IsFalse(target.IsEnd);
            Assert.AreEqual(tokens[1], target.Token);
            Assert.AreEqual(tokens.Length - 2, target.LookAhead.Count());

            Assert.IsTrue(target.MoveNext());
            Assert.IsFalse(target.IsEnd);
            Assert.AreEqual(tokens[2], target.Token);
            Assert.AreEqual(tokens.Length - 3, target.LookAhead.Count());

            Assert.IsTrue(target.MoveNext());
            Assert.IsFalse(target.IsEnd);
            Assert.AreEqual(tokens[3], target.Token);
            Assert.AreEqual(tokens.Length - 4, target.LookAhead.Count());

            Assert.IsFalse(target.MoveNext());
            Assert.IsTrue(target.IsEnd);
        }

        private static FragmentToken CreateFragmentToken(string name)
        {
            return new FragmentToken(FragmentType.Element, name, null);
        }
    }
}