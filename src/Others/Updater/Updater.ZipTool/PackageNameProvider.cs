using System;
using System.Diagnostics;

namespace Updater.ZipTool
{
    internal sealed class PackageNameProvider
    {
        public static string Generate(FileVersionInfo versionInfo)
        {
            return String.Format("{0}_{1}_{2}_{3}-{4}.zip",
                versionInfo.FileMajorPart,
                versionInfo.FileMinorPart,
                versionInfo.FileBuildPart,
                versionInfo.FilePrivatePart,
                DateTime.Now.ToString("dd_MM_yyyy"));
        }
    }
}