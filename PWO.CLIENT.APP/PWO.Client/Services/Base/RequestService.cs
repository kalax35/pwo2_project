using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PWO.Client.Services.Base
{
    public class RequestService : IRequestService
    {
        private readonly HttpClient _httpClient;
        private readonly HttpContext _httpContext;
        private readonly JsonSerializerSettings _serializerSettings;

        private bool _clientInitialised = false;

        public RequestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpContext = HttpContext.Current;
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                NullValueHandling = NullValueHandling.Ignore
            };

            _serializerSettings.Converters.Add(new StringEnumConverter());
        }

        public async Task<TResult> GetAsync<TResult>(string uri, string token = "")
        {
            HttpClient httpClient = CreateHttpClient(token);


            HttpResponseMessage response = await httpClient.GetAsync(uri);

            await HandleResponse(response);

            string serialized = await response.Content.ReadAsStringAsync();
            var result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));
            return result;

        }

        public Task<TResult> PostAsync<TResult>(string uri, TResult data, string token = "")
        {
            return PostAsync<TResult, TResult>(uri, data, token);
        }

        public async Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data, string token = "")
        {
            HttpClient httpClient = CreateHttpClient(token);
            string serialized = await Task.Run(() => JsonConvert.SerializeObject(data, _serializerSettings));
            HttpResponseMessage response = await httpClient.PostAsync(uri, new StringContent(serialized, Encoding.UTF8, "application/json"));

            await HandleResponse(response);

            string responseData = await response.Content.ReadAsStringAsync();

            var result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(responseData, _serializerSettings));

            return result;
        }

        public Task<TResult> PutAsync<TResult>(string uri, TResult data, string token = "")
        {
            return PutAsync<TResult, TResult>(uri, data, token);
        }

        public async Task<TResult> PutAsync<TRequest, TResult>(string uri, TRequest data, string token = "")
        {
            HttpClient httpClient = CreateHttpClient(token);
            string serialized = await Task.Run(() => JsonConvert.SerializeObject(data, _serializerSettings));
            HttpResponseMessage response = await httpClient.PutAsync(uri, new StringContent(serialized, Encoding.UTF8, "application/json"));

            await HandleResponse(response);

            string responseData = await response.Content.ReadAsStringAsync();

            var result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(responseData, _serializerSettings));


            return result;
        }

        public async Task DeleteAsync(string uri, string token = "")
        {
            HttpClient httpClient = CreateHttpClient(token);
            HttpResponseMessage response = await httpClient.DeleteAsync(uri);
            await HandleResponse(response);
        }

        private HttpClient CreateHttpClient(string token = "")
        {
            if (_clientInitialised)
            {
                return _httpClient;
            }

            _clientInitialised = true;
            _httpClient.DefaultRequestHeaders.Clear();

            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var accessToken = _httpContext.Session["ApiAccessToken"] as string;

            if (string.IsNullOrEmpty(token))
            {
                token = _httpContext.Session["ApiSecurityToken"] as string;
            }

            _httpClient.DefaultRequestHeaders.Add("x-api-token", token);

            if (!string.IsNullOrEmpty(accessToken))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return _httpClient;
        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new HttpRequestException(content);
                }

                throw new HttpRequestException(content);
            }
        }

        public void ClearHttpConfig()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _clientInitialised = false;
        }
    }
}