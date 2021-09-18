using Installer.Env;
using Installer.Files;
using Installer.SysArchitecture;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace Installer.Installer.Specific
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
