using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Utilities;

using System.Threading.Tasks;
using System.Threading;

using EnvDTE;

using CommunityToolkit.Mvvm.Messaging;

using Microsoft.VisualStudio.Extensibility.Shell;


namespace SystemTestToolkit.Extension
{
    /// <summary>
    /// Extension entrypoint for the VisualStudio.Extensibility extension.
    /// </summary>
    [VisualStudioContribution]
    internal class VsExtensibilityEntrypoint : Microsoft.VisualStudio.Extensibility.Extension
    {
        /// <inheritdoc />
        public override ExtensionConfiguration ExtensionConfiguration => new()
        {
            RequiresInProcessHosting = true,
            LoadedWhen = ActivationConstraint.ActiveProjectCapability(
                ProjectCapability.Custom(Constants.ProjectCapabilities.SystemTestPackage))
        };

        /// <inheritdoc />
        protected override void InitializeServices(IServiceCollection serviceCollection)
        {
            base.InitializeServices(serviceCollection);

            // You can configure dependency injection here by adding services to the serviceCollection.
        }

        protected override async Task OnInitializedAsync(VisualStudioExtensibility extensibility, CancellationToken cancellationToken)
        {
            await base.OnInitializedAsync(extensibility, cancellationToken);
        }
    }
}