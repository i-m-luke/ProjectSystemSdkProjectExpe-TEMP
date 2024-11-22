using System;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Microsoft.VisualStudio.ProjectSystem;

namespace SystemTestToolkit.Extension.VSSDK.ProjectSystem
{
    [Export]
    [AppliesTo(Constants.PojectCapabilities.SystemTestPackage)]
    internal class ConfiguredPackageProject
    {
        [Import, SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "MEF")]
        internal ConfiguredProject ConfiguredProject { get; private set; }

        [Import, SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "MEF")]
        internal ProjectProperties Properties { get; private set; }
    }
}