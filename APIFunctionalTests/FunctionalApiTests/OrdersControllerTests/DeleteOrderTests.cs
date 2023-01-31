using Microsoft.AspNetCore.Mvc.Testing;
using System.Diagnostics;
using System.Net;
using Xunit;

namespace ApiTests.FunctionalApiTests.OrdersControllerTests
{
    public class DeleteOrderTests
    {
        private HttpClient _httpClient;
        private const int _expectedMaxElapsedMilliseconds = 6000;

        public DeleteOrderTests()
        {
            var webAppFactory = new WebApplicationFactory<Program>(); //when you run the test then it will start the api project server internally and then execute the test
            _httpClient = webAppFactory.CreateDefaultClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44449/");
        }

        [Theory]
        [MemberData(nameof(DeleteOrder_Data))]
        public async Task DeleteOrderApiTests(string testNo, int orderId, HttpStatusCode expectedStatusCode, string expectedMessage = null)
        {
            //Arrange
            var stopwatch = Stopwatch.StartNew(); //For API Performance check

            //Act
            var response = await _httpClient.DeleteAsync($"/api/orders/{orderId}");
            var strResponseDeleteOrder = TestHelpers.GetResponseAsStr(response);

            //Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.True(stopwatch.ElapsedMilliseconds < _expectedMaxElapsedMilliseconds, $"Actual response time '${stopwatch.ElapsedMilliseconds}' is not < _expectedMaxElapsedMilliseconds '${_expectedMaxElapsedMilliseconds}'");

            if (expectedStatusCode == HttpStatusCode.NoContent){
                Assert.Equal(0, response.Content.Headers.ContentLength);

                var responseGetOrder = await _httpClient.GetAsync($"/api/orders/{orderId}");
                var strResponseGetOrder = TestHelpers.GetResponseAsStr(responseGetOrder);
                Assert.Matches(expectedMessage, strResponseGetOrder);
            }
            else {
                Assert.Matches(expectedMessage, strResponseDeleteOrder);
            }
        }

        public static List<object[]> DeleteOrder_Data()
        {
            var data = new List<object[]>
            {
                //204 - No Content
                new object[]
                {
                    "tc001", 9, HttpStatusCode.NoContent, "{.*\"title\":\"Not Found\",\"status\":404,\"detail\":\"No order found with Id: \\[9\\]\",\"traceId\":.*}"
                },
                
                //404
                /*Note: This test case WILL FAIL as there is no such code for this scenario error to be handled.
                 *Currently, deleting order which is not available then it does soft proceed without any errors which could also be fine for silent delete situation
                */
                new object[]
                {
                    "tc002", 111, HttpStatusCode.NoContent, "{.*\"title\":\"Not Found\",\"status\":404,\"detail\":\"Cant delete as No order found with Id: \\[111\\]\",\"traceId\":.*}"
                },

            };
            return data;
        }

    }
}
