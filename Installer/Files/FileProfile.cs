using System;
using System.Collections.Generic;
using System.Text;

namespace Installer.Files
{
    public class FileProfile
    {
        public File Bot { get; set; }
        public File FFMPEG { get; set; }
        public File YoutubeDl { get; set; }
        public File YoutubeDlServiceReceiver { get; set; }
        public File YoutubeDlServiceWorker { get; set; }
        public File Opus { get; set; }
        public File Libsodium { get; set; }
    }
}
