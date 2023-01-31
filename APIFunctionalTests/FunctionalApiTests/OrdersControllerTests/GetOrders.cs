using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using FluentAssertions;
using Newtonsoft.Json;
using AutomationTestSample.Dtos;
using Xunit;

namespace ApiTests.FunctionalApiTests.OrdersControllerTests
{
    public class GetOrders
    {
        private HttpClient _httpClient;
        private const int _expectedMaxElapsedMilliseconds = 1000;
        public GetOrders()
        {
            var webAppFactory = new WebApplicationFactory<Program>(); //when you run the test then it will start the api project server internally and then execute the test
            _httpClient = webAppFactory.CreateDefaultClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44449/");
        }

        [Fact]
        public async Task GetOrdersAPI_ReturnsExpectedResponseWith200AndPassesPerformanceMarkAndResponseHeader()
        {
            //Arrange
            var expectedStatusCode = System.Net.HttpStatusCode.OK;

            var expectedContent = "[{\"id\":1,\"accessionNumber\":\"00486\",\"orgCode\":\"LUM\",\"siteName\":\"Northern Beaches Hospital\",\"patientMrn\":\"P302\",\"patientName\":\"James Anderson\",\"modality\":\"MR\",\"studyDateTime\":\"2023-01-24T14:52:46.5653781+11:00\",\"status\":\"ZZ\",\"createdAt\":\"2023-01-24T14:54:27.5653781+11:00\",\"updatedAt\":\"2023-01-24T14:54:27.5653781+11:00\"},{\"id\":2,\"accessionNumber\":\"00487\",\"orgCode\":\"LUM\",\"siteName\":\"Baulkham Hills\",\"patientMrn\":\"P303\",\"patientName\":\"Sarah Jones\",\"modality\":\"US\",\"studyDateTime\":\"2023-01-24T14:58:06.565386+11:00\",\"status\":\"ZZ\",\"createdAt\":\"2023-01-24T15:01:27.565386+11:00\",\"updatedAt\":\"2023-01-24T15:01:27.565386+11:00\"},{\"id\":3,\"accessionNumber\":\"00492\",\"orgCode\":\"CUK\",\"siteName\":\"Spalding\",\"patientMrn\":\"P312\",\"patientName\":\"Tony Jenkins\",\"modality\":\"MR\",\"studyDateTime\":\"2023-01-24T04:03:39.5653866+00:00\",\"status\":\"ZZ\",\"createdAt\":\"2023-01-24T04:04:27.5653866+00:00\",\"updatedAt\":\"2023-01-24T04:04:27.5653866+00:00\"},{\"id\":4,\"accessionNumber\":\"00494\",\"orgCode\":\"LUM\",\"siteName\":\"Ingleburn\",\"patientMrn\":\"P067\",\"patientName\":\"Eva Larson\",\"modality\":\"CT\",\"studyDateTime\":\"2023-01-24T15:25:27.5653898+11:00\",\"status\":\"CMD\",\"createdAt\":\"2023-01-24T15:27:27.5653898+11:00\",\"updatedAt\":\"2023-01-24T15:27:27.5653898+11:00\"},{\"id\":5,\"accessionNumber\":\"00501\",\"orgCode\":\"LUM\",\"siteName\":\"St George Private Hospital\",\"patientMrn\":\"P332\",\"patientName\":\"James Martinez\",\"modality\":\"XR\",\"studyDateTime\":\"2023-01-24T15:26:33.5653902+11:00\",\"status\":\"CM\",\"createdAt\":\"2023-01-24T15:27:27.5653902+11:00\",\"updatedAt\":\"2023-01-24T15:27:27.5653902+11:00\"},{\"id\":6,\"accessionNumber\":\"00503\",\"orgCode\":\"USC\",\"siteName\":\"Clinic\",\"patientMrn\":\"P334\",\"patientName\":\"Jay Saunders\",\"modality\":\"US\",\"studyDateTime\":\"2023-01-24T15:26:39.5653906+11:00\",\"status\":\"CM\",\"createdAt\":\"2023-01-24T15:28:27.5653906+11:00\",\"updatedAt\":\"2023-01-24T15:28:27.5653906+11:00\"},{\"id\":7,\"accessionNumber\":\"00504\",\"orgCode\":\"CUK\",\"siteName\":\"Sussex\",\"patientMrn\":\"P335\",\"patientName\":\"Virgil Smith\",\"modality\":\"CT\",\"studyDateTime\":\"2023-01-24T04:52:10.565391+00:00\",\"status\":\"IP\",\"createdAt\":\"2023-01-24T04:53:27.565391+00:00\",\"updatedAt\":\"2023-01-24T04:53:27.565391+00:00\"},{\"id\":8,\"accessionNumber\":\"00506\",\"orgCode\":\"CUK\",\"siteName\":\"Lincoln\",\"patientMrn\":\"P337\",\"patientName\":\"William Richards\",\"modality\":\"XR\",\"studyDateTime\":\"2023-01-24T05:04:58.5653914+00:00\",\"status\":\"SC\",\"createdAt\":\"2023-01-24T05:06:27.5653914+00:00\",\"updatedAt\":\"2023-01-24T05:06:27.5653914+00:00\"},{\"id\":9,\"accessionNumber\":\"00507\",\"orgCode\":\"LUM\",\"siteName\":\"Camden Nuclear Medicine\",\"patientMrn\":\"P338\",\"patientName\":\"Alma Nuez\",\"modality\":\"XR\",\"studyDateTime\":\"2023-01-24T16:17:26.5653918+11:00\",\"status\":\"SC\",\"createdAt\":\"2023-01-24T16:18:27.5653918+11:00\",\"updatedAt\":\"2023-01-24T16:18:27.5653918+11:00\"}]";
            var expectedContentDeserialized = JsonConvert.DeserializeObject<IEnumerable<GetOrder>>(expectedContent);

            var stopwatch = Stopwatch.StartNew(); //For API Performance check

            //Act
            var response = await _httpClient.GetAsync("/api/orders");
            var strResponse = TestHelpers.GetResponseAsStr(response);
            var jsonResponseDeserialized = JsonConvert.DeserializeObject<IEnumerable<GetOrder>>(strResponse);


            //Assert            
            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.True(stopwatch.ElapsedMilliseconds < _expectedMaxElapsedMilliseconds, $"Actual response time '${stopwatch.ElapsedMilliseconds}' is not < _expectedMaxElapsedMilliseconds '${_expectedMaxElapsedMilliseconds}'");
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

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
            jsonResponseDeserialized.Should().HaveCountGreaterOrEqualTo(JToken.Parse(expectedContent).Count());
            jsonResponseDeserialized.Should().BeEquivalentTo(expectedContentDeserialized, options =>
                options.ComparingByMembers<GetOrder>()
                       .Excluding(x => x.StudyDateTime)
                       .Excluding(x => x.CreatedAt)
                       .Excluding(x => x.UpdatedAt));  //ComparingByMembers<>() is asserting both key and values exactly for all iterations in response

            jsonResponseDeserialized.Should().BeOfType<List<GetOrder>>();
        }
    }
}
