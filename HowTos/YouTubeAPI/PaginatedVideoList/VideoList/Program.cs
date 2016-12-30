using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace VideoList
{
    public class Program
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
                    Console.ReadKey();
                    return;
                }

                var playlistId = args[0];

                var theresMore = false;
                var escapeKeyPressed = false;
                string nextPageToken = null;
                var pageNumber = 0;
                const int MaxResults = 50;

                do
                {
                    var result = GetVideosInPlaylistAsync(playlistId, nextPageToken).Result;

                    if (result.pageInfo.totalResults == 0)
                    {
                        Console.WriteLine("No videos found in playlist.");
                        return;
                    }

                    pageNumber++;
                    nextPageToken = result.nextPageToken;
                    theresMore = (nextPageToken != null);
                    var from = (pageNumber - 1) * MaxResults + 1;
                    var to = from + result.items.Count - 1;

                    if (pageNumber == 1)
                        PrintReportHeader((int)result.pageInfo.totalResults, null);

                    PrintPageHeader(from, to);
                    PrintResult(result, from, to);
                    PrintPageFooter(theresMore);

                    if (theresMore)
                    {
                        escapeKeyPressed = (Console.ReadKey().Key == ConsoleKey.Escape);
                    }
                } while (!escapeKeyPressed && theresMore);
            }
            catch (AggregateException agg)
            {
                foreach (var e in agg.Flatten().InnerExceptions)
                    Console.WriteLine(e.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static async Task<dynamic> GetVideosInPlaylistAsync(string playlistId, string nextPageToken)
        {
            var parameters = new Dictionary<string, string>
            {
                ["key"] = ConfigurationManager.AppSettings["APIKey"],
                ["playlistId"] = playlistId,
                ["part"] = "snippet",
                ["fields"] = "nextPageToken, pageInfo, items/snippet(title)",
                ["maxResults"] = "50"
            };

            if (!string.IsNullOrEmpty(nextPageToken))
                parameters.Add("pageToken", nextPageToken);

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

        private static void PrintPageFooter(bool theresMore)
        {
            Console.WriteLine();

            if (theresMore)
            {
                Console.WriteLine("More...");
                Console.WriteLine("Press any key to see the remaining videos in this playlist.");
                Console.WriteLine("Press the 'Esc' key to exit the program.");
                Console.WriteLine(new String('-', 25));
                Console.WriteLine("\n");
            }
            else
            {
                Console.WriteLine("-- End of Results");
            }
        }

        private static void PrintResult(dynamic result, int from, int to)
        {
            var exceptionMessage = string.Format($"The value of the argument '{nameof(from)}' must be less than or the same as '{nameof(to)}'.");

            if (from > to) throw new ArgumentOutOfRangeException(exceptionMessage);

            var i = from;

            foreach (var item in result.items)
                Console.WriteLine(string.Format($"{from++,3}) {item.snippet.title}"));
        }

        private static void PrintPageHeader(int from, int to)
        {
            Console.WriteLine($"({from} - {to})");
            Console.WriteLine(new String('-', 20));
        }

        private static void PrintReportHeader(int totalItemsInPlaylist, int? publicItemsInPlaylist)
        {
            Console.WriteLine();
            Console.WriteLine($"Total items in playlist: {totalItemsInPlaylist, 2}");
            Console.WriteLine($"Public items in playlist: {publicItemsInPlaylist?.ToString() ?? "Unknown"}");
            Console.WriteLine("Only public items will be displayed.");
            Console.WriteLine();
        }

        private static void PrintHelp()
        {
            Console.WriteLine("This program lists all video names in the specified YouTube playlist Id.");
            Console.WriteLine("USAGE: VideoList.exe {PlayListId}");
        }
    }
}