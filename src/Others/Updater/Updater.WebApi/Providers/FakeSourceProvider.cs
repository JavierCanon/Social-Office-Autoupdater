using System;
using System.IO;
using System.Linq;

namespace Updater.WebApi.Providers
{
    public class FakeSourceProvider : ISourceProvider
    {
        private const string SourcePath = @"D:\temp\";
        private const string SearchPattern = @"*.zip";

        public string GetVersion(string appName)
        {
            var fileName = GetFileName(appName);
            if (String.IsNullOrWhiteSpace(fileName))
            {
                return null;
            }

            return PackageNameProvider.GetVersion(fileName);
        }

        public Stream GetFileStream(string appName)
        {
            var fileName = GetFileName(appName);
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                var mem = new MemoryStream();
                fs.CopyTo(mem);
                mem.Position = 0;
                return mem;
            }
        }

        private string GetFileName(string appName)
        {
            return Directory.GetFiles( Path.Combine(SourcePath, Path.GetFileNameWithoutExtension(appName).Replace('.', '_')), SearchPattern)
                .OrderByDescending(x => x)
                .FirstOrDefault();
        }
    }
}