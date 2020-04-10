using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Updater.Updater.Commands;

namespace Updater.Updater
{
    internal class Program
    {
        // argument 0 - process ID
        // argument 1 - full path to EXE file for start app
        private static void Main(string[] args)
        {
            try
            {
                if (args.Count() != 2)
                {
                    throw new Exception("missed parameters");
                }

                int processId;
                string appFullName = args[1];

                if (!Int32.TryParse(args[0], out processId) || !File.Exists(appFullName) || Path.GetExtension(appFullName) != ".exe")
                {
                    throw new Exception("invalid parameters");
                }

                var commands = new List<ICommand>
                {
                    new KillProcessCommand(processId),
                    new CopyContentCommand(),
                    new StartProcessCommand(appFullName)
                };

                commands.ForEach(x => x.Execute());

                Console.WriteLine("done");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}