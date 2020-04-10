using System.IO;

namespace Updater.WebApi.Providers
{
    public interface ISourceProvider
    {
        string GetVersion(string appName);
        Stream GetFileStream(string appName);
    }
}