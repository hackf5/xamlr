namespace Xamlr
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Threading;
    using EnvDTE;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using Task = System.Threading.Tasks.Task;

    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideOptionPage(typeof(ToolsOptions), "Xamlr", "General", 0, 0, true)]
    [Guid(XamlrPackage.PackageGuidString)]
    public sealed class XamlrPackage : AsyncPackage
    {
        /// <summary>
        /// XamlrPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "6476211c-0f53-40ff-91a7-c087089767a4";

        /// <summary>
        /// The Visual Studio 2K Command GUID.
        /// </summary>
        private static readonly string VsStd2KCmdIdGuid = typeof(VSConstants.VSStd2KCmdID).GUID.ToString("B");

        /// <summary>
        /// The Visual Studio 97 Command GUID.
        /// </summary>
        private static readonly string VsStd97CmdIdGuid = typeof(VSConstants.VSStd97CmdID).GUID.ToString("B");

        /// <summary>
        /// The <see cref="FormatDocumentCommand"/> factory.
        /// </summary>
        private readonly Func<FormatDocumentCommand> formatDocumentCommandFactory;

        /// <summary>
        /// The development environment.
        /// </summary>
        private DTE dte;

        /// <summary>
        /// The format document event.
        /// </summary>
        private CommandEvents formatDocumentEvent;

        /// <summary>
        /// The save item event.
        /// </summary>
        private CommandEvents saveProjectItemEvent;

        /// <summary>
        /// The save solution event.
        /// </summary>
        private CommandEvents saveSolutionEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="XamlrPackage"/> class. 
        /// </summary>
        public XamlrPackage()
        {
            Debug.WriteLine("Entering constructor for: {0}", this);
            this.formatDocumentCommandFactory = () => new FormatDocumentCommand(this.dte);
        }

        /// <summary>
        /// Gets the tools options.
        /// </summary>
        private ToolsOptions ToolsOptions => (ToolsOptions)this.GetDialogPage(typeof(ToolsOptions)).AutomationObject;

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            this.dte = (DTE)await this.GetServiceAsync(typeof(DTE));

            if (this.dte == null)
            {
                throw new InvalidOperationException("DTE is null");
            }

            this.formatDocumentEvent = this.dte.Events.CommandEvents[XamlrPackage.VsStd2KCmdIdGuid, (int)VSConstants.VSStd2KCmdID.FORMATDOCUMENT];
            this.saveProjectItemEvent = this.dte.Events.CommandEvents[XamlrPackage.VsStd97CmdIdGuid, (int)VSConstants.VSStd97CmdID.SaveProjectItem];
            this.saveSolutionEvent = this.dte.Events.CommandEvents[XamlrPackage.VsStd97CmdIdGuid, (int)VSConstants.VSStd97CmdID.SaveSolution];

            this.formatDocumentEvent.BeforeExecute += this.OnFormatDocumentBeforeExecute;
            this.saveProjectItemEvent.BeforeExecute += this.OnSaveProjectItemBeforeExecute;
            this.saveSolutionEvent.BeforeExecute += this.OnSaveSolutionBeforeExecute;
        }

        /// <summary>
        /// Handles the Format Document 
        /// <seealso cref="CommandEvents.BeforeExecute"/>
        /// event.
        /// </summary>
        /// <param name="guid">
        /// The GUID that identifies the command family.
        /// </param>
        /// <param name="id">
        /// The ID that identifies the command.
        /// </param>
        /// <param name="customIn">
        /// The custom input parameters.
        /// </param>
        /// <param name="customOut">
        /// The custom output parameters.
        /// </param>
        /// <param name="cancelDefault">
        /// A value indicating whether the command has been cancelled.
        /// </param>
        private void OnFormatDocumentBeforeExecute(
            string guid, int id, object customIn, object customOut, ref bool cancelDefault)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            Debug.WriteLine("Entering OnFormatDocumentBeforeExecute() of: {0}", this);
            if (!this.ToolsOptions.Enabled)
            {
                Debug.WriteLine("Xamlr is disabled, exiting");
                return;
            }

            var command = this.formatDocumentCommandFactory();
            if (command.CanExecute(this.dte.ActiveDocument))
            {
                command.Execute(this.dte.ActiveDocument);
                cancelDefault = true;
            }
        }

        /// <summary>
        /// Handles the Save Document 
        /// <seealso cref="CommandEvents.BeforeExecute"/>
        /// event.
        /// </summary>
        /// <param name="guid">
        /// The GUID that identifies the command family.
        /// </param>
        /// <param name="id">
        /// The ID that identifies the command.
        /// </param>
        /// <param name="customIn">
        /// The custom input parameters.
        /// </param>
        /// <param name="customOut">
        /// The custom output parameters.
        /// </param>
        /// <param name="cancelDefault">
        /// A value indicating whether the command has been cancelled.
        /// </param>
        private void OnSaveProjectItemBeforeExecute(
            string guid, int id, object customIn, object customOut, ref bool cancelDefault)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            Debug.WriteLine("Entering OnFormatDocumentBeforeExecute() of: {0}", this);
            if (!this.ToolsOptions.Enabled || !this.ToolsOptions.FormatOnSave)
            {
                Debug.WriteLine("Xamlr is disabled, exiting");
                return;
            }

            var command = this.formatDocumentCommandFactory();
            if (command.CanExecute(this.dte.ActiveDocument))
            {
                command.Execute(this.dte.ActiveDocument);
            }
        }

        /// <summary>
        /// Handles the Save Document 
        /// <seealso cref="CommandEvents.BeforeExecute"/>
        /// event.
        /// </summary>
        /// <param name="guid">
        /// The GUID that identifies the command family.
        /// </param>
        /// <param name="id">
        /// The ID that identifies the command.
        /// </param>
        /// <param name="customIn">
        /// The custom input parameters.
        /// </param>
        /// <param name="customOut">
        /// The custom output parameters.
        /// </param>
        /// <param name="cancelDefault">
        /// A value indicating whether the command has been cancelled.
        /// </param>
        private void OnSaveSolutionBeforeExecute(
            string guid, int id, object customIn, object customOut, ref bool cancelDefault)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            Debug.WriteLine("Entering OnFormatDocumentBeforeExecute() of: {0}", this);
            if (!this.ToolsOptions.Enabled || !this.ToolsOptions.FormatOnSave)
            {
                Debug.WriteLine("Xamlr is disabled, exiting");
                return;
            }

            foreach (Document document in this.dte.Documents)
            {
                var command = this.formatDocumentCommandFactory();
                if (command.CanExecute(document))
                {
                    command.Execute(document);
                }
            }
        }
    }
}