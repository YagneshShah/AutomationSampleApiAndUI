using Microsoft.AspNetCore;
using PactNet;
using PactNet.Infrastructure.Outputters;
using ApiTests.PactTests.ProviderTests.XUnitHelpers;
using Xunit;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Hosting;

namespace ApiTests.PactTests.ProviderTests
{
    public class ProviderPactTests2
    {
        private string _providerUri { get; }
        private string _pactServiceUri { get; }
        private IWebHost _webHost { get; }
        private ITestOutputHelper _outputHelper { get; }

        public ProviderPactTests2(ITestOutputHelper output)
        {
            _outputHelper = output;
            _providerUri = "http://localhost:44449";
            _pactServiceUri = "http://localhost:44449";

            _webHost = WebHost.CreateDefaultBuilder()
                .UseUrls(_pactServiceUri)
                .UseStartup<TestStartup>()
                .Build();

            _webHost.Start();
        }

        [Fact]
        //no need for Middleware or startup as this directly hits the localhost which is active and running
        public void EnsureProviderApi_HonoursPactWithOrdersApiConsumer() 
        {
            // Arrange
            var config = new PactVerifierConfig
            {
                // NOTE: We default to using a ConsoleOutput, however xUnit 2 does not capture the console output,
                // so a custom outputter is required.
                Outputters = new List<IOutput>
                {
                    new XUnitOutput(_outputHelper)
                },
                Verbose = true //output verbose verification logs to the test output
            };

            //Act / Assert
            new PactVerifier(config)
                .ProviderState($"{_pactServiceUri}/provider-states")
                .ServiceProvider("OrdersApiController", _providerUri)
                .HonoursPactWith("PactDemoConsumer")
                .PactUri(@"..\..\..\PactTests\Pacts\pacttests.consumer.ordersapi-controller.orders.json")
                .Verify(description: "A POST request to Orders api should create order with no response", providerState: "There is a post orders api");
        }
    }
}
