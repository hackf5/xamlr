// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormatterFactoryTests.cs" company="Xamlr">
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
//   Defines the FormatterFactoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute

#pragma warning disable 168
namespace Xamlr.Core.Test.Xaml
{
    using System;
    using System.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Xamlr.Core.Xaml;

    /// <summary>
    /// This class tests the <see cref="FormatterFactory"/> class.
    /// </summary>
    [TestClass]
    public class FormatterFactoryTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void Create_AllFragmentTypes_FormatterCreated()
        {
            // Arrange
            var target = new FormatterFactory(new FormattingContext(new XamlFormatterOptions()));

            foreach (FragmentType type in Enum.GetValues(typeof(FragmentType)))
            {
                try
                {
                    // Act
                    var formatter = target.Create(type);

                    // Assert
                    Assert.IsNotNull(formatter);
                }
                catch (Exception)
                {
                    Trace.WriteLine($"Failed to create formatter for type '{type}'");
                    throw;
                }
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Create_CreatedFormatters_AreNotReused()
        {
            // Arrange
            var target = new FormatterFactory(new FormattingContext(new XamlFormatterOptions()));

            foreach (FragmentType type in Enum.GetValues(typeof(FragmentType)))
            {
                try
                {
                    // Act
                    var formatter1 = target.Create(type);
                    var formatter2 = target.Create(type);

                    // Assert
                    Assert.AreNotSame(formatter1, formatter2);
                }
                catch (Exception)
                {
                    Trace.WriteLine($"Failed to create formatter for type '{type}'");
                    throw;
                }
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateInstance_ContextNull_Throws()
        {
            // Arrange

            // Act
            new FormatterFactory(null).Naught();

            // Assert
        }
    }
}