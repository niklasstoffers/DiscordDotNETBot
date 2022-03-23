using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.Util
{
    public static class DirectoryUtil
    {
        public static string GetApplicationBaseDirectory() => Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
    }
}
