using Microsoft.AspNetCore.Mvc;
using System;
using ServerlessLib;

namespace Service1
{
    // 400
    public class SomeAppSoecificException : MBHttpException
    {
        public SomeAppSoecificException(string message) :base(message, new BadRequestObjectResult(message) )
        {
        }
        public SomeAppSoecificException(string internalMessage, string externalMessage) 
            : base(internalMessage, externalMessage, new BadRequestObjectResult(externalMessage))
        {
        }
    }
}
