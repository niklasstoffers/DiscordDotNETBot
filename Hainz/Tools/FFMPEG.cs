using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.Tools
{
    public static class FFMPEG
    {
        public static Process CreateToPCMConverter()
        {
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = "ffmpeg",
                Arguments = "-hide_banner -loglevel quiet -i - -c:a pcm_s16le -f s16le -ac 2 -ar 48k pipe:1",
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            return Process.Start(processStartInfo);
        }
    }
}
