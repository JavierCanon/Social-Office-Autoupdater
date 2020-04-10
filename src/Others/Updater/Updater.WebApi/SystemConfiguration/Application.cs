using System;
using System.Configuration;

namespace Updater.WebApi.SystemConfiguration
{
    public sealed class Application : ConfigurationElement
    {
        [ConfigurationProperty("appGuid", IsKey = true, IsRequired = true)]
        public Guid AppGuid { get { return Guid.Parse(base["appGuid"].ToString()); } }

        [ConfigurationProperty("appName", IsRequired = true)]
        public string AppName { get { return base["appName"].ToString(); } }
    }
}