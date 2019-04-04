using Microsoft.VisualStudio.Shell;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Xamlr
{
    [ComVisible(true)]
    public class ToolsOptions : DialogPage
    {
        public ToolsOptions()
        {
            this.Enabled = true;
            this.FormatOnSave = true;
        }

        [Category("General")]
        [DisplayName("Enabled")]
        [Description("When true the default XAML document formatting behaviour will be overridden by this tool.")]
        [DefaultValue(true)]
        public bool Enabled { get; set; }

        [Category("General")]
        [DisplayName("Format on save")]
        [Description("When true all XAML documents will be formatted prior to save by this tool.")]
        [DefaultValue(true)]
        public bool FormatOnSave { get; set; }
    }
}
