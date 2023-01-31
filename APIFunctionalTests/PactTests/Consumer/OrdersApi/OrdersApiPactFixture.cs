
namespace ApiTests.PactTests.Consumer.OrdersApi
{
    public class OrdersApiPactFixture : ConsumerMockService
    {
        public OrdersApiPactFixture() : base("PactTests.Consumer.OrdersApi", "Controller.Orders")
        {
        }
    }
}