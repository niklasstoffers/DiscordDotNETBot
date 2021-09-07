using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace YoutubeDlServiceReceiver
{
    class Program
    {
        // Bugworkaround project for youtube-dl. When youtube-dl is started as a child process the standardoutput can't be read. This project introduces a workaround for that
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.Out.WriteLine("Invalid program use.");
                Environment.Exit(-1);
            }

            using var processModule = Process.GetCurrentProcess().MainModule;
            string dir = Path.GetDirectoryName(processModule?.FileName);

            if (dir == null)
            {
                Console.WriteLine($"Couldn't resolve current directory.");
                Environment.Exit(-1);
            }

            if (!HasWritePermission(dir))
            {
                Console.Out.WriteLine($"No permission to create dump files in the current directory. ({dir})");
                Environment.Exit(-1);
            }

            string fileName = "dump{0}.txt";
            string dumpFile = "";
            int iteration = 0;

            do
            {
                string formattedFileName = string.Format(fileName, iteration);
                dumpFile = Path.Combine(dir, formattedFileName);
                iteration++;
            } while (File.Exists(dumpFile));

            string workerPath = "YoutubeDlServiceWorker";
            string arguments = string.Join(" ", args);
            arguments += $" {dumpFile}";

            try
            {
                using (var process = Process.Start(new ProcessStartInfo()
                {
                    FileName = workerPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }))
                {
                    process.WaitForExit();
                    if (process.ExitCode != 0)
                        throw new Exception("Internal worker exited with non zero exit code. This may occur due to a number of reasons including missing dependencies or wrong command format");
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine($"Execution error. Internal message: {ex.Message}");
                Environment.Exit(-1);
            }

            try
            {
                string content = File.ReadAllText(dumpFile);
                File.Delete(dumpFile);
                Console.Out.WriteLine(content);
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine($"File writing error. Internal message: {ex.Message}");
                Environment.Exit(-1);
            }
        }

        private static bool HasWritePermission(string dir)
        {
            try
            {
                string fileName = "";
                string path = "";

                do
                {
                    fileName = GenerateRandomFileName();
                    path = Path.Combine(dir, fileName);
                } while (File.Exists(path));

                File.Create(fileName).Dispose();
                File.Delete(fileName);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static string GenerateRandomFileName()
        {
            var random = new Random();
            char[] content = new char[50];
            for (int i = 0; i < content.Length; i++)
            {
                content[i] = (char)(random.Next('a', 'z'));
            }
            return new string(content);
        }

    }
}
