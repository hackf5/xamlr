// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringMatchRule.cs" company="Xamlr">
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
//   Defines the StringMatchRule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Xaml
{
    using System;

    /// <summary>
    /// A rule that determines whether a string is matched.
    /// </summary>
    public struct StringMatchRule
    {
        /// <summary>
        /// The pattern that defines the rule.
        /// </summary>
        private readonly string pattern;

        /// <summary>
        /// The type that determines how to apply the pattern.
        /// </summary>
        private readonly StringMatchType type;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringMatchRule"/> struct. 
        /// </summary>
        /// <param name="pattern">
        /// The pattern.
        /// </param>
        /// <param name="type">
        /// The type that determines how to apply the pattern.
        /// </param>
        public StringMatchRule(string pattern, StringMatchType type)
        {
            this.pattern = pattern ?? string.Empty;

            switch (type)
            {
                case StringMatchType.Exact:
                case StringMatchType.Prefix:
                case StringMatchType.Suffix:
                    this.type = type;
                    break;
                default:
                    throw new InvalidOperationException(
                        $"Unknown match type {type}");
            }
        }

        /// <summary>
        /// Gets the pattern that defines the rule.
        /// </summary>
        public string Pattern => this.pattern;

        /// <summary>
        /// Gets the type that determines how to apply the pattern.
        /// </summary>
        public StringMatchType Type => this.type;

        /// <summary>
        /// Indicates whether this instance and a specified <see cref="StringMatchRule"/> are equal.
        /// </summary>
        /// <param name="other">
        /// Another <see cref="StringMatchRule"/> to compare to.
        /// </param>
        /// <returns>
        /// A value indicating whether <paramref name="other"/> and this instance represent the same value.
        /// </returns>
        public bool Equals(StringMatchRule other) => string.Equals(this.pattern, other.pattern) && this.type == other.type;

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">
        /// Another object to compare to.
        /// </param>
        /// <returns>
        /// A value indicating whether <paramref name="obj"/> and this instance represent the same value.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is StringMatchRule && this.Equals((StringMatchRule)obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (this.pattern.GetHashCode() * 397) ^ (int)this.type;
            }
        }

        /// <summary>
        /// Determines whether <paramref name="candidate"/> is a match for the rule.
        /// </summary>
        /// <param name="candidate">
        /// The candidate.
        /// </param>
        /// <returns>
        /// A value indicating whether <paramref name="candidate"/> is a match for the rule.
        /// </returns>
        public bool IsMatch(string candidate)
        {
            if (candidate == null)
            {
                return false;
            }

            switch (this.Type)
            {
                case StringMatchType.Exact:
                    if (candidate.Equals(this.Pattern, StringComparison.Ordinal))
                    {
                        return true;
                    }

                    break;
                case StringMatchType.Prefix:
                    if (candidate.StartsWith(this.Pattern, StringComparison.Ordinal))
                    {
                        return true;
                    }

                    break;
                case StringMatchType.Suffix:
                    if (candidate.EndsWith(this.Pattern, StringComparison.Ordinal))
                    {
                        return true;
                    }

                    break;
            }

            return false;
        }
    }
}