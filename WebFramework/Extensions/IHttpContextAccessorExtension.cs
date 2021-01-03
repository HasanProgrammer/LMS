using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace WebFramework.Extensions
{
    public static class IHttpContextAccessorExtension
    {
        public static string GetCurrentRouteName(this IHttpContextAccessor Context)
        {
            return Context.HttpContext.GetEndpoint().Metadata.GetMetadata<EndpointNameMetadata>().EndpointName;
        }

        public static T GetCurrentRouteValue<T>(this IHttpContextAccessor Context, string NameValue)
        {
            return (T) Context.HttpContext.GetRouteData().Values[NameValue];
        }
    }
}