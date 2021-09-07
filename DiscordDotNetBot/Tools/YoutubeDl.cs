using DiscordDotNetBot.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiscordDotNetBot.Tools
{
    public static class YoutubeDl
    {
        private const string PROGRAM_NAME = "YoutubeDlServiceReceiver";

        public static async Task<List<YoutubeFormat>> GetFormats(string url)
        {
            try
            {
                string arguments = $"--list-formats {url}";
                string dump;

                using (var worker = InitWorker(arguments))
                {
                    using (var output = worker.StandardOutput)
                    {
                        dump = await output.ReadToEndAsync();
                    }
                    worker.WaitForExit();
                }

                List<YoutubeFormat> result = new List<YoutubeFormat>();
                string[] lines = dump.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                string regex = @"^((\d+)\s+(\w+)\s+(\w+.*))$";
                foreach (string line in lines)
                {
                    var match = Regex.Match(line, regex);
                    if (match.Success)
                    {
                        int formatCode = Int32.Parse(match.Groups[2].Value);
                        string format = match.Groups[3].Value;
                        string desc = match.Groups[4].Value;
                        result.Add(new YoutubeFormat(formatCode, format, desc));
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<string> GetUrl(int format, string url, bool opusAudio)
        {
            try
            {
                string arguments = $"--get-url -f {format} {(opusAudio ? $"--extract-audio --audio-format \"opus\"" : "")} {url}";
                string dump;

                using (var worker = InitWorker(arguments))
                {
                    using (var output = worker.StandardOutput)
                    {
                        dump = await output.ReadToEndAsync();
                    }
                }

                return dump;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<string> GetBestOpusAudio(string url)
        {
            try
            {
                string arguments = $"--get-url -f \"bestaudio/opus\" --extract-audio --audio-format \"opus\" {url}";
                string dump;

                using (var worker = InitWorker(arguments))
                {
                    using (var output = worker.StandardOutput)
                    {
                        dump = await output.ReadToEndAsync();
                    }
                }

                return dump;
            }
            catch
            {
                return null;
            }
        }

        private static Process InitWorker(string arguments)
        {
            return Process.Start(new ProcessStartInfo()
            {
                FileName = PROGRAM_NAME,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            });
        }
    }
}
