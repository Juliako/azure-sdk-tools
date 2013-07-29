using System;
using System.Net.Http;
using Microsoft.WindowsAzure.Management.Utilities.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.WindowsAzure.Management.Utilities.MediaService.Services
{
    public static class HttpClientExtensions
    {
        public static HttpResponseMessage PostAsJsonAsync(
           this HttpClient client,
           string requestUri,
           JObject json,
           Action<string> Logger)
        {
            Microsoft.WindowsAzure.Management.Utilities.Common.HttpClientExtensions.AddUserAgent(client);

            Microsoft.WindowsAzure.Management.Utilities.Common.HttpClientExtensions.LogRequest(
                HttpMethod.Post.Method,
                client.BaseAddress + requestUri,
                client.DefaultRequestHeaders,
                JsonConvert.SerializeObject(json, Formatting.Indented),
                Logger);
            HttpResponseMessage response = client.PostAsJsonAsync(requestUri, json).Result;
            string content = response.EnsureSuccessStatusCode().Content.ReadAsStringAsync().Result;
            Microsoft.WindowsAzure.Management.Utilities.Common.LogResponse(
                response.StatusCode.ToString(),
                response.Headers,
                General.TryFormatJson(content),
                Logger);

            return response;
        }
    }
}