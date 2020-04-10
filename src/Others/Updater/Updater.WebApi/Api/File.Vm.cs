using System;

namespace Updater.WebApi.Api
{
    public sealed class FileInVm
    {
        public Guid AppGuid { get; set; }
    }


    public sealed class FileOutVm
    {
        public FileOutVm(string hash, string appName)
        {
            Hash = hash;
            AppName = appName;
        }

        public string Hash { get; set; }
        public string AppName { get; set; }
    }
}