using System;

namespace Updater.WebApi.Api
{
    public sealed class CheckVersionInVm
    {
        public Guid AppGuid { get; set; }
        public string Version { get; set; }
    }

    public sealed class CheckVersionOutVm
    {
        public CheckVersionOutVm(bool needUpdate, int refreshTimeout)
        {
            NeedUpdate = needUpdate;
            RefreshTimeout = refreshTimeout;
        }

        public bool NeedUpdate { get; set; }
        public int RefreshTimeout { get; set; }
    }
}