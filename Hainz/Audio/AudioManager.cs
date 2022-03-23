using Hainz.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.Audio
{
    public class AudioManager
    {
        private MusicService _currentMusicService;
        private object _lock = new object();
        private Logger _logger;

        public VoiceChannelService VCService { get; private set; }
        public MusicService MusicService 
        { 
            get 
            {
                if (_currentMusicService != null)
                    return _currentMusicService;

                lock (_lock)
                {
                    if (VCService.IsConnected)
                        _currentMusicService = new MusicService(VCService.AudioClient, _logger);
                    return _currentMusicService;
                }
            } 
        }

        public AudioManager(VoiceChannelService channelService,
                            Logger logger)
        {
            VCService = channelService;
            _logger = logger;

            VCService.Disconnected += (obj, e) =>
            {
                _currentMusicService?.Stop();
                _currentMusicService?.Dispose();
                _currentMusicService = null;
            };
        }
    }
}
