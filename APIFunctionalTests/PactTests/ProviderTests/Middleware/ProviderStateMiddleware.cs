using ApiTests.FunctionalApiTests;
using AutomationTestSample.Dtos;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace ApiTests.PactTests.ProviderTests.Middleware
{
    public class ProviderStateMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDictionary<string, Action> _providerStates;

        private HttpClient _httpClient = new HttpClient();

        public ProviderStateMiddleware(RequestDelegate next)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44449/");

            _next = next;
            _providerStates = new Dictionary<string, Action>
            {
                { "There is a post orders api", postOrderApiAsync}

                //Instead of listing all provider states inside here....we are specifying them under each file in folder 'ProviderState'...

                //{ "product with ID 10 exists", Product10Exists }
            };
        }

        private async void postOrderApiAsync()
        {
            PostOrder postOrderRequest = new PostOrder(
                        PatientMrn: "*3*5*7*9*12*15*",
                        PatientFirstName: "Winter",
                        PatientLastName: "Soldier",
                        AccessionNumber: "1001",
                        OrgCode: "LUM",
                        SiteId: 1001,
                        Modality: "MR",
                        StudyDateTime: DateTimeOffset.Now);

            var requestStr = JsonConvert.SerializeObject(postOrderRequest);
            var requestContent = new StringContent(requestStr);
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            //Act
            var response = await _httpClient.PostAsync($"api/orders", requestContent);
            var strResponse = TestHelpers.GetResponseAsStr(response);
        }

        //private void Product10Exists()
        //{
        //    List<Product> products = new List<Product>()
        //    {
        //        new Product(10, "28 Degrees", "CREDIT_CARD", "v1")
        //    };

        //    _repository.SetState(products);
        //}


        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/provider-states"))
            {
                await this.HandleProviderStatesRequest(context);
                await context.Response.WriteAsync(String.Empty);
            }
            else
            {
                await this._next(context);
            }
        }

        private async Task HandleProviderStatesRequest(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;

            if (context.Request.Method.ToUpper() == HttpMethod.Post.ToString().ToUpper() &&
                context.Request.Body != null)
            {
                string jsonRequestBody = String.Empty;
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                {
                    jsonRequestBody = await reader.ReadToEndAsync();
                }

                var providerState = JsonConvert.DeserializeObject<ProviderState>(jsonRequestBody);

                //A null or empty provider state key must be handled
                if (providerState != null && !String.IsNullOrEmpty(providerState.State))
                {
                    _providerStates[providerState.State].Invoke();
                }
            }
        }
    }
}

