// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FragmentToken.cs" company="Xamlr">
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
//   Defines the FragmentToken type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Xaml
{
    /// <summary>
    /// A token that represents a fragment of an XML document.
    /// </summary>
    public class FragmentToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FragmentToken"/> class.
        /// </summary>
        /// <param name="type">
        /// The type of fragment.
        /// </param>
        /// <param name="name">
        /// The name of the fragment if it has one.
        /// </param>
        /// <param name="content">
        /// The content of the fragment if it has any.
        /// </param>
        public FragmentToken(FragmentType type, string name, object content)
        {
            this.Type = type;
            this.Name = name;
            this.Content = content;
            this.ContentString = content as string ?? string.Empty;
        }

        /// <summary>
        /// Gets the type of fragment.
        /// </summary>
        public FragmentType Type { get; private set; }

        /// <summary>
        /// Gets the name of the fragment if it has one.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the content of the fragment if it has any.
        /// </summary>
        public object Content { get; private set; }

        /// <summary>
        /// Gets the content of the fragment cast to a string, or empty if <seealso cref="Content"/> is not a string.
        /// </summary>
        public string ContentString { get; private set; }
    }
}