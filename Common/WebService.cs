using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CoreHttpClient = System.Net.Http.HttpClient;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Common
{
    /*Stream | HttpClient*/
    public class WebService
    {
        private const string METHOD_NOT_CORRECT_EXCEPTION = "متدی که برای RestAPI استفاده شده است ، صحیح نمی باشد";
        
        /*-----------------------------------------------------------*/
        
        private readonly WebRequest _WebRequest;
        private Stream _Stream;
        private string _Status;

        /*-----------------------------------------------------------*/

        public WebService(string url)
        {
            _WebRequest = WebRequest.Create(url);
        }

        public WebService(string url, string method) : this(url)
        {
            if
            (
                method.Equals(Method.GET)    ||
                method.Equals(Method.POST)   ||
                method.Equals(Method.PUT)    ||
                method.Equals(Method.PATCH)  ||
                method.Equals(Method.DELETE)
            )
            {
                _WebRequest.Method = method;
            }
            else
            {
                throw new Exception(METHOD_NOT_CORRECT_EXCEPTION);
            }
        }
        
        public string Status
        {
            get => _Status;
            set => _Status = value;
        }

        public void SetHeaders(Dictionary<string, string> headers)
        {
            foreach (KeyValuePair<string, string> header in headers)
            {
                _WebRequest.Headers.Add(header.Key, header.Value);
            }
        }
        
        /* Url Encoded | FormData */
        /* "Item1=Value1&Item2=Value2&..." */
        public async Task SendRequestByUrlEncodedAsync(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            _WebRequest.ContentType   = "application/x-www-form-urlencoded";
            _WebRequest.ContentLength = bytes.Length;
            _Stream = await _WebRequest.GetRequestStreamAsync();
            await _Stream.WriteAsync(bytes, 0, bytes.Length);
            _Stream.Close();
        }
        
        /* JSON */
        /* new { Item1 = Value1 , Item2 = Value2 , ... } */
        public async Task SendRequestByJsonAsync(object data)
        {
            _WebRequest.ContentType = "application/json";
            Stream stream = await _WebRequest.GetRequestStreamAsync();
            StreamWriter streamWriter = new StreamWriter(stream);
            await streamWriter.WriteAsync(JsonConvert.SerializeObject(data));
            stream.Close();
            streamWriter.Close();
        }

        public async Task<string> ReceivedResponseAsync()
        {
            WebResponse response = await _WebRequest.GetResponseAsync();
            _Status = (response as HttpWebResponse).StatusDescription;
            _Stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(_Stream);
            string responseData = await reader.ReadToEndAsync();
            _Stream.Close();
            reader.Close();
            response.Close();
            return responseData;
        }

        /*-----------------------------------------------------------*/

        /*HttpClient*/
        public class HttpClient : IDisposable
        {
            public delegate void HttpClientHeaders(HttpRequestHeaders headers);
            
            /*-------------------------------------------------------*/
            
            private readonly Uri            _Url;
            private readonly HttpMethod     _Method;
            private readonly CoreHttpClient _HttpClient;

            public HttpClient(string url, string method)
            {
                _Url        = new Uri(url);
                _HttpClient = new CoreHttpClient();

                if
                (
                    method.Equals(Method.GET)    ||
                    method.Equals(Method.POST)   ||
                    method.Equals(Method.PUT)    ||
                    method.Equals(Method.PATCH)  ||
                    method.Equals(Method.DELETE)
                )
                {
                    switch (method)
                    {
                        case Method.GET    : _Method = HttpMethod.Get;    break;
                        case Method.POST   : _Method = HttpMethod.Post;   break;
                        case Method.PUT    : _Method = HttpMethod.Put;    break;
                        case Method.PATCH  : _Method = HttpMethod.Patch;  break;
                        case Method.DELETE : _Method = HttpMethod.Delete; break;
                    }
                }
                else
                {
                    throw new Exception(METHOD_NOT_CORRECT_EXCEPTION);
                }
            }
            
            /*Url Encoded | FormData*/
            public async Task<HttpResponseMessage> SendRequestByUrlEncodedAsync(Dictionary<string, string> data, HttpClientHeaders headers = null)
            {
                HttpRequestMessage request = new HttpRequestMessage(_Method, _Url)
                {
                    Content = new FormUrlEncodedContent(data)
                };

                headers(request.Headers);

                return await _HttpClient.SendAsync(request);
            }
            
            /*MultipartFormData | Send FormData by Files*/
            public async Task<HttpResponseMessage> SendRequestByMultipartFormDataAsync(Dictionary<string, string> data, List<IFormFile> file, string fileNameKey, HttpClientHeaders headers = null)
            {
                MultipartFormDataContent content = new MultipartFormDataContent();
                
                file.ForEach(item =>
                {
                    byte[] bytes = new BinaryReader(item.OpenReadStream()).ReadBytes((int) item.OpenReadStream().Length);
                    content.Add(new ByteArrayContent(bytes), fileNameKey, item.FileName);
                });

                content.Add(new FormUrlEncodedContent(data));
                
                HttpRequestMessage request = new HttpRequestMessage(_Method, _Url)
                {
                    Content = content
                };

                headers(request.Headers);

                return await _HttpClient.SendAsync(request);
            }
            
            /*MultipartFormData | Send FormData by File*/
            public async Task<HttpResponseMessage> SendRequestByMultipartFormDataAsync(Dictionary<string, string> data, IFormFile file, string fileNameKey, HttpClientHeaders headers = null)
            {
                byte[] bytes = new BinaryReader(file.OpenReadStream()).ReadBytes((int) file.OpenReadStream().Length);
                
                MultipartFormDataContent content = new MultipartFormDataContent();

                content.Add(new ByteArrayContent(bytes), fileNameKey, file.FileName);
                content.Add(new FormUrlEncodedContent(data));

                HttpRequestMessage request = new HttpRequestMessage(_Method, _Url)
                {
                    Content = content
                };

                headers(request.Headers);

                return await _HttpClient.SendAsync(request);
            }
            
            /*JSON*/
            public async Task<HttpResponseMessage> SendRequestByJsonAsync(object data, HttpClientHeaders headers = null)
            {
                HttpRequestMessage request = new HttpRequestMessage(_Method, _Url)
                {
                    Content = String.GetStringContent(data)
                };

                headers(request.Headers);

                return await _HttpClient.SendAsync(request);
            }
        
            /*-----------------------------------------------------------*/
        
            public void Dispose()
            {
                _HttpClient?.Dispose();
            }
        }
        
        /*-----------------------------------------------------------*/
        
        public static class Method
        {
            public const string GET    = "GET";
            public const string POST   = "POST";
            public const string PUT    = "PUT";
            public const string PATCH  = "PATCH";
            public const string DELETE = "DELETE";
        }
    }
}