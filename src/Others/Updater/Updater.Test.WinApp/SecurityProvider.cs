using Updater.UpdatePlugin.Providers;

namespace Updater.Test.WinApp
{
    public class SecurityProvider : ISecurityProvider
    {
        public string GetToken()
        {
            return "qwe";
        }
    }
}