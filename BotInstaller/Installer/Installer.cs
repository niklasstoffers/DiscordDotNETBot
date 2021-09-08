using BotInstaller.SysArchitecture;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BotInstaller.Installer
{
    public abstract class Installer : IDisposable
    {
        protected Logger Logger { get; }
        protected string InstallFolderName { get => "DiscordBot"; }

        public Installer(Logger logger)
        {
            Logger = logger;
        }

        public abstract bool CheckPermission();
        public abstract bool SetInstallationPath(string path);
        public abstract InstallerResult Install(Architecture architecture);
        public abstract void Dispose();
    }
}
