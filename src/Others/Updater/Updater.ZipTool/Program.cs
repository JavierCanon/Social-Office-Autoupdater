using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

namespace Updater.ZipTool
{
    class Program
    {
        private const string Temp = "temp";
        private const string App = "app";
        private const string Updater = "Updater.Updater.exe";

        // argument 0 - path to EXE file (source)
        // argument 1 - path to folder where must be stored appfolder with archive
        static void Main(string[] args)
        {
            try
            {
                if (args.Count() != 2)
                {
                    throw new Exception("missed parameters");
                }

                var sourceFile = args[0];
                var destinationPath = args[1];

                if (!File.Exists(sourceFile) || Path.GetExtension(sourceFile) != ".exe" || !Directory.Exists(destinationPath))
                {
                    throw new Exception("invalid parameters");
                }

                var appName = Path.GetFileNameWithoutExtension(sourceFile);
                var appFolder = appName.Replace('.', '_');
                var sourcePath = Path.GetDirectoryName(sourceFile);
                var versionInfo = FileVersionInfo.GetVersionInfo(sourceFile);
                var zipName = PackageNameProvider.Generate(versionInfo);

                var currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().FullName);
                var tempPath = Path.Combine(currentPath, appFolder, Temp);

                CopyContent(sourcePath, Path.Combine(tempPath, App));
                CopyUpdater(currentPath, tempPath);

                ZipFile.CreateFromDirectory(tempPath, Path.Combine(destinationPath, appFolder, zipName));

                Directory.Delete(tempPath, true);
                Console.WriteLine("done");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            Console.ReadLine();
        }

        private static void CopyContent(string from, string to)
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

        private static void CopyUpdater(string from, string to)
        {
            File.Copy(Path.Combine(from, Updater), Path.Combine(to, Updater));
        }
    }
}