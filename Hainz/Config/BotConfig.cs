using System;
using System.Collections.Generic;
using System.Text;

namespace Hainz.Config
{
    public class BotConfig
    {
        public static BotConfig Current { get; set; }

        public string Token { get; set; }
        public string YoutubeAPIKey { get; set; }

        [ChecksumIgnore]
        public string Checksum { get; set; }
    }
}
