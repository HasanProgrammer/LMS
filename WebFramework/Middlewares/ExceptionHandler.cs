using System;
using System.Text;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebFramework.Exceptions;

namespace WebFramework.Middlewares
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _Next;
        private readonly IConfiguration  _Configuration;
        
        public ExceptionHandler(RequestDelegate next, IConfiguration configuration)
        {
            _Next          = next;
            _Configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _Next(context);
            }
            catch (TokenNotValidException)
            {
                JsonResponse.Handle(context, _Configuration.GetValue<int>("StatusCode:TokenIsNotValid"));
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    code = _Configuration.GetValue<int>("StatusCode:TokenIsNotValid"),
                    msg  = _Configuration.GetValue<string>("Messages:TokenIsNotValid"),
                    body = new { }
                }), Encoding.UTF8);
            }
            catch (TokenExpireException)
            {
                JsonResponse.Handle(context, _Configuration.GetValue<int>("StatusCode:TokenExpire"));
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    code = _Configuration.GetValue<int>("StatusCode:TokenExpire"),
                    msg  = _Configuration.GetValue<string>("Messages:TokenExpire"),
                    body = new { }
                }), Encoding.UTF8);
            }
            catch (UnAuthorizedException)
            {
                JsonResponse.Handle(context, _Configuration.GetValue<int>("StatusCode:UnAuthorized"));
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    code = _Configuration.GetValue<int>("StatusCode:UnAuthorized"),
                    msg  = _Configuration.GetValue<string>("Messages:UnAuthorized"),
                    body = new { }
                }), Encoding.UTF8);
            }
            catch (AuthenticationFaildException)
            {
                JsonResponse.Handle(context, _Configuration.GetValue<int>("StatusCode:AuthenticationFaild"));
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    code = _Configuration.GetValue<int>("StatusCode:AuthenticationFaild"),
                    msg  = _Configuration.GetValue<string>("Messages:AuthenticationFaild"),
                    body = new {}
                }), Encoding.UTF8);
            }
        }
    }
}