using BotInstaller.SysArchitecture;
using System;
using System.Collections.Generic;
using System.Text;

namespace BotInstaller.Env
{
    public static class EnvVarName
    {
        public static string Get(EnvironmentVariable var, OS os)
        {
            return (var, os) switch
            {
                (EnvironmentVariable.Path, OS.Windows) => "PATH",
                (EnvironmentVariable.Path, OS.Linux) => "PATH",
                _ => null
            };
        }
    }
}
