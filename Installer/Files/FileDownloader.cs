using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Installer.Files
{
    public static class FileDownloader
    {
        public static event EventHandler<string> DownloadStartEvent;
        public static event EventHandler<string> DownloadFinishedEvent;
        public static event EventHandler<(string file, Exception exception)> DownloadFailedEvent;

        public static bool DownloadFile(string url, string path)
        {
            try
            {
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                double fileSizeMb = GetFileSize(url) / 1_000_000D;
                DownloadStartEvent?.Invoke(null, $"{path}... ({fileSizeMb:0.00}MB)");
                using (var client = new WebClient())
                    client.DownloadFile(url, path);
                DownloadFinishedEvent?.Invoke(null, path);
                return true;
            }
            catch (Exception ex)
            {
                DownloadFailedEvent?.Invoke(null, (path, ex));
                return false;
            }
        }

        private static long GetFileSize(string url)
        {
            var httpRequest = WebRequest.CreateHttp(url);
            using (var response = httpRequest.GetResponse())
            {
                return response.ContentLength;
            }
        }
    }
}
