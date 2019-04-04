// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormatDocumentCommand.cs" company="Xamlr">
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
// --------------------------------------------------------------------------------------------------------------------

namespace Xamlr
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;
    using EnvDTE;
    using Xamlr.Core;

    /// <summary>
    /// The format document command.
    /// </summary>
    public class FormatDocumentCommand
    {
        /// <summary>
        /// The development environment.
        /// </summary>
        private readonly DTE dte;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatDocumentCommand"/> class. 
        /// </summary>
        /// <param name="dte">
        /// The development environment.
        /// </param>
        public FormatDocumentCommand(DTE dte)
        {
            this.dte = dte ?? throw new ArgumentNullException(nameof(dte));
        }

        /// <summary>
        /// Gets a value indicating whether the command can execute.
        /// </summary>
        /// <param name="document">
        /// The document.
        /// </param>
        /// <returns>
        /// A value indicating whether the command can execute.
        /// </returns>
        public virtual bool CanExecute(Document document)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (document.ReadOnly || document.Language != "XAML")
            {
                Debug.WriteLine(
                    "Document '{0}' is not formattable: readonly={1}, language={2}",
                    document.FullName,
                    document.ReadOnly,
                    document.Language);

                return false;
            }

            Debug.WriteLine("Document '{0}' is formattable", document.FullName);
            return true;
        }

        /// <summary>
        /// Executes the format document command.
        /// </summary>
        /// <param name="document">
        /// The document to format.
        /// </param>
        public virtual void Execute(Document document)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var textDocument = (TextDocument)document.Object("TextDocument");

            var startPoint = textDocument.StartPoint.CreateEditPoint();
            var endPoint = textDocument.EndPoint.CreateEditPoint();

            var currentPoint = textDocument.Selection.ActivePoint;
            var originalLine = currentPoint.Line;
            var originalOffset = currentPoint.LineCharOffset;

            var unformatted = startPoint.GetText(endPoint);

            var options = this.CreateOptions();
            var builder = new StringBuilder();

            XamlFormatterFactory.Create(options).Format(unformatted, builder);

            startPoint.ReplaceText(endPoint, builder.ToString(), 0);

            if (originalLine <= textDocument.EndPoint.Line)
            {
                var linePoint = startPoint.CreateEditPoint();
                if (originalLine > 0)
                {
                    linePoint.LineDown(originalLine - 1);
                }

                originalOffset = Math.Min(originalOffset, linePoint.LineLength + 1);
                if (originalOffset > 0)
                {
                    textDocument.Selection.MoveToLineAndOffset(originalLine, originalOffset);
                }
                else
                {
                    textDocument.Selection.GotoLine(originalLine);
                }
            }
            else
            {
                var linePoint = startPoint.CreateEditPoint();
                linePoint.EndOfDocument();
                textDocument.Selection.MoveToPoint(linePoint);
            }
        }

        /// <summary>
        /// Creates the <see cref="XamlFormatterOptions"/> to be used by the formatter.
        /// </summary>
        /// <returns>
        /// The the <see cref="XamlFormatterOptions"/> to be used by the formatter.
        /// </returns>
        private XamlFormatterOptions CreateOptions()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            var xamlEditorProperties = this.dte.Properties["TextEditor", "XAML"];

            return new XamlFormatterOptions
            {
                IndentSize = Convert.ToInt32(xamlEditorProperties.Item("IndentSize").Value, CultureInfo.CurrentCulture)
            };
        }
    }
}