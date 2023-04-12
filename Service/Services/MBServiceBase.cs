using AutoMapper;
using Microsoft.Extensions.Configuration;
using ServerlessLib;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service1
{
    public class MBServiceBase
    {
        private IHttpClientFactory _httpClientFactory;
        private IMapper _mapper;
        private const string URL = "";
        private IConfiguration _Configuration { get; set; }

        public MBServiceBase(IHttpClientFactory httpClientFactory, IMapper mapper, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _Configuration = config;
        }

        protected async Task<T> CallService<T>(string uri)
        {
            try
            {
                HttpResponseMessage response = await InvokeService1(uri);
                return await CollateServiceResponse<T>(response);
            }
            catch (ArgumentNullException)
            {
                throw new BadRequestException("The request is invalid.");
            }
            catch (HttpRequestException)
            {
                throw new BadRequestException("HTTP request is invalid.");
            }
            catch (Exception)
            {
                throw new BadRequestException("HTTP request is invalid.");
            }
        }
        private async Task<HttpResponseMessage> InvokeService1(string uri)
        {
            return await _httpClientFactory.CreateClient().GetAsync(uri);
        }
        protected virtual async Task<T> CollateServiceResponse<T>(HttpResponseMessage response)
        {
            T retval = default(T);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStreamAsync();
                if (body != null)
                {
                    retval = await JsonSerializer.DeserializeAsync<T>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            else
            {
                // deserialise an error response object.
                string errMsg = "";
                ProcessResponse(response, errMsg);
            }
            return retval;
        }
        protected async Task ProcessResponse(HttpResponseMessage response, string errMsg)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new BadRequestException(errMsg);
                case HttpStatusCode.Forbidden:
                    return;
                case HttpStatusCode.PaymentRequired:
                    throw new SomeAppSoecificException(errMsg);
                case HttpStatusCode.NotFound:
                    throw new NotFoundException(errMsg);
                case HttpStatusCode.InternalServerError:
                    throw new ServerErrorException(errMsg);
                case HttpStatusCode.ServiceUnavailable:
                    throw new ServerErrorException($"The postcode log 500 error: {errMsg}");
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorisedException(errMsg);
            }
        }
    }
}
