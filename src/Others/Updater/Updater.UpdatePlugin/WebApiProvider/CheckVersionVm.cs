using System;

namespace Updater.UpdatePlugin.WebApiProvider
{
    internal sealed class CheckVersionInVm : IVm
    {
        public bool NeedUpdate { get; set; }
        public int RefreshTimeout { get; set; }
    }

    internal sealed class CheckVersionOutVm : IVm
    {
        public Guid AppGuid { get; set; }
        public string Version { get; set; }
    }
}