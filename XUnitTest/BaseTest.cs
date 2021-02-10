using System;
using System.IO;
using System.Net.Http;
using AngleSharp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebFramework.Extensions;

namespace XUnitTest
{
    public class BaseTest<TBoot> : IDisposable where TBoot : class
    {
        private readonly TestServer _Server;
        public  readonly HttpClient _HttpClient;
        
        public BaseTest()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            
            IWebHostBuilder builder = new WebHostBuilder().UseStartup<TBoot>().UseConfiguration(configuration).UseEnvironment("Development");

            _Server     = new TestServer(builder);
            _HttpClient = _Server.CreateClient();
            _HttpClient.BaseAddress = new Uri("http://localhost:5000");
        }

        public void Dispose() /*GC | Explicit Cleanup*/
        {
            _Server.Dispose();
            _HttpClient.Dispose();
        }
    }
}