using Newtonsoft.Json; 
using System.Net.Http; 
using System.Threading.Tasks;

namespace invoiceIntegration.helper
{
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
}
