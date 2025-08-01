﻿namespace SystemTestToolkit.Extension
{
    internal static class Constants
    {
        /// <summary>
        /// VSSDK package GUID string.
        /// </summary>
        public const string PackageGuidString = "d8e11f07-4a61-4a5c-aa50-462a9e70cc65";

        public static class ProjectCapabilities
        {
            public const string SystemTestPackage = nameof(SystemTestPackage);
        }

        internal static class Files
        {
            public static class Extensions
            {
                public const string PackageManifestFile = "packagemanifest";
                public const string PackageManifestFileWithDot = ".packagemanifest";
                public const string PackageManifestFileRegexPattern = @".*\.packagemanifest$";
            }
        }

        internal static class Directories
        {
            internal static class Names
            {
                /// <summary>
                /// Package assets directory name
                /// </summary>
                public const string Assets = "Assets";

                public const string AssetsRegexPattern = "[Aa]ssets";
            }
        }
    }
}