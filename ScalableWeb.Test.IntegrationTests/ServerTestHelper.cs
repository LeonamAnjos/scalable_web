using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using ScalableWeb.Models;

namespace ScalableWeb.Test.IntegrationTests
{
    public static class ServerTestHelper
    {
        public static async Task<HttpResponseMessage> GetAsync(string route)
        {
            using (var server = new TestServer(new WebHostBuilder().UseStartup<Startup>()))
            using (var client = server.CreateClient())
            {
                return await client.GetAsync(route);
            }
        }

        public static async Task<HttpResponseMessage> PostAsync(string route, HttpContent content)
        {
            using (var server = new TestServer(new WebHostBuilder().UseStartup<Startup>()))
            using (var client = server.CreateClient())
            {
                return await client.PostAsync(route, content);
            }
        }
    }
}