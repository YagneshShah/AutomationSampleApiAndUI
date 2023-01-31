using AutomationTestSample.Dtos;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ApiTests.PactTests.Consumer.OrdersApi
{
    public class OrdersApiClient
    {
        private readonly HttpClient _client;

        public OrdersApiClient(string baseUri)
        {
            _client = new HttpClient { BaseAddress = new Uri(baseUri) };
        }

        public IEnumerable<GetOrder>? GetOrders()
        {
            string reasonPhrase;

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/orders");
            request.Headers.Add("Accept", "application/json");

            var response = _client.SendAsync(request);

            var responseString = response.Result.Content.ReadAsStringAsync().Result;
            var status = response.Result.StatusCode;

            reasonPhrase = response.Result.ReasonPhrase; //NOTE: any Pact mock provider errors will be returned here and in the response body

            request.Dispose();
            response.Dispose();

            if (status == HttpStatusCode.OK)
            {
                return !string.IsNullOrEmpty(responseString) ?
                  JsonConvert.DeserializeObject<IEnumerable<GetOrder>>(responseString)
                  : null;
            }
            throw new Exception(reasonPhrase);
        }

        public GetOrder? GetOrderById(int id)
        {
            string reasonPhrase;

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/orders/{id}");
            request.Headers.Add("Accept", "application/json");

            var response = _client.SendAsync(request);

            var responseString = response.Result.Content.ReadAsStringAsync().Result;
            var status = response.Result.StatusCode;

            reasonPhrase = response.Result.ReasonPhrase; //NOTE: any Pact mock provider errors will be returned here and in the response body

            request.Dispose();
            response.Dispose();

            if (status == HttpStatusCode.OK)
            {
                return !string.IsNullOrEmpty(responseString) ?
                  JsonConvert.DeserializeObject<GetOrder>(responseString)
                  : null;
            }
            throw new Exception(reasonPhrase);
        }

        public string PostOrder(PostOrder postOrderRequest)
        {
            string reasonPhrase;

            var request = new HttpRequestMessage(HttpMethod.Post, $"/api/orders");
            request.Headers.Add("Accept", "application/json; charset=utf-8");
            request.Content = new StringContent(JsonConvert.SerializeObject(postOrderRequest), Encoding.UTF8, "application/json");

            var response = _client.SendAsync(request);

            var responseString = response.Result.Content.ReadAsStringAsync().Result;
            var status = response.Result.StatusCode;

            reasonPhrase = response.Result.ReasonPhrase; //NOTE: any Pact mock provider errors will be returned here and in the response body

            request.Dispose();
            response.Dispose();

            if (status == HttpStatusCode.Created)
            {
                return string.IsNullOrEmpty(responseString) ?
                  "" //when success then response body is empty 
                  : responseString; //else error message
            }
            throw new Exception(reasonPhrase);
        }
    }
}

