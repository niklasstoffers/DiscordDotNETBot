using BotInstaller.Env;
using BotInstaller.Files;
using BotInstaller.SysArchitecture;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BotInstaller.Installer.Specific
{
    public abstract class InstallerBase : Installer
    {
        private string _installPath;
        private FileProfile _fileProfile;
        private OS _os;

        public InstallerBase(Logger logger, OS os) : base(logger) 
        {
            _os = os;
        }

        public abstract override bool CheckPermission();

        public override InstallerResult Install(Architecture architecture)
        {
            OSVersion os = new OSVersion() { OS = _os, Architecture = architecture };
            _fileProfile = FileProfileBuilder.GetProfileFor(os);

            if (_fileProfile == null)
            {
                Logger.LogMessage("Couldn't retrieve file profile for current os version");
                return InstallerResult.Failed;
            }

            if (!TryCreateInstallationFolder(_installPath, out _installPath))
                return InstallerResult.Failed;

            if (!DownloadFiles())
            {
                Cleanup();
                return InstallerResult.Failed;
            }

            if (!TryAddNecessaryEnvVariables())
                return InstallerResult.Failed;
            return InstallerResult.Success;
        }

        private bool TryCreateInstallationFolder(string path, out string folderPath)
        {
            try
            {
                folderPath = Path.Combine(path, InstallFolderName);
                Directory.CreateDirectory(folderPath);
                return true;
            }
            catch (Exception ex)
            {
                folderPath = string.Empty;
                Logger.LogMessage("Error trying to create installation folder.");
                Logger.LogException(ex);
                return false;
            }
        }

        private bool DownloadFiles()
        {
            if (!FileDownloader.DownloadFile(_fileProfile.Bot.Url, Path.Combine(_installPath, _fileProfile.Bot.Name))) return DownloadError(_fileProfile.Bot.Name);
            if (!FileDownloader.DownloadFile(_fileProfile.FFMPEG.Url, Path.Combine(_installPath, _fileProfile.FFMPEG.Name))) return DownloadError(_fileProfile.FFMPEG.Name);
            if (!FileDownloader.DownloadFile(_fileProfile.Libsodium.Url, Path.Combine(_installPath, _fileProfile.Libsodium.Name))) return DownloadError(_fileProfile.Libsodium.Name);
            if (!FileDownloader.DownloadFile(_fileProfile.Opus.Url, Path.Combine(_installPath, _fileProfile.Opus.Name))) return DownloadError(_fileProfile.Opus.Name);
            if (!FileDownloader.DownloadFile(_fileProfile.YoutubeDlServiceReceiver.Url, Path.Combine(_installPath, _fileProfile.YoutubeDlServiceReceiver.Name))) return DownloadError(_fileProfile.YoutubeDlServiceReceiver.Name);
            if (!FileDownloader.DownloadFile(_fileProfile.YoutubeDlServiceWorker.Url, Path.Combine(_installPath, _fileProfile.YoutubeDlServiceWorker.Name))) return DownloadError(_fileProfile.YoutubeDlServiceWorker.Name);
            if (!FileDownloader.DownloadFile(_fileProfile.YoutubeDl.Url, Path.Combine(_installPath, _fileProfile.YoutubeDl.Name))) return DownloadError(_fileProfile.YoutubeDl.Name);
            return true;
        }

        private bool DownloadError(string filename)
        {
            Logger.LogMessage($"Failed to download the file: {filename}");
            return false;
        }

        private bool TryAddNecessaryEnvVariables()
        {
            try
            {
                string pathVarName = EnvVarName.Get(EnvironmentVariable.Path, OS.Windows);
                string currentValue = Environment.GetEnvironmentVariable(pathVarName, EnvironmentVariableTarget.User);
                if (!currentValue.Contains(_installPath))
                    currentValue += $";{_installPath}";
                Environment.SetEnvironmentVariable(pathVarName, currentValue, EnvironmentVariableTarget.User);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogMessage("Error trying to register necessary environment variables.");
                Logger.LogException(ex);
                return false;
            }
        }

        private void Cleanup()
        {
            try
            {
                Directory.Delete(_installPath, true);
            }
            catch { }
        }

        public override bool SetInstallationPath(string path)
        {
            _installPath = path;
            return Directory.Exists(_installPath);
        }

        public override void Dispose()
        {
            Logger.Dispose();
        }
    }
}
