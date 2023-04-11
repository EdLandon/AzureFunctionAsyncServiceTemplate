using Microsoft.AspNetCore.Mvc;
using System;
using ServerlessLib;

namespace Service1
{
    // 400
    public class LookupLimitReachedException: MBHttpException
    {
        public LookupLimitReachedException(string message) :base(message, new BadRequestObjectResult(message) )
        {
        }
        public LookupLimitReachedException(string internalMessage, string externalMessage) : base(internalMessage, externalMessage, new BadRequestObjectResult(externalMessage))
        {
        }
    }
}
