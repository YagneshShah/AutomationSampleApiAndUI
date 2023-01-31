using AutoFixture;
using AutomationTestSample.Dtos;
using PactNet.Matchers;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;

namespace ApiTests.PactTests.Consumer.OrdersApi
{
    [Collection("OrdersApiPactCollection")]
    public class OrdersApiConsumerTests : BaseApiConsumerTest
    {
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;

        public OrdersApiConsumerTests(OrdersApiPactFixture data)
        {
            _mockProviderServiceBaseUri = data.MockProviderServiceBaseUri;
            _mockProviderService = data.MockProviderService;
            _mockProviderService.ClearInteractions(); //NOTE: Clears any previously registered interactions before the test is run
        }

        [Fact]
        public void GetOrdersApiIsCalled_ThenReturnSomeValues()
        {
            Func<dynamic, IMatcher> _ = Match.Type;
            IEnumerable<GetOrder> responseBodyMock = new Fixture().CreateMany<GetOrder>(3);

            //Arrange
            _mockProviderService
              .Given("There is a Orders api")
              .UponReceiving("A GET request to Orders api should retrieve list of orders")
              .With(GetDefaultProviderServiceRequest($"/api/orders"))
              .WillRespondWith(GetDefaultProviderServiceResponse(_(responseBodyMock)));//NOTE: WillRespondWith call must come last as it will register the interaction

            var consumer = new OrdersApiClient(_mockProviderServiceBaseUri);

            //Act
            var result = consumer.GetOrders();

            //Assert
            Assert.Equal(responseBodyMock, result);

            _mockProviderService.VerifyInteractions(); //NOTE: Verifies that interactions registered on the mock provider are called at least once
        }

        [Fact]
        public void GetOrderByIdApiIsCalled_ThenReturnSomeOrder()
        {
            int id = 101;
            Func<dynamic, IMatcher> _ = Match.Type;
            GetOrder responseBodyMock = new Fixture().Create<GetOrder>();

            //Arrange
            _mockProviderService
              .Given("There is a OrderById api")
              .UponReceiving("A GET request to OrdersById api should retrieve order details")
              .With(GetDefaultProviderServiceRequest($"/api/orders/{id}"))
              .WillRespondWith(GetDefaultProviderServiceResponse(_(responseBodyMock)));//NOTE: WillRespondWith call must come last as it will register the interaction

            var consumer = new OrdersApiClient(_mockProviderServiceBaseUri);

            //Act
            var result = consumer.GetOrderById(id);

            //Assert
            Assert.Equal(responseBodyMock, result);

            _mockProviderService.VerifyInteractions(); //NOTE: Verifies that interactions registered on the mock provider are called at least once
        }

        [Fact]
        public void PostOrderApiIsCalled_ThenReturnCreatedWithNoContent()
        {
            Func<dynamic, IMatcher> _ = Match.Type;
            var responseBodyMock = "";
            var requestContent = new Fixture().Create<PostOrder>();

            //Arrange
            _mockProviderService
              .Given("There is a post orders api")
              .UponReceiving("A POST request to Orders api should create order with no response")
              .With(GetDefaultProviderServiceRequest($"/api/orders", _(requestContent), HttpVerb.Post, "application/json; charset=utf-8"))
              .WillRespondWith(GetDefaultProviderServiceResponse(false, 201));//NOTE: WillRespondWith call must come last as it will register the interaction

            var consumer = new OrdersApiClient(_mockProviderServiceBaseUri);

            //Act
            var result = consumer.PostOrder(requestContent);

            //Assert
            Assert.Equal(responseBodyMock, result);

            _mockProviderService.VerifyInteractions(); //NOTE: Verifies that interactions registered on the mock provider are called at least once
        }


    }
}
