using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.Audio
{
    public class Music
    {
        public string Title { get; set; }
        public TimeSpan Length { get; set; }
        public string Url { get; set; }

        public Music(string title, TimeSpan length, string url)
        {
            Title = title;
            Length = length;
            Url = url;
        }
    }
}
