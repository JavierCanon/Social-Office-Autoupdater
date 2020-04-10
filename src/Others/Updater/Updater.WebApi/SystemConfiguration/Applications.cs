using System.Configuration;

namespace Updater.WebApi.SystemConfiguration
{
    public sealed class Applications : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public ApplicationCollection Items
        {
            get { return (ApplicationCollection) this[""]; }
            set { this[""] = value; }
        }
    }
}