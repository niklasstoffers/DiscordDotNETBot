using System;
using System.Collections.Generic;
using System.Text;

namespace Hainz.Config
{
    public class BotConfig
    {
        public string Token { get; set; }
        public string YoutubeAPIKey { get; set; }
        public string StatusGameName { get; set; }
        public LoggerConfig LoggerConfig { get; set; }

        [ChecksumIgnore]
        public string Checksum { get; set; }

        public void LoadDefaults()
        {
            StatusGameName = "dir etwas vor. !play";
            LoggerConfig = new LoggerConfig()
            {
                EnableFileLogging = false,
                LogDirectory = "/logs"
            };
        }
    }
}
