using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace YoutubeDlServiceWorker
{
    class Program
    {
        // Bugworkaround project for youtube-dl. When youtube-dl is started as a child process the standardoutput can't be read. This project introduces a workaround for that
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Out.WriteLine("Invalid program use.");
                Environment.Exit(-1);
            }

            string commandOutput;
            string arguments = string.Join(" ", args.Take(args.Length - 1)); 
            string dumpFilePath = args[args.Length - 1]; // last argument is file path

            try
            {
                using (var process = Process.Start(new ProcessStartInfo()
                {
                    FileName = "youtube-dl",
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }))
                {
                    using (var output = process.StandardOutput)
                    {
                        commandOutput = output.ReadToEnd();
                    }
                }

                File.WriteAllText(dumpFilePath, commandOutput);
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine($"Program execution error. Internal message: {ex.Message}");
                Environment.Exit(-1);
            }
        }
    }
}
