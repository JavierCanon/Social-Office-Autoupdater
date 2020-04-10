using System;
using Microsoft.Owin.Hosting;
using Updater.WebApi.SystemConfiguration;

namespace Updater.WebApi
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (WebApp.Start<Startup>(ConfigurationProvider.Host))
            {
                Console.ReadLine();
            }
        }
    }
}