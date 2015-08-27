using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.ComponentModel;
using System.Net;
using WebDB.Common;

namespace WebDB.Client.Core
{
    public class WebDBClient<TModel> where TModel : class
    {
        private string requestUri = $"http://localhost:1987/api/General?entityType={typeof(TModel).Name}";

        public async Task<List<TModel>> Get() 
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage responseMessage = await client.GetAsync(requestUri);
            string json = await responseMessage.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<TModel>>(json);
            return result;
        }

        public async Task<TModel> Put(TModel item)
        {
            return await SendObjectGetResult(item, "PUT");
        }
        public async Task<TModel> Post(TModel item)
        {
            return await SendObjectGetResult(item, "POST");
        }

        private async Task<TModel> SendObjectGetResult(TModel item, string method)
        {
            string json = JsonConvert.SerializeObject(item);
            Uri uri = new Uri(requestUri);

            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            string response = await client.UploadStringTaskAsync(requestUri, method, json);

            var result = JsonConvert.DeserializeObject<TModel>(response);
            return result;
        }

        public async Task<TModel> Undo(TModel item)
        {
            string json = JsonConvert.SerializeObject(item);
            Uri uri = new Uri(requestUri);

            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            string response = await client.UploadStringTaskAsync(requestUri + "&id=" + item.GetId(), "PATCH", json);

            var result = JsonConvert.DeserializeObject<TModel>(response);
            return result;
        }
    }
}
