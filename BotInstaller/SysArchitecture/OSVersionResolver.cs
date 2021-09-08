using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BotInstaller.SysArchitecture
{
    public static class OSVersionResolver
    {
        public static OSVersion ResolveOS()
        {
            PlatformID platform = Environment.OSVersion.Platform;
            return platform switch
            {
                var x when
                    x == PlatformID.Win32NT ||
                    x == PlatformID.Win32S ||
                    x == PlatformID.Win32Windows ||
                    x == PlatformID.WinCE
                    => new OSVersion() { OS = OS.Windows, Architecture = ResolveArchitecture() },
                PlatformID.Unix => new OSVersion() { OS = OS.Linux, Architecture = ResolveArchitecture() },
                _ => null
            };
        }

        private static Architecture ResolveArchitecture() =>
            RuntimeInformation.ProcessArchitecture;
    }
}
