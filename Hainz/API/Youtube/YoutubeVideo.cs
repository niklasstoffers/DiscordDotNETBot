using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.API.Youtube
{
    public class YoutubeVideo
    {
        public string Title { get; set; }
        public TimeSpan Length { get; set; }
        public string Url { get; set; }
        public string ChannelName { get; set; }
    }
}
