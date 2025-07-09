namespace SystemTestToolkit.Extension.VSSDK.ProjectSystem;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Flavor;
using Microsoft.VisualStudio.Shell.Interop;

using System;
using System.Configuration;
using System.Runtime.InteropServices;

[Guid(GuidString)]
public class ProjectFactory : FlavoredProjectFactoryBase
{
    public const string GuidString = "d8e11f07-4a61-4a5c-aa50-462a9e70cc65";

    private readonly VssdkPackage package;

    public ProjectFactory(VssdkPackage package)
    {
        this.package = package;
    }

    protected override object PreCreateForOuter(IntPtr outerProjectIUnknown)
    {
        // Vytvoření a vrácení naší vlastní "flavor" třídy
        return new SystemTestFlavorProject(this.package);
    }

    protected override bool CanCreateProject(string fileName, uint flags)
    {
        return base.CanCreateProject(fileName, flags);
    }
}

// Factory can be empty to only leverage project template registration
[Guid(GuidString)]
public class EmptyProjectFactory
{
    public const string GuidString = "77a23a32-5212-4dd4-b79f-422379fc479a";
}

// Factory can be empty type and the ProvideProjectFactory works ...
//[Guid(GuidString)]
//public class ProjectFactory : IVsProjectFactory
//{
//    public const string GuidString = "d8e11f07-4a61-4a5c-aa50-462a9e70cc65";


//    private readonly VssdkPackage package;

//    public ProjectFactory(VssdkPackage package)
//    {
//        this.package = package;
//    }

//    public int CreateProject(string fileName, string location, string name,
//        uint flags, ref Guid projectGuid, out IntPtr project, out int canceled)
//    {
//        project = IntPtr.Zero;
//        canceled = 0;


//        return VSConstants.S_OK;
//    }

//    public int CanCreateProject(string fileName, uint flags, out int canCreate)
//    {
//        canCreate = 1;
//        return VSConstants.S_OK;
//    }

//    public int Close()
//    {
//        return VSConstants.S_OK;
//    }

//    public int SetSite(Microsoft.VisualStudio.OLE.Interop.IServiceProvider serviceProvider)
//    {
//        return VSConstants.S_OK;
//    }
//}