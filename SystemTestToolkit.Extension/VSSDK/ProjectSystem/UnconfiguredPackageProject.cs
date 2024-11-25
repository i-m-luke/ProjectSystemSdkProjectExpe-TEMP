using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.VS;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using System;
using System.CodeDom;
using System.ComponentModel.Composition;

namespace SystemTestToolkit.Extension.VSSDK.ProjectSystem
{
    [ProjectTypeRegistration(
        projectTypeGuid: Constants.PackageProjectTypeGuidString,
        displayName: "Expe Package",
        displayProjectFileExtensions: "SystemTestPackage",
        defaultProjectExtension: ProjectExtension,
        language: ProjectLanguage,
        PossibleProjectExtensions = ProjectExtension,
        Capabilities = Capabilities)]
    internal sealed class UnconfiguredPackageProject
    {
        public const string ProjectExtension = "systestpack";

        public const string ProjectLanguage = "SystemTestPackage";

        public const string Capabilities =
#if DEBUG
            ProjectCapabilities.DiagnoseCapabilities + ";" + // Support for diagnosing project capabilities
#endif
            ProjectLanguage + ";" + // Custom capability for the project type
            ProjectCapabilities.Cps + ";" +
            ProjectCapabilities.CSharp + ";" +
            ProjectCapabilities.ProjectReferences + ";" +
            ProjectCapabilities.PreserveFormatting + ";" +
            ProjectCapabilities.HandlesOwnReload + ";" +
            ProjectCapabilities.ProjectConfigurationsDeclaredDimensions + ";" +
            ProjectCapabilities.SharedImports + ";" +
            ProjectCapabilities.UseProjectEvaluationCache + ";" +
            "DependenciesTree;" + // Support for displaying dependencies tree in Solution Explorer
            "Pack;" + // Support for nuget pack
            "DotNetCore;" + // Support for .NET Core projects
            "AppDesigner;" + // Indicates that the project uses the app designer for managing project properties
            "EditAndContinue;" + // Indicates that the project supports the edit and continue debugging feature
            "UserSourceItems;" + // Indicates that the user is allowed to add arbitrary files to their project
            TestCapabilities;

        public const string TestCapabilities =
            ProjectCapabilities.ShowMissingItemTypes + ";" +
            ProjectCapabilities.ReferencesFolder + ";";

        [ImportingConstructor]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")]
        public UnconfiguredPackageProject(UnconfiguredProject unconfiguredProject)
        {
            this.ProjectHierarchies =
                new OrderPrecedenceImportCollection<IVsHierarchy>(projectCapabilityCheckProvider: unconfiguredProject);
        }

        [Import]
        internal UnconfiguredProject UnconfiguredProject { get; private set; }

        [Import]
        internal IActiveConfiguredProjectSubscriptionService SubscriptionService { get; private set; }

        [Import]
        internal IProjectThreadingService ProjectThreadingService { get; private set; }

        [Import]
        internal ActiveConfiguredProject<ConfiguredProject> ActiveConfiguredProject { get; private set; }

        [Import]
        internal ActiveConfiguredProject<ConfiguredPackageProject> MyActiveConfiguredProject { get; private set; }

        [ImportMany(ExportContractNames.VsTypes.IVsProject, typeof(IVsProject))]
        internal OrderPrecedenceImportCollection<IVsHierarchy> ProjectHierarchies { get; private set; }

        internal IVsHierarchy ProjectHierarchy => this.ProjectHierarchies.Single().Value;
    }
}