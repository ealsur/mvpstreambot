using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace mvpstreambot.Services
{
    public static class LUISService
    {
        static readonly string AppId;
        static readonly string SubscriptionKey;
        static LUISService()
        {
            AppId = ConfigurationManager.AppSettings["LUISAppId"];
            SubscriptionKey = ConfigurationManager.AppSettings["LUISSubscriptionKey"];
        }
        public static async Task<LUISResponse> GetIntent(string query)
        {
            if (string.IsNullOrEmpty(query) || query.StartsWith("hi") || query.StartsWith("hello") || query.StartsWith("hola"))
            {
                return new LUISResponse() { Intents = new List<LUISIntent>() { new LUISIntent() { Intent = "Greetings" } } };
            }
            using (var client = new HttpClient())
            {
                return JsonConvert.DeserializeObject<LUISResponse>(await client.GetStringAsync($"https://api.projectoxford.ai/luis/v1/application?id={AppId}&subscription-key={SubscriptionKey}&q={query}"));
            }
        }
    }

    public class LUISResponse
    {
        public List<LUISIntent> Intents { get; set; }
        public List<LUISEntity> Entities { get; set; }
    }

    public class LUISEntity
    {
        public string Type { get; set; }
        public string Entity { get; set; }
    }

    public class LUISIntent
    {
        public string Intent { get; set; }
        public double Score { get; set; }
    }
}