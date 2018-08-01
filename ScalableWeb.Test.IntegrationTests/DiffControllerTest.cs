using System;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using ScalableWeb.Models;
using Xunit;

namespace ScalableWeb.Test.IntegrationTests
{
    public class DiffControllerTest
    {

        [Fact]
        public async Task PostLeftTest()
        {
            const string route = "/v1/diff/1/left";

            var model = new DiffDataViewModel
            {
                Data = GetBase64Encoded("{ \"message\": \"CONTENT FOR TEST\"")
            };
            
            var response = await ServerTestHelper.PostAsync(route, GetHttpJsonContent(model));

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.ReadAsStringAsync().Result.Should().Be("{\"diffId\":1,\"side\":\"Left\"}");
        }

        [Fact]
        public async Task PostRightTest()
        {
            const string route = "/v1/diff/2/right";

            var model = new DiffDataViewModel
            {
                Data = GetBase64Encoded("{ \"message\": \"CONTENT FOR TEST\"")
            };
            
            var response = await ServerTestHelper.PostAsync(route, GetHttpJsonContent(model));

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.ReadAsStringAsync().Result.Should().Be("{\"diffId\":2,\"side\":\"Right\"}");
        }

        [Fact]
        public async Task DiffWithDataNotFoundTest()
        {
            const string route      = "/v1/diff/3";

            using (var server = new TestServer(new WebHostBuilder().UseStartup<Startup>()))
            using (var client = server.CreateClient())
            {
                var response = await client.GetAsync(route);
                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
                response.Content.ReadAsStringAsync().Result.Should().Be("{\"areEqual\":false,\"message\":\"No data found for ID 3.\"}");
            }
        }

        [Fact]
        public async Task DiffBetweenTwoEqualDataTest()
        {
            const string leftRoute  = "/v1/diff/3/left";
            const string rightRoute = "/v1/diff/3/right";
            const string route      = "/v1/diff/3";

            var model = new DiffDataViewModel
            {
                Data = GetBase64Encoded("{ \"message\": \"CONTENT FOR TEST\"")
            };

            using (var server = new TestServer(new WebHostBuilder().UseStartup<Startup>()))
            using (var client = server.CreateClient())
            {
                var leftResponse = await client.PostAsync(leftRoute, GetHttpJsonContent(model));
                leftResponse.EnsureSuccessStatusCode();

                var rightResponse = await client.PostAsync(rightRoute, GetHttpJsonContent(model));
                rightResponse.EnsureSuccessStatusCode();

                var response = await client.GetAsync(route);
                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
                response.Content.ReadAsStringAsync().Result.Should().Be("{\"areEqual\":true,\"message\":\"They are equal!\"}");
            }
        }

        [Fact]
        public async Task DiffBetweenTwoDifferentDataSizeTest()
        {
            const string leftRoute  = "/v1/diff/3/left";
            const string rightRoute = "/v1/diff/3/right";
            const string route      = "/v1/diff/3";

            var leftModel = new DiffDataViewModel
            {
                Data = GetBase64Encoded("{ \"message\": \"1+2+3+4\"")
            };

            var rightModel = new DiffDataViewModel
            {
                Data = GetBase64Encoded("{ \"message\": \"1+2+3\"")
            };

            using (var server = new TestServer(new WebHostBuilder().UseStartup<Startup>()))
            using (var client = server.CreateClient())
            {
                var leftResponse = await client.PostAsync(leftRoute, GetHttpJsonContent(leftModel));
                leftResponse.EnsureSuccessStatusCode();

                var rightResponse = await client.PostAsync(rightRoute, GetHttpJsonContent(rightModel));
                rightResponse.EnsureSuccessStatusCode();

                var response = await client.GetAsync(route);
                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
                response.Content.ReadAsStringAsync().Result.Should().Be("{\"areEqual\":false,\"message\":\"Size difference! Left: 22; Right: 20\"}");
            }
        }

        [Fact]
        public async Task DiffBetweenTwoDifferentDataTest()
        {
            const string leftRoute  = "/v1/diff/3/left";
            const string rightRoute = "/v1/diff/3/right";
            const string route      = "/v1/diff/3";

            var leftModel = new DiffDataViewModel
            {
                Data = GetBase64Encoded("{ \"message\": \"1+2+3+4+6\"")
            };

            var rightModel = new DiffDataViewModel
            {
                Data = GetBase64Encoded("{ \"message\": \"1+2+3+5+6\"")
            };

            using (var server = new TestServer(new WebHostBuilder().UseStartup<Startup>()))
            using (var client = server.CreateClient())
            {
                var leftResponse = await client.PostAsync(leftRoute, GetHttpJsonContent(leftModel));
                leftResponse.EnsureSuccessStatusCode();

                var rightResponse = await client.PostAsync(rightRoute, GetHttpJsonContent(rightModel));
                rightResponse.EnsureSuccessStatusCode();

                var response = await client.GetAsync(route);
                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
                response.Content.ReadAsStringAsync().Result.Should().Be("{\"areEqual\":false,\"message\":\"They are different from the index 20.\"}");
            }
        }

        private static StringContent GetHttpJsonContent(DiffDataViewModel content)
        {
            return new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        }

        private static string GetBase64Encoded(string json)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        }
    }
}
