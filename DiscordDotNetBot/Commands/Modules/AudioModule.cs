using Discord;
using Discord.Audio;
using Discord.Commands;
using DiscordDotNetBot.API.Youtube;
using DiscordDotNetBot.Config;
using DiscordDotNetBot.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DiscordDotNetBot.Commands.Modules
{
    public class AudioModule : ModuleBase<SocketCommandContext>
    {
        private static IAudioClient _client;
        private static IVoiceChannel _vc;
        private static Stream _input;
        private static AudioOutStream _output;

        private static YoutubeApiClient _ytClient;

        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayAsync(string search)
        {
            IVoiceChannel vc = (Context.User as IGuildUser)?.VoiceChannel;

            if (vc == null)
                return;

            if (_vc == null || _vc.Id != vc.Id)
            {
                await DCAsync();

                _vc = vc;
                _client = await vc.ConnectAsync();
            }

            _ytClient ??= new YoutubeApiClient(BotConfig.Current.YoutubeAPIKey);
            string videoUrl = search;
            if (!Uri.TryCreate(search, UriKind.Absolute, out _))
                videoUrl = await _ytClient.GetMostRelevantForSearch(search);

            if (videoUrl == null)
            {
                await Context.Channel.SendMessageAsync("Fehler beim Aufrufen der YT Api.");
            }
            else
            {
                try
                {
                    var audioFormats = await YoutubeDl.GetFormats(videoUrl);
                    if (audioFormats == null)
                    {
                        await Context.Channel.SendMessageAsync("Fehler beim Extrahieren der Audio Daten.");
                    }
                    else
                    {
                        foreach (var audioFormat in audioFormats)
                        {
                            if (audioFormat.FormatCode == 251)
                            {
                                string url = await YoutubeDl.GetUrl(audioFormat.FormatCode, videoUrl, true);
                                Console.WriteLine("Got url");
                                if (!string.IsNullOrEmpty(url))
                                {
                                    var httpRequest = WebRequest.CreateHttp(url);
                                    httpRequest.Method = HttpMethod.Get.ToString();

                                    using (var response = await httpRequest.GetResponseAsync())
                                    {
                                        using (_input = response.GetResponseStream())
                                        {
                                            using (_output = _client.CreatePCMStream(AudioApplication.Mixed))
                                            {
                                                using (var ffmpeg = Process.Start(new ProcessStartInfo()
                                                {
                                                    FileName = "ffmpeg",
                                                    Arguments = "-hide_banner -loglevel quiet -f webm -i - -c:a pcm_s16le -f s16le pipe:1",
                                                    RedirectStandardInput = true,
                                                    RedirectStandardOutput = true,
                                                    UseShellExecute = false,
                                                    CreateNoWindow = true
                                                }))
                                                {
                                                    bool isWriting = true;
                                                    var inputTask = Task.Run(async () =>
                                                    {
                                                        long contentLength = response.ContentLength;
                                                        byte[] buffer = new byte[8192];
                                                        while(contentLength > 0)
                                                        {
                                                            int read = await _input.ReadAsync(buffer, 0, buffer.Length);
                                                            await ffmpeg.StandardInput.BaseStream.WriteAsync(buffer, 0, (int)Math.Min(contentLength, read));
                                                            contentLength -= read;
                                                        }
                                                        isWriting = false;
                                                    });

                                                    var outputTask = Task.Run(async () =>
                                                    {
                                                        byte[] buffer = new byte[8192];
                                                        int ffmpegCount = 0;
                                                        while(isWriting)
                                                        {
                                                            ffmpegCount = await ffmpeg.StandardOutput.BaseStream.ReadAsync(buffer, 0, buffer.Length);
                                                            await _output.WriteAsync(buffer, 0, ffmpegCount);
                                                        }
                                                    });

                                                    await Task.WhenAll(inputTask, outputTask);
                                                    await ffmpeg.StandardInput.BaseStream.DisposeAsync();
                                                    await ffmpeg.StandardOutput.BaseStream.DisposeAsync();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    await Context.Channel.SendMessageAsync("Ungültige YT Url oder nicht unterstütztes Format.");
                }
            }
        }

        [Command("dc", RunMode = RunMode.Async)]
        public async Task DCAsync()
        {
            try
            {
                if (_client != null) await _client.StopAsync();
                if (_vc != null) await _vc.DisconnectAsync();
            }
            finally
            {
                _client?.Dispose();
                
                if (_input != null) await _input.DisposeAsync();
                if (_output != null) await _output.DisposeAsync();

                _client = null;
                _vc = null;
                _input = null;
                _output = null;
            }
        }
    }
}
