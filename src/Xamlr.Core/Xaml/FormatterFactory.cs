// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormatterFactory.cs" company="Xamlr">
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
//   Defines the FormatterFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr.Core.Xaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using JetBrains.Annotations;

    /// <summary>
    /// A factory that produces <see cref="IFormatter"/> instances.
    /// </summary>
    public class FormatterFactory
    {
        /// <summary>
        /// The individual factories that produce the formatters.
        /// </summary>
        private static readonly Dictionary<FragmentType, Func<FormattingContext, IFormatter>> Factories;

        /// <summary>
        /// The formatting context.
        /// </summary>
        private readonly FormattingContext context;

        /// <summary>
        /// Initializes static members of the <see cref="FormatterFactory"/> class.
        /// </summary>
        static FormatterFactory()
        {
            var parameter = Expression.Parameter(typeof(FormattingContext));
            FormatterFactory.Factories = Assembly
                .GetAssembly(typeof(FormatterFactory))
                .GetTypes()
                .Where(candidate => candidate.IsClass)
                .Where(candidate => typeof(IFormatter).IsAssignableFrom(candidate))
                .Where(candidate => candidate.GetCustomAttributes(typeof(FragmentFormatterAttribute), false).Any())
                .Select(type => new { Type = type, Ctor = type.GetConstructor(new[] { typeof(FormattingContext) }) })
                .Select(temp => new { temp.Type, Exp = Expression.Convert(Expression.New(temp.Ctor, parameter), typeof(IFormatter)) })
                .Select(temp => new { temp.Type, Exp = Expression.Lambda<Func<FormattingContext, IFormatter>>(temp.Exp, parameter) })
                .Select(temp => new { temp.Type, Factory = temp.Exp.Compile() })
                .ToDictionary(temp => temp.Type.GetCustomAttributes(false).OfType<FragmentFormatterAttribute>().First().Type, temp => temp.Factory);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatterFactory"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public FormatterFactory([NotNull] FormattingContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Creates a <see cref="IFormatter"/> that can format a fragment of type <paramref name="type"/>.
        /// </summary>
        /// <param name="type">
        /// The fragment type.
        /// </param>
        /// <returns>
        /// A <see cref="IFormatter"/> that can format a fragment of type <paramref name="type"/>.
        /// </returns>
        public IFormatter Create(FragmentType type) => FormatterFactory.Factories[type](this.context);
    }
}