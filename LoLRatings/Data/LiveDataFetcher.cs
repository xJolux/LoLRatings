using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System;
using System.Threading.Tasks;


namespace LoLRatings.Data
{
    public static class LiveDataFetcher
    {
        private static readonly string _liveDataUrl = "https://127.0.0.1:2999/liveclientdata/allgamedata";

        private static readonly HttpClient _httpClient = new HttpClient(new HttpClientHandler { ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true })
        {
            Timeout = TimeSpan.FromSeconds(5)
        };

        public static async Task<JObject> GetLiveData()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(_liveDataUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var liveDataJson = JsonConvert.DeserializeObject<JObject>(responseBody);

                return liveDataJson;
            }
            catch
            {
                return null;
            }
        }
    }
}