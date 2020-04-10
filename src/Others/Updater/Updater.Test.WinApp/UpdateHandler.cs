using Updater.UpdatePlugin.Providers;

namespace Updater.Test.WinApp
{
    public class UpdateHandler : IUpdateHandler
    {
        public bool HaveUpdates()
        {
            return true;
        }
    }
}