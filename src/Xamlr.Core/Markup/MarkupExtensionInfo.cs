// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarkupExtensionInfo.cs" company="Xamlr">
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
//   Defines the MarkupExtensionInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Markup
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using JetBrains.Annotations;

    /// <summary>
    /// Represents information about a markup extension.
    /// </summary>
    [DebuggerDisplay("[{TypeName} Argument={arguments.Length}]")]
    public class MarkupExtensionInfo : IMarkupExtensionInfo
    {
        /// <summary>
        /// The arguments.
        /// </summary>
        private readonly IMarkupExtensionArgumentInfo[] arguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkupExtensionInfo"/> class.
        /// </summary>
        /// <param name="typeName">
        /// The type name.
        /// </param>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        public MarkupExtensionInfo(
            [NotNull] string typeName, [NotNull] IEnumerable<IMarkupExtensionArgumentInfo> arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            this.TypeName = typeName ?? throw new ArgumentNullException(nameof(typeName));
            this.arguments = arguments as IMarkupExtensionArgumentInfo[] ?? arguments.ToArray();
        }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        public IEnumerable<IMarkupExtensionArgumentInfo> Arguments => this.arguments;

        /// <summary>
        /// Gets a value indicating whether the markup extension has nested arguments.
        /// </summary>
        public bool HasNestedArguments
        {
            get { return this.Arguments.Any(candidate => candidate.IsNested); }
        }

        /// <summary>
        /// Gets the type name.
        /// </summary>
        public string TypeName { get; private set; }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current 
        /// <see cref="MarkupExtensionInfo"/>.
        /// </summary>
        /// <returns>
        /// A value indicating whether <paramref name="obj"/> is equal to the current 
        /// <see cref="MarkupExtensionInfo"/>.
        /// </returns>
        /// <param name="obj">
        /// The <see cref="object"/> to compare with the current <see cref="MarkupExtensionInfo"/>.
        /// </param>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(null, obj))
            {
                return false;
            }

            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == this.GetType() && this.Equals((MarkupExtensionInfo)obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return this.Arguments.Aggregate(
                    this.TypeName.GetHashCode() * 331,
                    (hash, arg) => hash ^ (arg.GetHashCode() * 331));
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="MarkupExtensionInfo"/> is equal to the current 
        /// <see cref="MarkupExtensionInfo"/>.
        /// </summary>
        /// <returns>
        /// A value indicating whether <paramref name="other"/> is equal to the current <see cref="MarkupExtensionInfo"/>.
        /// </returns>
        /// <param name="other">
        /// The <see cref="MarkupExtensionInfo"/> to compare with the current <see cref="MarkupExtensionInfo"/>.
        /// </param>
        protected bool Equals(MarkupExtensionInfo other) =>
            string.Equals(this.TypeName, other.TypeName) &&
            this.Arguments.SequenceEqual(other.Arguments);
    }
}