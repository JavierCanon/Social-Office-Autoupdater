using System;
using System.IO;
using System.Reflection;

namespace Updater.Updater.Commands
{
    internal sealed class CopyContentCommand : ICommand
    {
        private const string Updates = "updates";
        private const string App = "app";

        public void Execute()
        {
            var currentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var destinationdirInfo = Directory.GetParent(currentFolder);

            RemoveContent(destinationdirInfo);
            AddContent(Path.Combine(currentFolder, App), destinationdirInfo.FullName);

            Directory.Delete(Path.Combine(currentFolder, App), true);
        }

        private static void RemoveContent(DirectoryInfo destinationdirInfo)
        {
            foreach (var file in destinationdirInfo.GetFiles())
            {
                File.SetAttributes(file.FullName, FileAttributes.Normal);
                file.Delete();
            }

            foreach (var dir in destinationdirInfo.GetDirectories())
            {
                if (dir.Name != Updates)
                {
                    RemoveContent(dir);
                    dir.Delete();
                }
            }
        }

        private static void AddContent(string from, string to)
        {
            foreach (var file in Directory.GetFiles(from, "*.*", SearchOption.AllDirectories))
            {
                var newFile = file.Replace(from, to);
                var newPath = Path.GetDirectoryName(newFile);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                File.Copy(file, newFile);
            }
        }
    }
}