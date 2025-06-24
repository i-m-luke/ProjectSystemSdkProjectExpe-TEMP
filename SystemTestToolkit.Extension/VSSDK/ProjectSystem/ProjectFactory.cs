namespace SystemTestToolkit.Extension.VSSDK.ProjectSystem;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

using System;

public class ProjectFactory : IVsProjectFactory
{
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

        try
        {
            // Delegate to default project factory for SDK-style projects
            var serviceProvider = package as IServiceProvider;
            var solution = serviceProvider.GetService(typeof(SVsSolution)) as IVsSolution;

            // Let VS handle the SDK-style project creation
            return VSConstants.S_OK;
        }
        catch
        {
            return VSConstants.E_FAIL;
        }
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