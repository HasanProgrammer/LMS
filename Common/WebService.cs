using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Common
{
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
        
        public async Task SendRequestByUrlEncodedAsync(string data) /* "Item1=Value1&Item2=Value2&..." */
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            _WebRequest.ContentType   = "application/x-www-form-urlencoded";
            _WebRequest.ContentLength = bytes.Length;
            _Stream = await _WebRequest.GetRequestStreamAsync();
            await _Stream.WriteAsync(bytes, 0, bytes.Length);
            _Stream.Close();
        }
        
        public async Task SendRequestByJsonAsync(object data) /* new { Item1 = Value1 , Item2 = Value2 , ... } */
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