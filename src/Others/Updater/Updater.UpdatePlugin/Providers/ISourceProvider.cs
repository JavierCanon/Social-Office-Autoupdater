using System;

namespace Updater.UpdatePlugin.Providers
{
    public interface ISourceProvider : IDisposable
    {
        bool CheckForUpdates();
        event Action<ISourceProvider> HaveUpdates;
        PackageInfo GetPackage();
        void CheckInBackground();
    }
}