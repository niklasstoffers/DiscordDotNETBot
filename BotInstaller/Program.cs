using BotInstaller.Files;
using BotInstaller.Installer;
using BotInstaller.SysArchitecture;
using System;

namespace BotInstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            OSVersion version = OSVersionResolver.ResolveOS();
            Installer.Installer installer = InstallerBuilder.GetInstallerFor(version);

            if (installer == null)
            {
                Interactor.MessageAndWaitForExit("Unsupported operating system!");
                return;
            }

            using (installer)
            {
                if (!installer.CheckPermission())
                {
                    Interactor.MessageAndWaitForExit("Installer doesn't have the access permissions to install the application! Ensure that the installer is run as admin!");
                    return;
                }

                string installationPath = Interactor.GetNonEmpty("Enter your installation path:");
                if (!installer.SetInstallationPath(installationPath))
                {
                    Interactor.MessageAndWaitForExit("Error setting the installation path. Check path validity and run again!");
                    return;
                }

                FileDownloader.DownloadStartEvent += FileDownloader_DownloadStartEvent;
                FileDownloader.DownloadFinishedEvent += FileDownloader_DownloadFinishedEvent;
                FileDownloader.DownloadFailedEvent += FileDownloader_DownloadFailedEvent;

                Interactor.Message("Running installer. This may take a couple of minutes! Do not exit!");
                InstallerResult result = installer.Install(version.Architecture);
                string message = result switch
                {
                    InstallerResult.Success => "Installation successfull!",
                    InstallerResult.Failed => "An error occurred during installation. Check log files for more information!",
                    InstallerResult.FailedNoLog => "An error occurred during installation. Couldn't write log file!",
                    _ => "Unknown error occurred during installation"
                };

                Interactor.MessageAndWaitForExit(message);
            }
        }

        private static void FileDownloader_DownloadFailedEvent(object sender, (string file, Exception exception) e)
        {
            Interactor.Message($"File download has failed for {e}");
        }

        private static void FileDownloader_DownloadFinishedEvent(object sender, string e)
        {
            Interactor.Message($"File download has finished for {e}");
        }

        private static void FileDownloader_DownloadStartEvent(object sender, string e)
        {
            Interactor.Message($"Starting file download of {e}");
        }
    }
}
