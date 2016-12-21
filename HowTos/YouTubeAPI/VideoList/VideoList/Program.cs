using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace VideoList
{
    class Program
    {
        // VideoList.exe PlayListId
        static void Main(string[] args)
        {
            try
            {
                var len = args?.Length;

                if (len == null || len.Value == 0)
                {
                    PrintHelp();
                    return;
                }

                var playListId = args[0];

                var result = GetVideosInPlayListAsync(playListId).Result;

                PrintResult(result);
            }
            catch(AggregateException agg)
            {
                foreach (var e in agg.Flatten().InnerExceptions)
                    Console.WriteLine(e.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }

        private static async Task<dynamic> GetVideosInPlayListAsync(string playListId)
        {
            var parameters = new Dictionary<string, string>
            {
                ["key"] = ConfigurationManager.AppSettings["APIKey"],
                ["playlistId"] = playListId,
                ["part"] = "snippet",
                ["fields"] = "pageInfo, items/snippet(title, description)",
                ["maxResults"] = "50"
            };

            var baseUrl = "https://www.googleapis.com/youtube/v3/playlistItems?";
            var fullUrl = MakeUrlWithQuery(baseUrl, parameters);

            var result = await new HttpClient().GetStringAsync(fullUrl);

            if (result != null)
            {
                return JsonConvert.DeserializeObject(result);
            }

            return default(dynamic);
        }

        private static string MakeUrlWithQuery(string baseUrl, 
            IEnumerable<KeyValuePair<string, string>> parameters)
        {
            if (string.IsNullOrEmpty(baseUrl))
                throw new ArgumentNullException(nameof(baseUrl));

            if (parameters == null || parameters.Count() == 0)
                return baseUrl;

            return parameters.Aggregate(baseUrl,
                (accumulated, kvp) => string.Format($"{accumulated}{kvp.Key}={kvp.Value}&"));
        }

        private static void PrintResult(dynamic result)
        {
            var count = result.items.Count;
            Console.WriteLine($"Total items in playlist: {result.pageInfo.totalResults, 2}");
            Console.WriteLine($"Public items in playlist: {count, 2}");
            Console.WriteLine("-----------------------------------------------------");

            var i = 0;

            if (count > 0)
                foreach (var item in result.items)
                    Console.WriteLine(string.Format($"{++i,3}) {item.snippet.title}"));
        }

        private static void PrintHelp()
        {
            Console.WriteLine("This program lists all video names in the specified YouTube playlist Id.");
            Console.WriteLine("USAGE: VideoList.exe {PlayListId}");
        }
    }
}
