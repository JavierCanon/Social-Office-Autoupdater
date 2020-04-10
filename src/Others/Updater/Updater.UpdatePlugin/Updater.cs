using System;
using System.Collections.Generic;
using System.Linq;
using Updater.UpdatePlugin.Providers;
using Updater.UpdatePlugin.WebApiProvider;

namespace Updater.UpdatePlugin
{
    public sealed class Updater
    {
        public static void Init(
            ISecurityProvider securityProvider,
            bool alwaysCheck,
            IUpdateHandler updateHandler,
            ISourceProvider[] sourceProviders)
        {
            if (securityProvider == null || updateHandler == null)
            {
                throw new ArgumentException("Parameters missed!");
            }

            var sp = new List<ISourceProvider> { new WebApiSourceProvider(securityProvider) };
            if (sourceProviders != null && sourceProviders.Any())
            {
                sp.AddRange(sourceProviders);
            }

            var updateManager = new UpdateManager(updateHandler, sp);
            updateManager.InitialCheck();
            if (alwaysCheck)
            {
                updateManager.Subscribe();
            }
        }
    }
}