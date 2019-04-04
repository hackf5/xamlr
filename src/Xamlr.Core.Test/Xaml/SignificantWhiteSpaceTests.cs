// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignificantWhiteSpaceTests.cs" company="Xamlr">
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
//   Defines the SignificantWhiteSpaceTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Test.Xaml
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Xamlr.Core.Test;

    /// <summary>
    /// This class contain tests that validates the behaviour of the formatter with respect to significant white space.
    /// </summary>
    [TestClass]
    public class SignificantWhiteSpaceTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void Format_MultiLine_SpacePreserved()
        {
            // Arrange
            var source =
@"<Element1 xml:space=""preserve"">       



</Element1>";
            var expected =
@"<Element1 xml:space=""preserve"">       



</Element1>";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_Text_SpacePreserved()
        {
            // Arrange
            var source =
@"<Element1 xml:space=""preserve"">       

Moo foo
        Mba 
            SDFSD slb 
 sdlf
   

</Element1>";
            var expected =
@"<Element1 xml:space=""preserve"">       

Moo foo
        Mba 
            SDFSD slb 
 sdlf
   

</Element1>";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_SimpleNested_SpacePreserved()
        {
            // Arrange
            var source =
@"<Element1 xml:space=""preserve"">       


          <Element2 A1=""Value1"" A2=""Value2"" />            


</Element1>";
            var expected =
@"<Element1 xml:space=""preserve"">       


          <Element2
        A1=""Value1""
        A2=""Value2"" />            


</Element1>";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_NestedMultiLine_SpacePreserved()
        {
            // Arrange
            var source =
@"<Element1 xml:space=""preserve"">       

                <Element2>   Foo bar   </Element2>    

    Moo X

BAR

</Element1>";
            var expected =
@"<Element1 xml:space=""preserve"">       

                <Element2>   Foo bar   </Element2>    

    Moo X

BAR

</Element1>";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_NestedDefault_NormalFormattingResumed()
        {
            // Arrange
            var source =
@"<Element1 xml:space=""preserve"">       

                <Element2 xml:space=""default"">   Foo bar   </Element2>    

    Moo X

BAR

</Element1>";
            var expected =
@"<Element1 xml:space=""preserve"">       

                <Element2 xml:space=""default"">Foo bar</Element2>    

    Moo X

BAR

</Element1>";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_DeepNestedDefault_NormalFormattingResumed()
        {
            // Arrange
            var source =
@"<Element1 xml:space=""preserve"" Attribute1=""Moo"" Attribute2=""Foo"">       

                <Element2 xml:space=""default"" Attribute1=""Zoo"" Attribute2=""{Binding Path=Height, RelativeSource={RelativeSource Mode=FindAncestor, AncestorType=views:MainView}}"">   <Element3><Element4>       Foo bar     </Element4></Element3>  </Element2>    

    Moo X

BAR

</Element1>";
            var expected =
@"<Element1
    xml:space=""preserve""
    Attribute1=""Moo""
    Attribute2=""Foo"">       

                <Element2
        xml:space=""default""
        Attribute1=""Zoo""
        Attribute2=""{Binding
            Path=Height,
            RelativeSource={RelativeSource
                Mode=FindAncestor,
                AncestorType=views:MainView}}"">
        <Element3>
            <Element4>Foo bar</Element4>
        </Element3>
    </Element2>    

    Moo X

BAR

</Element1>";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_SignificantWhiteSpaceContainsSingleLineComment_SpacePreserved()
        {
            // Arrange
            var source =
@"<Element1 xml:space=""preserve"">       

          
                        <!--  
    
  I'm a lovely comment   
        -->       


</Element1>";
            var expected =
@"<Element1 xml:space=""preserve"">       

          
                        <!-- I'm a lovely comment -->       


</Element1>";

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Format_SignificantWhiteSpaceContainsMultiLineComment_SpacePreserved()
        {
            // Arrange
            var source =
@"<Element1 xml:space=""preserve"">       

          
                        <!--  
    
  I'm a lovely comment   1
        I'm a lovely comment  3 
    I'm a lovely comment   4
   I'm a lovely comment  2 
        -->       


</Element1>";
            var expected =
@"<Element1 xml:space=""preserve"">       

          
                        <!--
        I'm a lovely comment   1
        I'm a lovely comment  3
        I'm a lovely comment   4
        I'm a lovely comment  2
    -->       


</Element1>";     

            // Act
            var actual = source.Format();

            // Assert
            var result = StringDiff.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.Details);
        }
    }
}