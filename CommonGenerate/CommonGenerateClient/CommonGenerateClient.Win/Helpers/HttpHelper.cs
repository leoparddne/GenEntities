using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CommonGenerateClient.Win.Helpers
{
    public static class HttpHelper
    {
        public static async Task<TResult> GetAsync<TResult>(string url, Dictionary<string, object> parameter = null, Dictionary<string, string> Headers = null, int timeout = 10000)
        {
            return JsonConvert.DeserializeObject<TResult>(await GetAsync(url, parameter, Headers, timeout));
        }

        public static async Task<string> GetAsync(string url, Dictionary<string, object> parameter = null, Dictionary<string, string> Headers = null, int timeout = 10000)
        {
            GetInit(url, parameter, Headers, timeout, out var client, out var request);
            return await GetResult(client, request);
        }

        private static void GetInit(string url, Dictionary<string, object> parameter, Dictionary<string, string> Headers, int timeout, out RestClient client, out RestRequest request)
        {
            client = new RestClient();
            request = new RestRequest(url);
            request.Timeout = timeout;
            GenerateHeader(request, Headers);
            if (parameter == null)
            {
                return;
            }

            foreach (KeyValuePair<string, object> item in parameter)
            {
                Parameter parameter2 = Parameter.CreateParameter(item.Key, item.Value, ParameterType.GetOrPost);
                request.AddParameter(parameter2);
            }
        }

        public static async Task<TResult> PostAsync<TResult>(string url, object obj = null, Dictionary<string, string> Headers = null, int timeout = 10000)
        {
            return JsonConvert.DeserializeObject<TResult>(await PostAsync(url, obj, Headers, timeout));
        }

        public static async Task<string> PostAsync(string url, object obj = null, Dictionary<string, string> Headers = null, int timeout = 10000)
        {
            PostInit(url, obj, Headers, timeout, out var client, out var request);
            return await GetResult(client, request);
        }

        public static async Task<TResult> PostFormDataAsync<TResult>(string url, Dictionary<string, string> Headers = null, Dictionary<string, object> parameters = null, int timeout = 10000)
        {
            return JsonConvert.DeserializeObject<TResult>(await PostFormDataAsync(url, Headers, parameters, timeout));
        }

        public static async Task<string> PostFormDataAsync(string url, Dictionary<string, string> Headers = null, Dictionary<string, object> parameters = null, int timeout = 10000)
        {
            PostInit(url, null, Headers, timeout, out var client, out var request);
            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                request.AddParameter(parameter.Key, parameter.Value, ParameterType.GetOrPost);
            }

            return await GetResult(client, request);
        }

        private static async Task<string> GetResult(RestClient client, RestRequest request)
        {
            RestResponse restResponse = await client.ExecuteAsync(request);
            if (restResponse.IsSuccessful)
            {
                return restResponse.Content;
            }

            if (restResponse.ErrorException == null)
            {
                if (!string.IsNullOrWhiteSpace(restResponse.ErrorMessage))
                {
                    throw new Exception(restResponse.ErrorMessage);
                }

                throw new Exception("Get response error. " + request.Resource);
            }

            HttpRequestException ex = restResponse.ErrorException as HttpRequestException;
            if (ex != null)
            {
                throw new Exception($"{(int)(ex?.StatusCode).GetValueOrDefault()}:{ex?.Message ?? string.Empty}");
            }

            throw restResponse.ErrorException;
        }

        private static void PostInit(string url, object obj, Dictionary<string, string> Headers, int timeout, out RestClient client, out RestRequest request)
        {
            client = new RestClient();
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = null,
                DefaultValueHandling = DefaultValueHandling.Include,
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };
            client.UseNewtonsoftJson(settings);
            request = new RestRequest(url, Method.Post);
            request.Timeout = timeout;
            GenerateHeader(request, Headers);
            if (obj != null)
            {
                request.AddBody(obj);
            }
        }

        private static void GenerateHeader(RestRequest request, Dictionary<string, string> Headers)
        {
            if (Headers != null && Headers.Count > 0)
            {
                request.AddOrUpdateHeaders(Headers);
            }
        }
    }
}