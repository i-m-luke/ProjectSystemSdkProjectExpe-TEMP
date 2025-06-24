using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;

using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.ProjectSystem;

namespace SystemTestToolkit.Extension.VSSDK.ProjectSystem
{
    /// <summary>
    /// Updates nodes in the project tree by overriding property values calcuated so far by lower priority providers.
    /// </summary>
    [Export(typeof(IProjectTreePropertiesProvider))]
    [AppliesTo(Constants.ProjectCapabilities.SystemTestPackage)]
    [Order((int)Order.TreePropertiesProvider)]
    internal sealed class ProjectTreePropertiesProvider : IProjectTreePropertiesProvider
    {
        // NOTE -  to list KnownMonikers list use:
        // a) Glyphlist: https://glyphlist.azurewebsites.net/knownmonikers/
        // b) KnownMonikers Explorer 2022
        private static readonly Dictionary<string, ImageMoniker> fileExtensionRegexesWithIcons =
            new Dictionary<string, ImageMoniker>()
            {
                { Constants.Files.Extensions.PackageManifestFileRegexPattern, KnownMonikers.Settings },
            };

        /// <summary>
        /// Calculates new property values for each node in the project tree.
        /// </summary>
        /// <param name="propertyContext">Context information that can be used for the calculation.</param>
        /// <param name="propertyValues">Values calculated so far for the current node by lower priority tree properties providers.</param>
        public void CalculatePropertyValues(
            IProjectTreeCustomizablePropertyContext propertyContext,
            IProjectTreeCustomizablePropertyValues propertyValues)
        {
            // IProjectTreeCustomizablePropertyValues.ExpandedIcon: Can be used for specifying the icon when the node is expaned

            var icon = GetTreeNodeIcon(propertyContext, propertyValues);

            if (icon.Equals(default(ImageMoniker)))
            {
                return;
            }

            propertyValues.Icon = icon.ToProjectSystemType();
        }

        /// <summary>
        /// Gets tree node icon basad on criteria.
        /// </summary>
        /// <param name="propertyContext"></param>
        /// <param name="propertyValues"></param>
        /// <returns></returns>
        private static ImageMoniker GetTreeNodeIcon(
            IProjectTreeCustomizablePropertyContext propertyContext,
            IProjectTreeCustomizablePropertyValues propertyValues)
        {
            // Project root node icon
            if (propertyValues.Flags.Contains(ProjectTreeFlags.Common.ProjectRoot))
            {
                return KnownMonikers.Settings;
            }

            // Filesystem items icons
            if (!propertyContext.ExistsOnDisk)
            {
                return default;
            }

            // Folder items icons
            var assetsFolderRegex = new Regex(Constants.Directories.Names.AssetsRegexPattern);
            if (propertyContext.IsFolder && assetsFolderRegex.Match(propertyContext.ItemName).Success)
            {
                return KnownMonikers.DocumentsFolder;
            }

            // File items icons
            foreach (var regexWithIcon in fileExtensionRegexesWithIcons)
            {
                var fileNameExtensionRegex = new Regex(regexWithIcon.Key);
                var fileNameContainsExtension = fileNameExtensionRegex.Match(propertyContext.ItemName).Success;

                if (propertyValues.Flags.Contains(ProjectTreeFlags.Common.SourceFile) &&
                    fileNameContainsExtension)
                {
                    return regexWithIcon.Value;
                }
            }

            return default;
        }
    }
}