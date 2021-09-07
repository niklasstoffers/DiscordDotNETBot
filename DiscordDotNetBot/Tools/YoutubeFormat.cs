using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordDotNetBot.Tools
{
    public readonly struct YoutubeFormat
    {
        public int FormatCode { get; }
        public string Format { get; }
        public string Description { get; }

        public YoutubeFormat(int formatCode, string format, string description)
        {
            FormatCode = formatCode;
            Format = format;
            Description = description;
        }
    }
}
