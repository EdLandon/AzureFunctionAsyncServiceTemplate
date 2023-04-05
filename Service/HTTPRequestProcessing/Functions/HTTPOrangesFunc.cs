using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ServerlessLib;
using System;
using System.Net;

namespace Service1.Functions
{
    public class HTTPOrangesFunc : HTTPServerlessBase<OrangesPayloadModelIn, HTTPOrangesFunc>
	{
        public HTTPOrangesFunc(ILogger<HTTPOrangesFunc> log, IConfiguration configuration)
        {
            _log = log;
            _config = configuration;
        }

        [FunctionName("HTTPOrangesFuncRun")]
        [OpenApiOperation(operationId: "HTTPOrangesFunc", tags: new[] { "name" })]
        [OpenApiParameter(name: "OrangesPayloadModelIn", In = ParameterLocation.Query, Required = true, Type = typeof(OrangesPayloadModelIn), Description = "The OrangesPayloadModelIn parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(OrangesPayloadModelOut), Description = "The OK response")]
        public IActionResult HTTPOrangesFuncRun(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [ServiceBus("%requests:inboxQueue%", ServiceBusEntityType.Queue, Connection = "ServiceBusConnection")] out ServiceBusMessage msg)
		{
            msg = null;
            return base.ProcessRequest(req, _config["requests:orangesFunc:inboxFQEN"], out msg);
        }

        public override void ValidateRequest()
		{
            bool valid = !string.IsNullOrEmpty( _deSerialisedModel.name );

            if (!valid) throw new ApplicationException("Invalid request: name missing.");
		}
    }
}
