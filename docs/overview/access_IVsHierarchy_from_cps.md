# Accessing `IVsHierarchy` from CPS

The following sample uses an `IDeployProvider` to log the `Caption` property of the `IVsHierarchy`.

Notes:
- You should avoid accessing `IVsHierarchy` properties and try to use CPS mechanisms instead if possible
- You need to switch to the UI thread before calling any COM interfaces (e.g. IVsHierarchy) using
   ```csharp
   await ThreadHandling.SwitchToUIThread();
   ```
- Additional information about [ItemIds](ItemIDs.md) in CPS

```csharp
using System.ComponentModel.Composition;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Build;
using Microsoft.VisualStudio.Shell.Interop;

[Export(typeof(IDeployProvider))]
[AppliesTo(MyUnconfiguredProject.UniqueCapability)]
internal class DeployProvider1 : IDeployProvider
{
    [ImportingConstructor]
    public DeployProvider1(UnconfiguredProject unconfiguredProject)
    {
        this.IVsHierarchies = new OrderPrecedenceImportCollection<IVsHierarchy>(projectCapabilityCheckProvider: unconfiguredProject);
    }

    /// <summary>
    /// Gets or sets IVsHierarchies.
    /// </summary>
    [ImportMany(ExportContractNames.VsTypes.IVsHierarchy)]
    private OrderPrecedenceImportCollection<IVsHierarchy> IVsHierarchies { get; set; }

    /// <summary>
    /// Gets the IVsHierarchy instance for the project being built.
    /// </summary>
    internal IVsHierarchy VsHierarchy => this.IVsHierarchies.First().Value;

    [Import]
    internal IProjectThreadingService ProjectThreadingService { get; private set; }

    public async Task DeployAsync(CancellationToken cancellationToken, TextWriter outputPaneWriter)
    {
        await ProjectThreadingService.SwitchToUIThread();

        object caption;
        if (ErrorHandler.Succeeded(this.VsHierarchy.GetProperty((uint)VSConstants.VSITEMID.Root, (int)__VSHPROPID.VSHPROPID_Caption, out caption)))
        {
            await outputPaneWriter.WriteAsync(caption as string);
        }
    }

    public bool IsDeploySupported => true;

    public void Commit()
    {
    }

    public void Rollback()
    {
    }
}
```
