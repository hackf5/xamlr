// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarkupExtensionTokenDetailsTests.cs" company="Xamlr">
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
//   Defines the MarkupExtensionTokenDetailsTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Test.Markup
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Xamlr.Core.Markup;

    [TestClass]
    public class MarkupExtensionTokenDetailsTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_OriginalValueParameter_IsAssignedToProperty()
        {
            // Arrange

            // Act
            var target = new MarkupExtensionTokenDetails("Original", false);

            // Assert
            Assert.AreEqual("Original", target.OriginalValue);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_IsQuotedParameter_IsAssignedToProperty()
        {
            // Arrange

            // Act
            var target = new MarkupExtensionTokenDetails(string.Empty, true);

            // Assert
            Assert.AreEqual(true, target.IsQuoted);
        }
    }
}