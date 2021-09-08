using BotInstaller.Env;
using BotInstaller.Files;
using BotInstaller.SysArchitecture;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace BotInstaller.Installer.Specific
{
    public class WindowsInstaller : InstallerBase
    {
        public WindowsInstaller(Logger logger) : base(logger, OS.Windows) { }

        public override bool CheckPermission()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
    }
}
