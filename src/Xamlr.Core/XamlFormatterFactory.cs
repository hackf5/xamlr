// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XamlFormatterFactory.cs" company="Xamlr">
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
//   Defines the XamlFormatterFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core
{
    using Xamlr.Core.Xaml;

    /// <summary>
    /// The <see cref="XamlFormatter"/> factory (in lieu of an IoC container).
    /// </summary>
    public static class XamlFormatterFactory
    {
        /// <summary>
        /// Creates a new <see cref="IFormatter"/>.
        /// </summary>
        /// <param name="options">
        /// The options with which to create it.
        /// </param>
        /// <returns>
        /// A new <see cref="IFormatter"/>.
        /// </returns>
        public static IFormatter Create(XamlFormatterOptions options)
        {
            return new FormattingContext(options).Create(FragmentType.Xaml);
        }
    }
}