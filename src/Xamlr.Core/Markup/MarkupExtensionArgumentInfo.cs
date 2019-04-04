// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarkupExtensionArgumentInfo.cs" company="Xamlr">
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
//   Defines the MarkupExtensionArgumentInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Markup
{
    using System;
    using System.Diagnostics;
    using JetBrains.Annotations;

    /// <summary>
    /// Represents information about an argument in a <see cref="MarkupExtensionInfo"/>.
    /// </summary>
    [DebuggerDisplay("{MemberNameValue}={StringValue}")]
    public class MarkupExtensionArgumentInfo : IMarkupExtensionArgumentInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarkupExtensionArgumentInfo"/> class.
        /// </summary>
        /// <param name="memberName">
        /// The value of the MEMBERNAME component.
        /// </param>
        /// <param name="memberNameDetails">
        /// The details associated with the MEMBERNAME component.
        /// </param>
        /// <param name="string">
        /// The value of the STRING component.
        /// </param>
        /// <param name="stringDetails">
        /// The details associated with the STRING component.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any of the non-nullable parameters is null.
        /// </exception>
        public MarkupExtensionArgumentInfo(
            string memberName,
            IMarkupExtensionTokenDetails memberNameDetails,
            [NotNull] object @string,
            [NotNull] IMarkupExtensionTokenDetails stringDetails)
        {
            this.MemberNameValue = memberName;
            this.MemberNameDetails = memberNameDetails;
            this.StringValue = @string ?? throw new ArgumentNullException(nameof(@string));
            this.StringDetails = stringDetails ?? throw new ArgumentNullException(nameof(stringDetails));
            this.IsNested = @string is MarkupExtensionInfo;
        }

        /// <summary>
        /// Gets the type of the argument.
        /// </summary>
        public MarkupExtensionArgumentType ArgumentType =>
            this.MemberNameValue == null
                ? MarkupExtensionArgumentType.Positional
                : MarkupExtensionArgumentType.Named;

        /// <summary>
        /// Gets a value indicating whether the argument is empty. The criterion for emptiness is that in the source
        /// string it consisted entirely of whitespace.
        /// </summary>
        public bool IsEmpty =>
            !this.IsNested && this.ArgumentType == MarkupExtensionArgumentType.Positional
                           && !this.StringDetails.IsQuoted && string.IsNullOrEmpty((string)this.StringValue);

        /// <summary>
        /// Gets a value indicating whether is the STRING component is itself a <see cref="MarkupExtensionInfo"/>.
        /// </summary>
        public bool IsNested { get; private set; }

        /// <summary>
        /// Gets the details associated with the MEMBERNAME component.
        /// </summary>
        public IMarkupExtensionTokenDetails MemberNameDetails { get; private set; }

        /// <summary>
        /// Gets the value of the MEMBERNAME component.
        /// </summary>
        public string MemberNameValue { get; private set; }

        /// <summary>
        /// Gets the details associated with the STRING component.
        /// </summary>
        public IMarkupExtensionTokenDetails StringDetails { get; private set; }

        /// <summary>
        /// Gets the value of the STRING component. This is either a <see cref="string"/> or a 
        /// <see cref="MarkupExtensionInfo"/>. 
        /// </summary>
        public object StringValue { get; private set; }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current 
        /// <see cref="MarkupExtensionArgumentInfo"/>.
        /// </summary>
        /// <returns>
        /// A value indicating whether <paramref name="obj"/> is equal to the current 
        /// <see cref="MarkupExtensionArgumentInfo"/>.
        /// </returns>
        /// <param name="obj">
        /// The <see cref="object"/> to compare with the current <see cref="MarkupExtensionArgumentInfo"/>.
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

            return obj.GetType() == this.GetType() && this.Equals((MarkupExtensionArgumentInfo)obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="MarkupExtensionArgumentInfo"/>.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.MemberNameValue != null ? this.MemberNameValue.GetHashCode() : 0) * 331)
                       ^ (this.StringValue.GetHashCode() * 331)
                       ^ (this.StringDetails.IsQuoted.GetHashCode() * 331);
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="MarkupExtensionArgumentInfo"/> is equal to the current 
        /// <see cref="MarkupExtensionArgumentInfo"/>.
        /// </summary>
        /// <returns>
        /// A value indicating whether <paramref name="other"/> is equal to the current 
        /// <see cref="MarkupExtensionArgumentInfo"/>.
        /// </returns>
        /// <param name="other">
        /// The <see cref="MarkupExtensionArgumentInfo"/> to compare with the current 
        /// <see cref="MarkupExtensionArgumentInfo"/>.
        /// </param>
        protected bool Equals(MarkupExtensionArgumentInfo other) =>
            string.Equals(this.MemberNameValue, other.MemberNameValue)
            && object.Equals(this.StringValue, other.StringValue)
            && this.StringDetails.IsQuoted == other.StringDetails.IsQuoted;
    }
}