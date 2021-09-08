using BotInstaller.Installer.Specific;
using BotInstaller.SysArchitecture;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BotInstaller.Installer
{
    public static class InstallerBuilder
    {
        public static Installer GetInstallerFor(OSVersion os)
        {
            string folder = Process.GetCurrentProcess().MainModule.FileName;
            folder = Path.GetDirectoryName(folder);
            folder = Path.Combine(folder, "installer-logs");
            Logger logger = new Logger(folder);

            return os.OS switch
            {
                OS.Windows => new WindowsInstaller(logger),
                _ => null
            };
        }
    }
}
