using System.Collections.Generic;

namespace Updater.WebApi.Providers
{
    public class FakeSecurityProvider : ISecurityProvider
    {
        public bool IsValid(Dictionary<string, string> token)
        {
            return true;
        }
    }
}