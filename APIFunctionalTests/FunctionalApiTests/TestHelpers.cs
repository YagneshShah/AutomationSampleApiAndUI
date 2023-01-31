using AutomationTestSample.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Xunit;

namespace ApiTests.FunctionalApiTests
{
    public class TestHelpers
    {
        private const string _jsonMediaType = "application/json";
        private const int _expectedMaxElapsedMilliseconds = 1000;
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
        public static string GetResponseAsStr(HttpResponseMessage response)
        {
            return response.Content.ReadAsStringAsync().Result;
        }
        public static void AssertCommonResponseParts(HttpResponseMessage response,
            System.Net.HttpStatusCode expectedStatusCode, Stopwatch stopwatch)
        {
            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.True(stopwatch.ElapsedMilliseconds < _expectedMaxElapsedMilliseconds);
            Assert.Equal(_jsonMediaType, response.Content.Headers.ContentType?.MediaType);
        }
        public static void AssertCommonResponseAndContentAsync(HttpResponseMessage response,
            System.Net.HttpStatusCode expectedStatusCode, Stopwatch stopwatch, string expectedContent)
        {
            AssertCommonResponseParts(response, expectedStatusCode, stopwatch);
            Assert.True(JToken.DeepEquals(JToken.Parse(expectedContent), JToken.Parse(GetResponseAsStr(response))));
        }
    }
}

