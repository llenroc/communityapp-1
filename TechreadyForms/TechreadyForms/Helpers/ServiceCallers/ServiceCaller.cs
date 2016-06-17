using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TechReady.Helpers.ServiceCallers
{
    public class ServiceCallerResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class ServiceCaller
    {
        public async Task<ServiceCallerResponse> CallService(string urlString, string inputData)
        {
            // var urlString = string.Format("{0}{1}", Utilities.SyncContactsPath, "");
            Uri restUri = new Uri(urlString, UriKind.RelativeOrAbsolute);

            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, restUri.ToString());

            request.Headers.Add("Accept", "application/json");
           
            request.Content = new StringContent(inputData, Encoding.UTF8, "application/json");


            var result = await client.SendAsync(request);

            ServiceCallerResponse sr = new ServiceCallerResponse();

            bool success = false;
            if (result.IsSuccessStatusCode)
            {
                string responseString = await result.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(responseString))
                {
                    sr.Message = responseString;
                    sr.IsSuccess = true;
                }
            }
            else
            {
                sr.Message = result.ReasonPhrase;
                sr.IsSuccess = false;
            }
            return sr;
        }
    }
}
