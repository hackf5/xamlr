// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestUtilities.cs" company="Xamlr">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 Brian Tyler
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
//   Defines the TestUtilities type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Test
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;

    public static class TestUtilities
    {
        public static string Format(this string source)
        {
            var builder = new StringBuilder();
            var formatter = XamlFormatterFactory.Create(new XamlFormatterOptions { IndentSize = 4 });
            formatter.Format(source, builder);

            var target = builder.ToString();
            return target;
        }

        public static string GetSource(this string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            name = $"{assembly.GetName().Name}.Resources.{name}";
            var stream = assembly.GetManifestResourceStream(name);
            if (stream == null)
            {
                throw new ArgumentException($"Resource '{name}' not found", nameof(name));
            }

            return new StreamReader(stream).ReadToEnd();
        }

        public static TEntity Naught<TEntity>(this TEntity entity) => entity;
    }
}