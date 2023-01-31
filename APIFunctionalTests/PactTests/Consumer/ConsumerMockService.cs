using Newtonsoft.Json;
using PactNet;
using PactNet.Mocks.MockHttpService;
using System.Net;
using System.Net.Sockets;

namespace ApiTests.PactTests.Consumer
{
    public class ConsumerMockService : IDisposable
    {
        public IPactBuilder PactBuilder { get; private set; }
        public IMockProviderService MockProviderService { get; private set; }
        //public int MockServerPort { get { return 9222; } } //hardcode approach
        public int MockServerPort { get; private set; } //or get dynamically each time
        public string MockProviderServiceBaseUri { get { return String.Format("http://localhost:{0}", MockServerPort); } }
        public string ConsumerAppName { get; set; }
        public string ProviderAppName { get; set; }

        public ConsumerMockService(string consumer, string provider)
        {
            MockServerPort = GetTcpPort();
            ConsumerAppName = consumer;
            ProviderAppName = provider;

            PactBuilder = new PactBuilder(); //Defaults to specification version 1.1.0, uses default directories. PactDir: ..\..\pacts and LogDir: ..\..\logs
                                             //or
            PactBuilder = new PactBuilder(new PactConfig { SpecificationVersion = "2.0.0" }); //Configures the Specification Version
                                                                                              //or
            PactBuilder = new PactBuilder(new PactConfig { 
                PactDir = @"..\..\..\PactTests\Pacts", 
                LogDir = @"..\..\..\PactTests\Logs" }); //Configures the PactDir and/or LogDir.

            //PactBuilder
            //  .ServiceConsumer("Consumer")
            //  .HasPactWith("Something API Provider");
            PactBuilder
              .ServiceConsumer(ConsumerAppName)
              .HasPactWith(ProviderAppName);

            MockProviderService = PactBuilder.MockService(MockServerPort); //Configure the http mock server
                                                                           //or
            MockProviderService = PactBuilder.MockService(MockServerPort, false); //By passing true as the second param, you can enabled SSL. A self signed SSL cert will be provisioned by default.
            //                                                                     //or
            //MockProviderService = PactBuilder.MockService(MockServerPort, false, sslCert: "sslCert", sslKey: "sslKey"); //By passing true as the second param and an sslCert and sslKey, you can enabled SSL with a custom certificate. See "Using a Custom SSL Certificate" for more details.
                                                                                                                   //or
            MockProviderService = PactBuilder.MockService(MockServerPort, new JsonSerializerSettings()); //You can also change the default Json serialization settings using this overload
                                                                                                         //or
            MockProviderService = PactBuilder.MockService(MockServerPort, host: PactNet.Models.IPAddress.Any); //By passing host as IPAddress.Any, the mock provider service will bind and listen on all ip addresses

        }

        public void Dispose()
        {
            PactBuilder.Build(); //NOTE: Will save the pact file once finished
        }

        static int GetTcpPort()
        {
            TcpListener tcpListener = new TcpListener(System.Net.IPAddress.Loopback, 0);
            tcpListener.Start();
            int port = ((IPEndPoint)tcpListener.LocalEndpoint).Port;
            tcpListener.Stop();
            return port;
        }
    }
}

