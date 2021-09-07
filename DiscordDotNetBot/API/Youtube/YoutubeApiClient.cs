using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DiscordDotNetBot.API.Youtube
{
    public class YoutubeApiClient
    {
        private static readonly Dictionary<YTEndpoint, Endpoint> _endpoints = new Dictionary<YTEndpoint, Endpoint>()
        {
            { YTEndpoint.SearchList, new Endpoint("https://www.googleapis.com/youtube/v3/search", HttpMethod.Get.ToString()) },
            { YTEndpoint.VideoUrl, new Endpoint("https://www.youtube.com/watch", HttpMethod.Get.ToString()) }
        };

        private string _apiKey;

        public YoutubeApiClient(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<string> GetMostRelevantForSearch(string search)
        {
            try
            {
                dynamic response = await MakeRequest(_endpoints[YTEndpoint.SearchList],
                                                 new QueryParameter[]
                                                 {
                                                     new QueryParameter("q", search),
                                                     new QueryParameter("part", "snippet"),
                                                     new QueryParameter("order", "relevance")
                                                 });

                if (response.items.Count == 0)
                    return null;

                return $"{_endpoints[YTEndpoint.VideoUrl].Url}?v={response.items[0].id.videoId}";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Youtube API request failed!");
                return null;
            }
        }

        private async Task<object> MakeRequest(Endpoint endpoint, QueryParameter[] parameters)
        {
            endpoint.Url += $"?key={HttpUtility.UrlEncode(_apiKey)}&";
            foreach (var param in parameters)
                endpoint.Url += $"{HttpUtility.UrlEncode(param.Name)}={HttpUtility.UrlEncode(param.Value)}&";
            endpoint.Url = endpoint.Url.TrimEnd('&');

            var httpRequest = WebRequest.CreateHttp(endpoint.Url);
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
