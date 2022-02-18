using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace NETCore.Basic.Services.External
{
    public interface IHttpConsumer
    {
        void AddCookies(List<Cookie> cookies);
        void AddHeaders(KeyValuePair<string, string> header);
        void AddHeaders(List<KeyValuePair<string, string>> headers);
        T Get<T>(string url);
        T Get<T>(string url, List<KeyValuePair<string, object>> param = null);
        Task<T> GetAsync<T>(string url);
        T Post<T>(string url, List<KeyValuePair<string, object>> param);
        T Post<T>(string url, object param);
        Task<T> PostAsync<T>(string url, List<KeyValuePair<string, object>> param);
        Task<T> PostAsync<T>(string url, object param);
        T Put<T>(string url, List<KeyValuePair<string, object>> param);
        T Put<T>(string url, object param);
    }
    public class HttpConsumer : IHttpConsumer
    {
        private readonly RestClient _request;
        public HttpConsumer()
        {
            _request = new RestClient();
        }
        public HttpConsumer(string baseUrl)
        {
            _request = new RestClient(baseUrl);
        }
        public T Get<T>(string url)
        {
            var request = new RestRequest(url, Method.GET, DataFormat.Json);

            var response = _request.Get<T>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new Exception($"Houve um erro {response.StatusCode} ao tentar conectar ao endereço {response.ResponseUri}");
        }

        public T Post<T>(string url, object param)
        {
            var request = new RestRequest(url, Method.POST, DataFormat.Json);
            request.AddJsonBody(param);

            var response = _request.Post<T>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new Exception($"Houve um erro {response.StatusCode} ao tentar conectar ao endereço {response.ResponseUri}");
        }

        public T Post<T>(string url, List<KeyValuePair<string, object>> param)
        {
            var request = new RestRequest(url, Method.POST, DataFormat.Json);
            //request.AddJsonBody(param);
            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = _request.Post<T>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new Exception($"Houve um erro {response.StatusCode} ao tentar conectar ao endereço {response.ResponseUri}");
        }

        public T Put<T>(string url, object param)
        {
            var request = new RestRequest(url, Method.POST, DataFormat.Json);
            request.AddJsonBody(param);

            var response = _request.Put<T>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new Exception($"Houve um erro {response.StatusCode} ao tentar conectar ao endereço {response.ResponseUri}");
        }

        public T Put<T>(string url, List<KeyValuePair<string, object>> param)
        {
            var request = new RestRequest(url, Method.POST, DataFormat.Json);
            //request.AddJsonBody(param);
            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = _request.Put<T>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new Exception($"Houve um erro {response.StatusCode} ao tentar conectar ao endereço {response.ResponseUri}");
        }


        public T Get<T>(string url, List<KeyValuePair<string, object>> param = null)
        {
            var request = new RestRequest(url, Method.GET, DataFormat.Json);

            if (param != null)
            {
                param.ForEach(p => request.AddParameter(p.Key, p.Value, ParameterType.GetOrPost));
            }

            var response = _request.Get<T>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new Exception($"Houve um erro {response.StatusCode} ao tentar conectar ao endereço {response.ResponseUri}");
        }


        public async Task<T> GetAsync<T>(string url)
        {
            var request = new RestRequest(url, Method.GET, DataFormat.Json);

            var response = await _request.ExecuteGetAsync<T>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new Exception($"Houve um erro {response.StatusCode} ao tentar conectar ao endereço {response.ResponseUri}");
        }
        public async Task<T> PostAsync<T>(string url, object param)
        {
            var request = new RestRequest(url, Method.POST, DataFormat.Json);
            request.AddJsonBody(param);

            var response = await _request.ExecutePostAsync<T>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new Exception($"Houve um erro {response.StatusCode} ao tentar conectar ao endereço {response.ResponseUri}");
        }
        public async Task<T> PostAsync<T>(string url, List<KeyValuePair<string, object>> param)
        {
            var request = new RestRequest(url, Method.POST, DataFormat.Json);

            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = await _request.ExecutePostAsync<T>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new Exception($"Houve um erro {response.StatusCode} ao tentar conectar ao endereço {response.ResponseUri}");
        }

        public void AddCookies(List<Cookie> cookies)
        {
            this._request.CookieContainer = new CookieContainer();

            cookies.ForEach(cookie => this._request.CookieContainer.Add(cookie));
        }

        public void AddHeaders(List<KeyValuePair<string, string>> headers)
        {
            headers.ForEach(header => this._request.AddDefaultHeader(header.Key, header.Value));
        }
        public void AddHeaders(KeyValuePair<string, string> header)
        {
            this._request.AddDefaultHeader(header.Key, header.Value);
        }
    }
}
