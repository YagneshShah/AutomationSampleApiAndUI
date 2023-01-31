using PactNet.Mocks.MockHttpService.Models;

namespace ApiTests.PactTests.Consumer
{
    public class BaseApiConsumerTest
    {
        protected ProviderServiceRequest GetDefaultProviderServiceRequest(string path, HttpVerb httpMethod = HttpVerb.Get)
        {
            return new ProviderServiceRequest
            {
                Method = httpMethod,
                Path = path,
                Headers = new Dictionary<string, object>
                {
                    { "Accept", "application/json" }
                }
            };
        }

        protected ProviderServiceRequest GetDefaultProviderServiceRequest(string path, dynamic body, HttpVerb httpMethod = HttpVerb.Get)
        {
            return new ProviderServiceRequest
            {
                Method = httpMethod,
                Path = path,
                Headers = new Dictionary<string, object>
                {
                    { "Accept", "application/json" }
                },
                Body = body
            };
        }

        protected ProviderServiceRequest GetDefaultProviderServiceRequest(string path, dynamic body, HttpVerb httpMethod = HttpVerb.Get, String contentType = "application/json")
        {
            return new ProviderServiceRequest
            {
                Method = httpMethod,
                Path = path,
                Headers = new Dictionary<string, object>
                {
                    { "Content-Type", contentType }
                },
                Body = body
            };
        }

        protected ProviderServiceResponse GetDefaultProviderServiceResponse(dynamic body, int httpStatus = 200)
        {
            return new ProviderServiceResponse
            {
                Status = httpStatus,
                Headers = new Dictionary<string, object>
                {
                    { "Content-Type", "application/json; charset=utf-8" }
                },
                Body = body
            };
        }

        protected ProviderServiceResponse GetDefaultProviderServiceResponse(bool withResponseHeaders = true, int httpStatus = 200)
        {
            if (withResponseHeaders == true)
            {
                return new ProviderServiceResponse
                {
                    Status = httpStatus,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    }
                };
            }
            else
            {
                return new ProviderServiceResponse
                {
                    Status = httpStatus,
                };
            }
        }


    }
}
