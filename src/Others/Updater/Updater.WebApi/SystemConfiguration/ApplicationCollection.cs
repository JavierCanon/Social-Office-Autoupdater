using System.Configuration;

namespace Updater.WebApi.SystemConfiguration
{
    public sealed class ApplicationCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new Application();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Application)element).AppGuid;
        }
    }
}