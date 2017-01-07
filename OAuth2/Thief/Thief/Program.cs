using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;

namespace Thief
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var baseUrl = "https://graph.facebook.com/me?";
                var parameters = new Dictionary<string, string>
                {
                    ["access_token"] = "EAAXgcrFzbNIBAHZCc7tvh1zr1ZAirlZAKYnKsbQ0uaw0hmO1xtPPH6RdkLBubejWKTHGZC67e4RXBH0mvTZCrHzOWSH38wlY6WROZASpMXKlWbMcXbVZByZBmFa1u8g4bODh3g5FdHZBktL6fgdJt0IbxxJZBoUldSJi9gZCBZBKqfQvJwZDZD",
                    ["fields"] = "name,first_name,last_name,email"
                };

                var url = MakeUrlWithQuery(baseUrl, parameters);

                var result = new HttpClient().GetStringAsync(url).Result;

                Console.WriteLine(result);
            }
            catch(AggregateException agg)
            {
                foreach(var e in agg.Flatten().InnerExceptions)
                    Console.WriteLine(e.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
    }
}
