using System.Diagnostics;

namespace Updater.Updater.Commands
{
    internal sealed class StartProcessCommand : ICommand
    {
        private readonly string _appName;

        public StartProcessCommand(string appName)
        {
            _appName = appName;
        }

        public void Execute()
        {
            Process.Start(_appName);
        }
    }
}