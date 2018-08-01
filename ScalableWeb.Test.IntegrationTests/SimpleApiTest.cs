using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace ScalableWeb.Test.IntegrationTests
{
    public class SimpleApiTest
    {
        [Fact]
        public async Task ApiValuesRouteTest()
        {
            var response = await ServerTestHelper.GetAsync("/api/values");
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
