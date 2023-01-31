using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using FluentAssertions;
using Newtonsoft.Json;
using AutomationTestSample.Dtos;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Xunit;

namespace ApiTests.FunctionalApiTests.OrdersControllerTests
{
    public class GetOrderById
    {
        private HttpClient _httpClient;
        private const int _expectedMaxElapsedMilliseconds = 1000;

        public GetOrderById()
        {
            var webAppFactory = new WebApplicationFactory<Program>(); //when you run the test then it will start the api project server internally and then execute the test
            _httpClient = webAppFactory.CreateDefaultClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44449/");
        }


        [Theory]
        [MemberData(nameof(GetOrderById_Data))]
        public async Task GetOrderByIdTests(string testNo, int id, HttpStatusCode expectedStatusCode, string expectedMediaType, GetOrder expectedContent, string errorMessage = null)
        {
            //Arrange
            var stopwatch = Stopwatch.StartNew(); //For API Performance check

            //Act
            var response = await _httpClient.GetAsync($"/api/orders/{id}");
            var strResponse = TestHelpers.GetResponseAsStr(response);

            //Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.True(stopwatch.ElapsedMilliseconds < _expectedMaxElapsedMilliseconds, $"Actual response time '${stopwatch.ElapsedMilliseconds}' is not < _expectedMaxElapsedMilliseconds '${_expectedMaxElapsedMilliseconds}'");
            Assert.Equal(expectedMediaType, response.Content.Headers.ContentType?.MediaType);

            //Assert.True(JToken.DeepEquals(JToken.Parse(expectedContent), JToken.Parse(strResponse)));
            /*
             * JsonToken DeepEqual is handy in few situations to assert actual vs expected response content.
             * But in our case, since we have dates as part of API response, which keeps changing for each execution. 
             * Hence, JToken.DeepEquals() is not a good option to assert.
             */

            /* As an alternative, we would be using library fluentassertions.json for json comparison and assertions.
             * References: 
             * https://github.com/fluentassertions/fluentassertions.json,
             * https://fluentassertions.com/objectgraphs/
             */
            if (expectedStatusCode == HttpStatusCode.OK){
                var jsonResponse = JsonConvert.DeserializeObject<GetOrder>(strResponse);

                jsonResponse.Should().BeAssignableTo<GetOrder>();

                jsonResponse.Should().BeEquivalentTo(expectedContent, options =>
                    options.ComparingByMembers<GetOrder>()
                           .Excluding(x => x.StudyDateTime)
                           .Excluding(x => x.CreatedAt)
                           .Excluding(x => x.UpdatedAt));
            }
            else {
                strResponse.Should().Contain(errorMessage);
            }
        }
        public static List<object[]> GetOrderById_Data()
        {
            var data = new List<object[]>
            {
                //200
                new object[]
                {
                    "tc001", 1, HttpStatusCode.OK, "application/json",
                    new GetOrder(
                        Id: 1,
                        AccessionNumber: "00486",
                        OrgCode: "LUM",
                        SiteName: "Northern Beaches Hospital",
                        PatientMrn: "P302",
                        PatientName: "James Anderson",
                        Modality: "MR",
                        StudyDateTime: DateTimeOffset.Now,
                        Status: "ZZ",
                        CreatedAt: DateTimeOffset.Now,
                        UpdatedAt: DateTimeOffset.Now
                    )
                },

                //HttpStatusCode = 404
                new object[]
                {
                    "tc002", 100, HttpStatusCode.NotFound, "application/problem+json",
                    null, "{\"type\":\"https://tools.ietf.org/html/rfc7231#section-6.5.4\",\"title\":\"Not Found\",\"status\":404,\"detail\":\"No order found with Id: [100]\",\"traceId\":"
                }
            };
            return data;
        }


    }
}
