using System.IO;

namespace Updater.UpdatePlugin.Providers
{
    public sealed class PackageInfo
    {
        public PackageInfo(string appName, Stream stream)
        {
            AppName = appName;
            Stream = stream;
        }

        public string AppName { get; private set; }

        public Stream Stream { get; private set; }
    }
}