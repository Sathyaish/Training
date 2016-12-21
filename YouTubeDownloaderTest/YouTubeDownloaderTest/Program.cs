using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace YouTubeDownloaderTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var playlistId = "PLHfwoPeLRqw4FcaaSaPY1oCVzdyP-q19V";
            var apiKey = "AIzaSyCgY74Kh2fE2S9PmMY68llyTihKfPbpQdk";

            Download(playlistId, apiKey);

            Console.ReadKey();
        }

        static async void Download(string playlistId, string apiKey)
        {
            try
            {
                var playListNameTask = GetPlayListNameAsync(playlistId, apiKey);

                var url = string.Format($"https://www.googleapis.com/youtube/v3/playlistItems?part=contentDetails,id,snippet,status&playlistId={playlistId}&maxResults=50&key={apiKey}");

                dynamic result = await GetResult(url);

                var playlistName = await playListNameTask;

                Console.WriteLine($"Playlist: {playlistName}");
                Console.WriteLine($"{result?.pageInfo?.totalResults} videos in playlist.");
                Console.WriteLine("-----------------------------------------------------\n");

                var i = 0;
                foreach (var video in result.items)
                    Console.WriteLine($"{++i}. {video.snippet.title}");
            }
            catch(Exception ex)
            {
                Debugger.Break();
                Console.WriteLine(ex.Message);
            }
        }

        static async Task<string> GetPlayListNameAsync(string playlistId, string apiKey)
        {
            string playlistName = null;

            try
            {
                var url = string.Format($"https://www.googleapis.com/youtube/v3/playlists?part=snippet,id&id={playlistId}&key={apiKey}");

                dynamic result = await GetResult(url);

                playlistName = result?.items?[0]?.snippet.title;
            }
            catch (Exception ex)
            {
                Debugger.Break();
                Console.WriteLine(ex.Message);
            }

            return playlistName;
        }

        static async Task<dynamic> GetResult(string url)
        {
            dynamic result = null;

            try
            {
                var client = new HttpClient();
                
                var json = await client.GetStringAsync(url);

                result = JsonConvert.DeserializeObject(json);
            }
            catch(Exception ex)
            {
                Debugger.Break();
                Console.WriteLine(ex.Message);
            }

            return result;
        }
    }
}