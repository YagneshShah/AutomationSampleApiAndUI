using Xunit;

namespace ApiTests.PactTests.Consumer.OrdersApi
{
    [CollectionDefinition("OrdersApiPactCollection")]
    public class OrdersApiPactTestCollection : ICollectionFixture<OrdersApiPactFixture>
    {
    }
}
