using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.VS;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using System;
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
        Capabilities = "SystemTestPackage")]
    internal sealed class UnconfiguredPackageProject
    {
        public const string ProjectExtension = "systestpack";

        public const string ProjectLanguage = "SystemTestPackage";

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