using BotInstaller.SysArchitecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotInstaller.Installer.Specific
{
    public class LinuxInstaller : InstallerBase
    {
        public LinuxInstaller(Logger logger) : base(logger, OS.Linux) { }

        public override bool CheckPermission()
        {
            return Mono.Unix.Native.Syscall.getuid() == 0;
        }
    }
}
