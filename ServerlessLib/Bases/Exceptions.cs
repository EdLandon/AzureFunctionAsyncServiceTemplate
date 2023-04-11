using Azure;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;

namespace ServerlessLib
{
    public class MBHttpException : Exception
    {
        public MBHttpException(string message, ObjectResult response) : base(message)
        {
            HttpResponse = response;
            InternalMessage = ExternalMessage = message;
        }  
        public ObjectResult HttpResponse { get; set; }
        public string InternalMessage { get; set; }
        public string ExternalMessage { get; set; }
        public MBHttpException(string internalMessage, string externalMessage, ObjectResult response) : base(internalMessage)
        {
            InternalMessage = internalMessage;
            ExternalMessage = externalMessage;
            HttpResponse = response;    
        }
    }
    // 404
    public class NotFoundException : MBHttpException
    {
        public NotFoundException(string message) : base(message, new NotFoundObjectResult(message) )
        {
        }
        public NotFoundException(string internalMessage, string externalMessage) : base(internalMessage, externalMessage, new NotFoundObjectResult(message) )
        {
        }

    }
    public class UnauthorisedException : MBHttpException
    {
        public UnauthorisedException(string message) : base(message, new UnauthorizedObjectResult(message))
        {
        }
        public UnauthorisedException(string internalMessage, string externalMessage) : base(internalMessage, externalMessage, new UnauthorisedException(externalMessage))
        {
        }
    }
    // 400
    public class BadRequestException : MBHttpException
    {
        public BadRequestException(string message) : base( message, new BadRequestObjectResult(message) )
        {
        }
        public BadRequestException(string internalMessage, string externalMessage) : base(internalMessage, externalMessage, new BadRequestException(externalMessage))
        {
        }
    }
    // 500
    public class ServerErrorException : MBHttpException
    {
        public ServerErrorException(string message) : base( message, new ExceptionResult(new Exception(message), true) )
        {
        }
        public ServerErrorException(string internalMessage, string externalMessage) : base(internalMessage, externalMessage, new ExceptionResult(new Exception(message), true))
        {
        }
    }
}
