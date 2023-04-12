using Microsoft.AspNetCore.Mvc;
using System;
using ServerlessLib;

namespace Service1
{
    // 400
    public class SomeAppSpecificException: MBHttpException
    {
        public SomeAppSpecificException(string message) :base(message, new BadRequestObjectResult(message) )
        {
        }
        public SomeAppSpecificException(string internalMessage, string externalMessage) 
            : base(internalMessage, externalMessage, new BadRequestObjectResult(externalMessage))
        {
        }
    }
}
