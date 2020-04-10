using System.Diagnostics;
using System.Threading;

namespace Updater.Updater.Commands
{
    internal sealed class KillProcessCommand : ICommand
    {
        private readonly int _processId;

        public KillProcessCommand(int processId)
        {
            _processId = processId;
        }

        public void Execute()
        {
            Process.GetProcessById(_processId).Kill();
            Thread.Sleep(2000);
        }
    }
}