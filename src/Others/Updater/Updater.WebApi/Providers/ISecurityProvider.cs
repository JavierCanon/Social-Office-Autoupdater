using System.Collections.Generic;

namespace Updater.WebApi.Providers
{
    public interface ISecurityProvider
    {
        bool IsValid(Dictionary<string, string> token);
    }
}