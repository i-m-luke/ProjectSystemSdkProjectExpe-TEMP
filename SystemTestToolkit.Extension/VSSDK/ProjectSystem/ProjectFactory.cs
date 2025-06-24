namespace SystemTestToolkit.Extension.VSSDK.ProjectSystem;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

using System;
using System.Runtime.InteropServices;

// Factory can be empty type and the ProvideProjectFactory works ...
[Guid(GuidString)]
public class ProjectFactory : IVsProjectFactory
{
    public const string GuidString = "d8e11f07-4a61-4a5c-aa50-462a9e70cc65";


    private readonly VssdkPackage package;

    public ProjectFactory(VssdkPackage package)
    {
        this.package = package;
    }

    public int CreateProject(string fileName, string location, string name,
        uint flags, ref Guid projectGuid, out IntPtr project, out int canceled)
    {
        project = IntPtr.Zero;
        canceled = 0;
        return VSConstants.S_OK;
    }

    public int CanCreateProject(string fileName, uint flags, out int canCreate)
    {
        canCreate = 1;
        return VSConstants.S_OK;
    }

    public int Close()
    {
        return VSConstants.S_OK;
    }

    public int SetSite(Microsoft.VisualStudio.OLE.Interop.IServiceProvider serviceProvider)
    {
        return VSConstants.S_OK;
    }
}