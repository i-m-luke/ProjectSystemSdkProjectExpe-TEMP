using Microsoft.VisualStudio.Shell.Flavor;
using Microsoft.VisualStudio.Shell.Interop;

using System;

namespace SystemTestToolkit.Extension.VSSDK.ProjectSystem
{
    internal class SystemTestFlavorProject : FlavoredProjectBase
    {
        private readonly VssdkPackage _package;

        public SystemTestFlavorProject(VssdkPackage package)
        {
            _package = package;
        }


        protected override int GetProperty(uint itemid, int propid, out object pvar)
        {
            // Příklad přepsání vlastnosti: změna ikony projektu
            if (propid == (int)__VSHPROPID.VSHPROPID_IconIndex)
            {
                // Zde můžete vrátit index vlastní ikony, pokud ji máte v resources
            }

            return base.GetProperty(itemid, propid, out pvar);
        }
    }
}