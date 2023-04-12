using AutoMapper;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Service1
{
    public class Service1 : MBServiceBase, IService1
    {
        public Service1(IHttpClientFactory httpClientFactory, IMapper mapper, IConfiguration config) 
            :base(httpClientFactory, mapper, config) { }

        public async Task<ApplesPayloadModelOut> CallSomeService(string query)
        {
            string uri = ConstructUrl(query);
            return await CallService<ApplesPayloadModelOut>(uri);
        }
        private string ConstructUrl(string query)
        {
            return $"https://someUrl/{query}";
        }
        protected override async Task ProcessResponse(HttpResponseMessage response, string errMsg)
        {
            base.ProcessResponse(response, errMsg);
            switch (response.StatusCode)
            {
                case HttpStatusCode.PaymentRequired:
                    throw new SomeAppSoecificException(errMsg);
            }
        }

    }
}
