using System;
using System.Collections.Generic;
using System.Text;

namespace BotInstaller.Files
{
    public class File
    {
        public string Url { get; set; }
        public string Name { get; set; }

        public File(string url, string name)
        {
            Url = url;
            Name = name;
        }
    }
}
