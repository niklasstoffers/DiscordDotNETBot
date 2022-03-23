using Discord;
using Discord.Audio;
using Discord.WebSocket;
using Hainz.Framework;
using Hainz.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.Audio
{
    public class VoiceChannelService : DisposableBase
    {
        private DiscordSocketClient _client;
        private Logger _logger;

        public bool IsConnected => AudioClient?.ConnectionState == ConnectionState.Connected;
        public IVoiceChannel Current { get; private set; }
        public IAudioClient AudioClient { get; private set; }

        public event EventHandler Disconnected;

        public VoiceChannelService(DiscordSocketClient client,
                                   Logger logger)
        {
            _client = client;
            _logger = logger;
        }

        private void InitNewClient()
        {
            AudioClient.Disconnected += async ex =>
            {
                if (ex != null)
                {
                    await _logger.LogMessageAsync("Disconnect from VC due to exception", LogLevel.Error);
                    await _logger.LogExceptionAsync(ex);
                }

                await DisconnectAsync();
                Disconnected?.Invoke(this, new EventArgs());
            };
        }

        public async Task<IAudioClient> ConnectAsync(IVoiceChannel voiceChannel)
        {
            if (IsConnected)
            {
                if (Current.Id == voiceChannel.Id)
                    return AudioClient;

                await DisconnectAsync();
            }

            try
            {
                AudioClient = await voiceChannel.ConnectAsync();
                Current = voiceChannel;
                InitNewClient();
            }
            catch (AggregateException ex)
            {
                await _logger.LogMessageAsync("Exception while connecting to VC", LogLevel.Error);
                await _logger.LogExceptionAsync(ex);
                return null;
            }
            
            return AudioClient;
        }

        public async Task DisconnectAsync()
        {
            if (IsConnected)
            {
                try
                {
                    await (Current?.DisconnectAsync() ?? Task.CompletedTask);
                    await (AudioClient?.StopAsync() ?? Task.CompletedTask);
                    AudioClient?.Dispose();
                    Current = null;
                }
                catch (AggregateException ex)
                {
                    await _logger.LogMessageAsync("Exception while disconnecting from VC", LogLevel.Error);
                    await _logger.LogExceptionAsync(ex);
                }

                AudioClient = null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!Disposed && disposing)
            {
                DisconnectAsync().GetAwaiter().GetResult();
            }
            base.Dispose(disposing);
        }
    }
}
