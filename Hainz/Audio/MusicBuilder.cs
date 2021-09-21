using Hainz.API.Youtube;
using Hainz.Log;
using Hainz.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.Audio
{
    public class MusicBuilder
    {
        private YoutubeApiClient _ytClient;
        private Logger _logger;
        private string _query;
        private MusicPlatform _platform;

        public MusicBuilder(YoutubeApiClient ytClient,
                            Logger logger)
        {
            _ytClient = ytClient;
            _logger = logger;
        }

        public MusicBuilder WithQuery(string query)
        {
            _query = query;
            return this;
        }

        public MusicBuilder WithPlatform(MusicPlatform platform)
        {
            _platform = platform;
            return this;
        }

        public async Task<Music> BuildAsync()
        {
            try
            {
                var builder = _platform switch
                {
                    MusicPlatform.Youtube => BuildYoutube(),
                    _ => throw new Exception($"Unsupported music platform {_platform}")
                };

                Music result = await builder;

                _query = null;
                _platform = 0;

                return result;
            }
            catch (Exception ex)
            {
                await _logger.LogMessageAsync($"Exception while building music with query {_query} on platform {_platform}", LogLevel.Error);
                await _logger.LogExceptionAsync(ex);
                return null;
            }
        }

        private async Task<Music> BuildYoutube()
        {
            YoutubeVideo video = null;
            if (Uri.TryCreate(_query, UriKind.Absolute, out _))
                video = await _ytClient.GetVideoInfo(_query);
            else
                video = await _ytClient.GetMostRelevantForSearch(_query);

            if (video == null)
                return null;

            string internalUrl = await YoutubeDl.GetBestOpusAudio(video.Url);
            if (string.IsNullOrEmpty(internalUrl))
                return null;

            return new Music(video.Title, video.Length, internalUrl, video.ChannelName);
        }
    }
}
