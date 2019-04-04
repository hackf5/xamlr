namespace Xamlr.Core.Xaml
{
    using System;
    using System.Text;

    [FragmentFormatter(FragmentType.XmlDeclaration)]
    public class XmlDeclarationFormatter : IFormatter
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly FormattingContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDeclarationFormatter"/> class. 
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public XmlDeclarationFormatter(FormattingContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public void Format(object obj, StringBuilder builder)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (!(obj is FragmentToken token))
            {
                throw new ArgumentException(@"Parameter must be of type FragmentToken", nameof(obj));
            }

            builder
                .Indent(this.context)
                .AppendFormat("<?{0} {1}?>", token.Name, token.Content)
                .AppendLine(this.context);
        }
    }
}