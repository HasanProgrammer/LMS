using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Common;
using LMS;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WebFramework.Filters.Controllers.AuthController;
using Xunit;

namespace XUnitTest.Actions.AuthController
{
    public class LoginTests : IClassFixture<BaseTest<Startup>>
    {
        private readonly HttpClient _HttpClient;
        
        public LoginTests(BaseTest<Startup> Test)
        {
            _HttpClient = Test._HttpClient;
        }

        /*در تست زیر ، هیچ داده ای برای لاگین به سیستم ارسال نمی گردد و انتظار می رود ، سیستم در خروجی status = 400 را برگرداند*/
        [Fact]
        public async Task IfDontSendData_MustReturnStatusCode400()
        {
            HttpResponseMessage response = await _HttpClient.PatchAsync("/api/v1.0/auth/login", String.GetStringContent(new {}));
            Response result = await response.Content.ReadFromJsonAsync<Response>();
            if (result?.code == 400)
            {
                Assert.True(true);
                return;
            }
            Assert.True(false);
        }
        
        /*-----------------------------------------------------------*/
        
        public class Response
        {
            public int code    { get; set; }
            public string msg  { get; set; }
            public object body { get; set; }
        }
    }
}