using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using Updater.UpdatePlugin.Providers;

namespace Updater.UpdatePlugin
{
    internal sealed class UpdateManager
    {
        private const string UpdatePackageName = "updates.zip";
        private const string UpdateFolderName = "updates";
        private const string UpdaterName = "Updater.Updater.exe";

        private readonly IUpdateHandler _updateHandler;
        private readonly List<ISourceProvider> _sourceProviders;

        public UpdateManager(
            IUpdateHandler updateHandler,
            List<ISourceProvider> sourceProviders)
        {
            _updateHandler = updateHandler;
            _sourceProviders = sourceProviders;
        }

        public void InitialCheck()
        {
            foreach (var sourceProvider in _sourceProviders)
            {
                if (sourceProvider.CheckForUpdates())
                {
                    ProcessUpdate(sourceProvider);
                    break;
                }
            }
        }

        public void Subscribe()
        {
            foreach (var sourceProvider in _sourceProviders)
            {
                sourceProvider.HaveUpdates += HaveUpdatesHandler;
                sourceProvider.CheckInBackground();
            }
        }

        private void ProcessUpdate(ISourceProvider sourceProvider)
        {
            string appPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().FullName);
            string zipPackageName = Path.Combine(appPath, UpdatePackageName);
            string updaterFolder = Path.Combine(appPath, UpdateFolderName);

            var packageInfo = sourceProvider.GetPackage();
            using (var fs = new FileStream(zipPackageName, FileMode.Create))
            {
                packageInfo.Stream.CopyTo(fs);
                fs.Close();
            }
            packageInfo.Stream.Dispose();

            if (Directory.Exists(updaterFolder))
            {
                Directory.Delete(updaterFolder, true);
            }

            ZipFile.ExtractToDirectory(zipPackageName, updaterFolder);
            File.Delete(zipPackageName);

            Process.Start(
                Path.Combine(updaterFolder, UpdaterName),
                String.Format("{0} {1}", Process.GetCurrentProcess().Id, Path.Combine(appPath, packageInfo.AppName)));
        }

        private void HaveUpdatesHandler(ISourceProvider sourceProvider)
        {
            if (_updateHandler.HaveUpdates())
            {
                ProcessUpdate(sourceProvider);
            }
        }
    }
}