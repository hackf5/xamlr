// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringDiff.cs" company="Xamlr">
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
//   Defines the StringDiff type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Test
{
    using System;
    using System.Linq;

    internal static class StringDiff
    {
        public static StringDiffResult Compare(string expected, string actual)
        {
            if (expected == null)
            {
                throw new ArgumentNullException(nameof(expected));
            }

            if (actual == null)
            {
                throw new ArgumentNullException(nameof(actual));
            }

            var length = Math.Min(expected.Length, actual.Length);

            var comparison = Enumerable
                .Range(0, length)
                .Zip(expected, (i, c) => new { I = i, Expected = c })
                .Zip(actual, (z, c) => new { z.I, AreEqual = c.Equals(z.Expected) })
                .ToArray();

            var areEqual = expected.Length == actual.Length && comparison.All(x => x.AreEqual);

            if (areEqual)
            {
                return new StringDiffResult(true);
            }

            var firstDifference = comparison.FirstOrDefault(x => !x.AreEqual);
            var firstDifferenceIndex = firstDifference == null ? length : firstDifference.I;

            return StringDiff.CreateResult(expected, actual, firstDifferenceIndex);
        }

        private static string CreateAroundIndex(string value, int index)
        {
            var window = 20;
            var start = Math.Max(index - window, 0);
            var end = Math.Min(index + window, value.Length - 1);

            var substr = value.Substring(start, end - start).Replace('\n', '·').Replace('\r', '·').Replace('\t', '·');

            return $"{substr}\n{new string(' ', Math.Min(index, window))}^";
        }

        private static StringDiffResult CreateResult(string expected, string actual, int firstDifferenceIndex)
        {
            var details = $"Expected:\n{StringDiff.CreateAroundIndex(expected, firstDifferenceIndex)}\nActual:\n{StringDiff.CreateAroundIndex(actual, firstDifferenceIndex)}";

            return new StringDiffResult(false, firstDifferenceIndex, details);
        }
    }
}