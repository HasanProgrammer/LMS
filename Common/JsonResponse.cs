using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Common
{
    public class JsonResponse
    {
        public static void Handle(HttpContext context, int StatusCode)
        {
            context.Response.Headers.Clear();
            context.Response.StatusCode = StatusCode;
            context.Response.Headers.Add("Content-Type", "application/json");
            context.Response.Headers.Add("Accept"      , "application/json");
        }

        public static void Handle(HttpContext context, Dictionary<string, string> Headers)
        {
            foreach (var (key, value) in Headers)
            {
                context.Response.Headers.Add(key, value);
            }
        }

        public static void Handle(HttpContext context, string key, object data)
        {
            context.Response.Headers.Add(key, JsonConvert.SerializeObject(data));
        }
        
        public static JsonResult Return(int code, string message, object data)
        {
            return new JsonResult(new
            {
                code,
                msg  = message,
                body = data
            });
        }
    }
}