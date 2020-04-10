using System;

namespace Updater.UpdatePlugin.WebApiProvider
{
    internal sealed class FileInVm : IVm
    {
        public string Hash { get; set; }
        public string AppName { get; set; }
    }

    internal sealed class FileOutVm : IVm
    {
        public Guid AppGuid { get; set; }
    }
}