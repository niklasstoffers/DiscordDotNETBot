using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordDotNetBot.API
{
    public class Endpoint
    {
        public string Url { get; set; }
        public string Method { get; set; }

        public Endpoint(string url, string method)
        {
            Url = url;
            Method = method;
        }
    }
}
