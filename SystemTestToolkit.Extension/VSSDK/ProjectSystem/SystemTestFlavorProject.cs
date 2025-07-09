using Microsoft.VisualStudio.Shell.Flavor;
using Microsoft.VisualStudio.Shell.Interop;

using System;
using System.Runtime.InteropServices;

namespace SystemTestToolkit.Extension.VSSDK.ProjectSystem
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    internal class SystemTestFlavorProject : FlavoredProjectBase
    {
        public SystemTestFlavorProject(VssdkPackage package)
        {
            this.serviceProvider = package;
        }
    }
}