using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Updater.WebApi.SystemConfiguration
{
    internal sealed class ConfigurationProvider
    {
        public static string Host
        {
            get { return ConfigurationManager.AppSettings["Host"]; }
        }

        public static int RefreshTimeout
        {
            get { return Int32.Parse(ConfigurationManager.AppSettings["RefreshTimeout"]); }
        }

        public static List<Application> Applications
        {
            get
            {
                var applications = ConfigurationManager.GetSection("applications") as Applications;
                if (applications == null || applications.Items.Count == 0)
                {
                    throw new ArgumentException("configuration section 'apllications' missed");
                }

                return applications.Items.Cast<Application>().ToList();
            }
        }
    }
}