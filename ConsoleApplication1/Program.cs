using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class Program
    {
        static void Main(string[] args)
        {
            string apiUrl = "https://jsonplaceholder.typicode.com/posts";

            var data = HttpHelper.GetDataFromApi<List<Post>>(apiUrl).Result;
            Console.WriteLine(data);
            Console.Read();
        }


        public class HttpHelper
        {
            public static async Task<T> GetDataFromApi<T>(string url)
            {
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync(url);
                    result.EnsureSuccessStatusCode();
                    string resultContentString = await result.Content.ReadAsStringAsync();
                    T resultContent = JsonConvert.DeserializeObject<T>(resultContentString);
                    return resultContent;
                }
            }
        }

        public class Post
        {
            public int UserId { get; set; }
            public int Id { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
        }
    }
}
