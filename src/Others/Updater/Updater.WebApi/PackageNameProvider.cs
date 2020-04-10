using System.IO;

namespace Updater.WebApi
{
    internal sealed class PackageNameProvider
    {
        public static string GetVersion(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName)
                .Split('-')[0]
                .Replace('_', '.');
        }
    }
}