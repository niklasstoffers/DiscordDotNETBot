using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Hainz.API.Youtube
{
    public class YoutubeApiClient
    {
        private static readonly Dictionary<YTEndpoint, Endpoint> _endpoints = new Dictionary<YTEndpoint, Endpoint>()
        {
            { YTEndpoint.SearchList, new Endpoint("https://www.googleapis.com/youtube/v3/search", HttpMethod.Get.ToString()) },
            { YTEndpoint.VideoDetails, new Endpoint("https://www.googleapis.com/youtube/v3/videos", HttpMethod.Get.ToString()) },
            { YTEndpoint.VideoUrl, new Endpoint("https://www.youtube.com/watch", HttpMethod.Get.ToString()) }
        };

        private string _apiKey;

        public YoutubeApiClient(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<YoutubeVideo> GetMostRelevantForSearch(string search)
        {
            try
            {
                dynamic response = await MakeRequest(_endpoints[YTEndpoint.SearchList],
                                                 new QueryParameter[]
                                                 {
                                                     new QueryParameter("q", search),
                                                     new QueryParameter("part", "snippet"),
                                                     new QueryParameter("order", "relevance"),
                                                     new QueryParameter("type", "video")
                                                 });

                if (response.items.Count == 0)
                    return null;

                for(int i = 0; i < response.items.Count; i++)
                {
                    if (response.items[i].snippet.liveBroadcastContent == "none")
                    {
                        dynamic item = response.items[i];
                        string videoUrl = $"{_endpoints[YTEndpoint.VideoUrl].Url}?v={item.id.videoId}";
                        return await GetVideoInfo(videoUrl);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Youtube API request failed!");
                return null;
            }
        }

        public async Task<YoutubeVideo> GetVideoInfo(string videoUrl)
        {
            try
            {
                dynamic response = await MakeRequest(_endpoints[YTEndpoint.VideoDetails],
                                                     new QueryParameter[]
                                                     {
                                                         new QueryParameter("part", "contentDetails,snippet"),
                                                         new QueryParameter("id", ExtractVideoId(videoUrl))
                                                     });

                dynamic item = response.items[0];

                string duration = item.contentDetails.duration;
                string channelName = item.snippet.channelTitle;
                string title = item.snippet.title;

                return new YoutubeVideo()
                {
                    Length = XmlConvert.ToTimeSpan(duration),
                    Url = videoUrl,
                    ChannelName = channelName,
                    Title = title
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Youtube API request failed!");
                return null;
            }
        }

        private string ExtractVideoId(string videoUrl)
        { 
            if (Uri.TryCreate(videoUrl, UriKind.Absolute, out Uri url))
            {
                string query = url.Query;
                var paramCollection = HttpUtility.ParseQueryString(query);
                string videoId = paramCollection.Get("v");
                return videoId;
            }
            return null;
        }

        private async Task<object> MakeRequest(Endpoint endpoint, QueryParameter[] parameters)
        {
            string url = $"{endpoint.Url}?key={HttpUtility.UrlEncode(_apiKey)}&";
            foreach (var param in parameters)
                url += $"{HttpUtility.UrlEncode(param.Name)}={HttpUtility.UrlEncode(param.Value)}&";
            url = url.TrimEnd('&');

            var httpRequest = WebRequest.CreateHttp(url);
            httpRequest.Method = endpoint.Method;

            string resultJson;
            using (var response = await httpRequest.GetResponseAsync())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var streamReader = new StreamReader(responseStream))
                    {
                        resultJson = streamReader.ReadToEnd();
                    }
                }
            }
            object result = JsonConvert.DeserializeObject(resultJson);
            return result;
        }
    }
}
