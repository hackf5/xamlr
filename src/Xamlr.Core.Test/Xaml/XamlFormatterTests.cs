// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XamlFormatterTests.cs" company="Xamlr">
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
//   Defines the XamlFormatterTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Test.Xaml
{
    using System.ComponentModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Xamlr.Core.Test;

    /// <summary>
    /// This class contain tests that validates the behaviour of the formatter with respect to XAML documents as a 
    /// whole.
    /// </summary>
    /// <remarks>
    /// All though all of the tests are essentially integration tests, they are focussed on individual characteristics
    /// of a XAML document and in this sense are really unit tests that are decoupled from the implementation. These
    /// tests examine the behaviour of the formatter on real world large documents.
    /// </remarks>
    [TestClass]
    public class XamlFormatterTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void Format_IntegrationTest1_FormatsAsExpected()
        {
            // Arrange
            var source = "IntegrationTest1.xaml".GetSource();
            var expected = "IntegrationTest1_Formatted.xaml".GetSource();

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_IntegrationTest1RoundTrip_OutputIsStable()
        {
            // Arrange
            var source = "IntegrationTest1.xaml".GetSource();
            var expected = "IntegrationTest1_Formatted.xaml".GetSource();

            // Act
            var actual = source.Format();
            actual = actual.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        [ExpectedException(typeof(WarningException))]
        public void Format_IgnoredDocument_Throws()
        {
            // Arrange
            var source =
@"<!-- xamlr:ignore -->
<Page>
</Page>";

            // Act
            source.Format();

            // Assert
        }

        [TestMethod]
        [TestCategory("Integration")]
        [ExpectedException(typeof(WarningException))]
        public void Format_IgnoredDocumentInCopyright_Throws()
        {
            // Arrange
            var source =
@"    <!-- 
        My Funky Copyright 
        
        xamlr:ignore
    -->

<Page>
</Page>";

            // Act
            source.Format();

            // Assert
        }

        [TestMethod]
        [TestCategory("Integration")]
        [ExpectedException(typeof(WarningException))]
        public void Format_IgnoredDocumentAfterCopyright_Throws()
        {
            // Arrange
            var source =
@"    <!-- 
        My Funky Copyright 
      -->
        <!-- xamlr:ignore -->

<Page>
</Page>";

            // Act
            source.Format();

            // Assert
        }
    }
}