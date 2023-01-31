using Microsoft.AspNetCore.Mvc.Testing;
using System.Diagnostics;
using Newtonsoft.Json;
using AutomationTestSample.Dtos;
using System.Net;
using System.Net.Http.Headers;
using AutoFixture;
using Xunit;

namespace ApiTests.FunctionalApiTests.OrdersControllerTests
{
    public class PostOrderTests
    {
        private HttpClient _httpClient;
        private const int _expectedMaxElapsedMilliseconds = 6000;

        public PostOrderTests()
        {
            var webAppFactory = new WebApplicationFactory<Program>(); //when you run the test then it will start the api project server internally and then execute the test
            _httpClient = webAppFactory.CreateDefaultClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44449/");
        }


        [Theory]
        [MemberData(nameof(PostOrder_Data))]
        public async Task PostOrderApiTests(string testNo, PostOrder postOrderRequest, HttpStatusCode expectedStatusCode, string expectedMediaType, GetOrder expectedContent, string expectedErrorMessage = null)
        {
            //Arrange
            var stopwatch = Stopwatch.StartNew(); //For API Performance check

            var requestStr = JsonConvert.SerializeObject(postOrderRequest);
            var requestContent = new StringContent(requestStr);
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            //Act
            var response = await _httpClient.PostAsync($"/api/orders", requestContent);
            var strResponse = TestHelpers.GetResponseAsStr(response);

            //Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.True(stopwatch.ElapsedMilliseconds < _expectedMaxElapsedMilliseconds, $"Actual response time '${stopwatch.ElapsedMilliseconds}' is not < _expectedMaxElapsedMilliseconds '${_expectedMaxElapsedMilliseconds}'");

            if (expectedStatusCode == HttpStatusCode.Created){
                Assert.Equal(0, response.Content.Headers.ContentLength);
            }
            else {
                Assert.Matches(expectedErrorMessage, strResponse);
            }
        }
        public static List<object[]> PostOrder_Data()
        {
            Fixture fixture = new Fixture();
            var data = new List<object[]>
            {
                //200
                //Note: Approach1: request content created by passing proper data
                new object[]
                {
                    "tc001"
                    ,new PostOrder(
                        PatientMrn: "*3*5*7*9*12*15*",
                        PatientFirstName:"Winter",
                        PatientLastName:"Soldier",
                        AccessionNumber:"1001",
                        OrgCode:"LUM",
                        SiteId: 1001,
                        Modality:"MR",
                        StudyDateTime: DateTimeOffset.Now )
                    ,HttpStatusCode.Created, null, null
                },

                //409 conflict - order exists
                new object[]
                {
                    "tc002"
                    ,new PostOrder(
                        PatientMrn: "*3*5*7*9*12*15*",
                        PatientFirstName:"Winter",
                        PatientLastName:"Soldier",
                        AccessionNumber:"1001",
                        OrgCode:"LUM",
                        SiteId: 1001,
                        Modality:"MR",
                        StudyDateTime: DateTimeOffset.Now )
                    ,HttpStatusCode.Conflict,
                    "application/problem+json", null, "{\"type\":\"https://tools.ietf.org/html/rfc7231#section-6.5.8\",\"title\":\"Conflict\",\"status\":409,\"detail\":\"An order already exists with accession number \\[1001\\]\",\"traceId\":.*"
                },

                //412 precondition failed - StudyDateTime cannot be in the future
                //Note: Approach2: request content created by using AutoFixture
                new object[]
                {
                    "tc003",
                    fixture.Build<PostOrder>()
                        .With(x => x.PatientMrn, fixture.Create<string>().Substring(0,15))
                        .With(x => x.AccessionNumber, fixture.Create<string>().Substring(0,16))
                        .With(x => x.OrgCode, fixture.Create<string>().Substring(0,5))
                        .With(x => x.Modality, fixture.Create<string>().Substring(0,5))
                        .With(x => x.StudyDateTime, DateTimeOffset.Now.AddDays(1))
                        .Create()

                    ,HttpStatusCode.PreconditionFailed,
                    "application/problem+json", null, "{\"status\":412,\"detail\":\"StudyDateTime cannot be in the future:.*\",\"traceId\":.*"
                },

                //400 - missing mandatory fields
                //Note: Approach2: request content created by using AutoFixture
                new object[]
                {
                    "tc004",
                    fixture.Build<PostOrder>()
                        .With(x => x.PatientMrn, "")
                        .With(x => x.PatientFirstName, "")
                        .With(x => x.PatientLastName, "")
                        .With(x => x.AccessionNumber, "")
                        .With(x => x.OrgCode, "")
                        .With(x => x.Modality, "")
                        .With(x => x.StudyDateTime, DateTimeOffset.Now.AddDays(1))
                        .Create()

                    ,HttpStatusCode.BadRequest,
                    "application/problem+json", null, "{\"type\":\"https://tools.ietf.org/html/rfc7231#section-6.5.1\"," +
                    "\"title\":\"One or more validation errors occurred.\"," +
                    "\"status\":400," +
                    "\"traceId\":.*," +
                    "\"errors\":" +
                        "{\"OrgCode\":\\[\"The OrgCode field is required.\"\\]," +
                        "\"Modality\":\\[\"The Modality field is required.\"\\]," +
                        "\"PatientMrn\":\\[\"The PatientMrn field is required.\"\\]," +
                        "\"AccessionNumber\":\\[\"The AccessionNumber field is required.\"\\]," +
                        "\"PatientLastName\":\\[\"The PatientLastName field is required.\"\\]," +
                        "\"PatientFirstName\":\\[\"The PatientFirstName field is required.\"\\]}}"
                },

                //400 - Max field length errors
                //Note: Approach2: request content created by using AutoFixture
                new object[]
                {
                    "tc005",
                    new Fixture().Build<PostOrder>()
                        .With(x => x.PatientMrn, "*3*5*7*10*13*16*")
                        .With(x => x.PatientFirstName, "2*4*6*8*11*14*17*20*23*26*29*32*35*38*41*44*47*50*53*56*59*62*65*")
                        .With(x => x.PatientLastName, "2*4*6*8*11*14*17*20*23*26*29*32*35*38*41*44*47*50*53*56*59*62*65*")
                        .With(x => x.AccessionNumber, "2*4*6*8*11*14*17*")
                        .With(x => x.OrgCode, "2*4*6*")
                        .With(x => x.Modality, "2*4*6*")
                        .With(x => x.StudyDateTime, DateTimeOffset.Now)
                        .Create()

                    ,HttpStatusCode.BadRequest,
                    "application/problem+json", null, "{\"type\":\"https://tools.ietf.org/html/rfc7231#section-6.5.1\"," +
                    "\"title\":\"One or more validation errors occurred.\"," +
                    "\"status\":400," +
                    "\"traceId\":.*," +
                    "\"errors\":" +
                        "{\"OrgCode\":\\[\"The field OrgCode must be a string or array type with a maximum length of '5'.\"\\]," +
                        "\"Modality\":\\[\"The field Modality must be a string or array type with a maximum length of '5'.\"\\]," +
                        "\"PatientMrn\":\\[\"The field PatientMrn must be a string or array type with a maximum length of '15'.\"\\]," +
                        "\"AccessionNumber\":\\[\"The field AccessionNumber must be a string or array type with a maximum length of '16'.\"\\]," +
                        "\"PatientLastName\":\\[\"The field PatientLastName must be a string or array type with a maximum length of '64'.\"\\]," +
                        "\"PatientFirstName\":\\[\"The field PatientFirstName must be a string or array type with a maximum length of '64'.\"\\]}}"
                }


            };
            return data;
        }
    }
}