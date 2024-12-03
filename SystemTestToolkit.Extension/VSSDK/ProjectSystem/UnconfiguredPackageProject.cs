using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.VS;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using System;
using System.CodeDom;
using System.ComponentModel.Composition;

using ProjectSystemProjectCapabilities = Microsoft.VisualStudio.ProjectSystem.ProjectCapabilities;

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

        public const string ProjectLanguage = Constants.ProjectCapabilities.SystemTestPackage;

        // NOTE:
        // When non-SDK project is loaded the project has these capabilites:
        // .NET;AllTargetOutputGroups;AppSettings;AssemblyReferences;ClassDesigner;COMReferences;ConfigurableFileNesting;CPS;CSharp;DataSourceWindow;DeclaredSourceItems;DependenciesTree;DiagnoseCapabilities;DisableBuiltinDebuggerServices;DynamicDependentFile;FallbackProjectConfigurationService;FolderPublish;HostSetActiveProjectConfiguration;IntegratedConsoleDebugging;LaunchProfiles;Managed;Microsoft.VisualStudio.ProjectSystem.RetailRules;NoGeneralDependentFileIcon;OutputGroups;PersistDesignTimeDataOutOfProject;ProjectImportsTree;ProjectPropertiesEditor;ProjectPropertyInterception;ProjectReferences;Publish;RelativePathDerivedDefaultNamespace;RunningInVisualStudio;SharedProjectReferences;SingleFileGenerators;SupportAvailableItemName;SystemTestPackage;UseFileGlobs;UserSourceItems;VisualStudioWellKnownOutputGroups;WinRTReferences
        // Problematic A : HostSetActiveProjectConfiguration;IntegratedConsoleDebugging;LaunchProfiles;Managed;
        public const string Capabilities =
#if DEBUG
            ProjectSystemProjectCapabilities.DiagnoseCapabilities + ";" + // Support for diagnosing project capabilities
#endif
            Constants.ProjectCapabilities.SystemTestPackage + ";";
        //Constants.ProjectCapabilities.SystemTestPackage + ";" + // Custom capability for the project type
        //ProjectSystemProjectCapabilities.Cps + ";" +
        //ProjectSystemProjectCapabilities.CSharp + ";" +
        //ProjectSystemProjectCapabilities.ProjectReferences + ";" +
        //ProjectSystemProjectCapabilities.PreserveFormatting + ";" +
        //ProjectSystemProjectCapabilities.HandlesOwnReload + ";" +
        //ProjectSystemProjectCapabilities.ProjectConfigurationsDeclaredDimensions + ";" +
        //ProjectSystemProjectCapabilities.SharedImports + ";" +
        //ProjectSystemProjectCapabilities.UseProjectEvaluationCache + ";" +
        //ProjectSystemProjectCapabilities.ShowMissingItemTypes + ";" +
        //"DependenciesTree;" + // Support for displaying dependencies tree in Solution Explorer
        //"Pack;" + // Support for nuget pack
        //".NET;" + // Indicates that the project for .NET 
        //"DotNetCore;" + // Indicates that the project .NET Core projects
        //"AppDesigner;" + // Indicates that the project uses the app designer for managing project properties
        //"EditAndContinue;" + // Indicates that the project supports the edit and continue debugging feature
        //"UserSourceItems;"; // Indicates that the user is allowed to add arbitrary files to their project


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