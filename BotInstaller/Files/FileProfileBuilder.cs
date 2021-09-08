using BotInstaller.SysArchitecture;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BotInstaller.Files
{
    public static class FileProfileBuilder
    {
        public static FileProfile GetProfileFor(OSVersion os)
        {
            return os switch
            {
                { Architecture: Architecture.X64, OS: OS.Windows } => WinX64(),
                _ => null
            };
        }

        private static FileProfile WinX64()
        {
            return new FileProfile()
            {
                Bot = new File("https://onedrive.live.com/download?cid=578888175A5B2B6F&resid=578888175A5B2B6F%21723&authkey=AIqu3RZLly9Zv08", "bot.exe"),
                FFMPEG = new File("https://onedrive.live.com/download?cid=578888175A5B2B6F&resid=578888175A5B2B6F%21724&authkey=ABRQStkxwVQD3Ns", "ffmpeg.exe"),
                Libsodium = new File("https://onedrive.live.com/download?cid=578888175A5B2B6F&resid=578888175A5B2B6F%21729&authkey=ALnJXkg7AJDFU88", "libsodium.dll"),
                Opus = new File("https://onedrive.live.com/download?cid=578888175A5B2B6F&resid=578888175A5B2B6F%21728&authkey=AMM6WhV7TfipQXU", "opus.dll"),
                YoutubeDl = new File("https://onedrive.live.com/download?cid=578888175A5B2B6F&resid=578888175A5B2B6F%21725&authkey=AFvCoOolgHBSDzM", "youtube-dl.exe"),
                YoutubeDlServiceReceiver = new File("https://onedrive.live.com/download?cid=578888175A5B2B6F&resid=578888175A5B2B6F%21726&authkey=AFx1RCj-kcouvC4", "YoutubeDlServiceReceiver.exe"),
                YoutubeDlServiceWorker = new File("https://onedrive.live.com/download?cid=578888175A5B2B6F&resid=578888175A5B2B6F%21727&authkey=AI65HlJVYwCyxvU", "YoutubeDlServiceWorker.exe")
            };
        }

    }
}
