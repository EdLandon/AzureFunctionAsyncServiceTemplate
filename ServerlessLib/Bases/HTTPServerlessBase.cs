using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ServerlessLib
{
    public abstract class HTTPServerlessBase<TPayloadModel, TFunc>:QueueBase
    {
		protected HttpRequest _request;
        protected TPayloadModel _deSerialisedModel;
        protected string _fqen;
        protected string _eventShortName;

        public ILogger<TFunc> _log;
        public IConfiguration _config;


        public IActionResult ProcessRequest(HttpRequest req, string fqen, out ServiceBusMessage msg)
        {
            msg = null; _request = req; _fqen = fqen;

            try
            {
                SetEventShortName();
                DeserialiseRequest();
                ValidateRequest();
                QueueMessage(out msg, _deSerialisedModel, _fqen, _eventShortName);
                LogRequest(msg);
            }
            catch (Exception ex)
            {
                _log.LogError($"FQEN: {_fqen}, MsgId: {msg}, Topic: {_config.GetValue<string>("topic")}, Error: {ex.Message}.");
                return new BadRequestObjectResult("Useful message here!"); // 400 bad request
            }
            return new OkObjectResult($"Request payload {_deSerialisedModel.GetType().Name} successfully validated and queued.");
        }

        private void SetEventShortName()
        {
            if (_fqen == null) 
                throw new InvalidOperationException("Missing FQEN.");
            if ( ! _fqen.Contains("_") ) 
                throw new InvalidOperationException($"{_fqen} is an invalid FQEN. Must be underscore delimited: 'Domain_MX_EventShortName_Verson_Event'.");

            var parts = _fqen.Split('_');
            if ( parts.Length<5 )
                throw new InvalidOperationException($"{_fqen} is an invalid FQEN. Must be 5-part: 'Domain_MX_EventShortName_Verson_Event'.");

            _eventShortName = parts[3];
        }
        public void DeserialiseRequest()
		{
			string request = null;
			using ( StreamReader sr = new StreamReader(_request.Body) )
                request = sr.ReadToEnd();
			if (string.IsNullOrEmpty(request))
				throw new ArgumentException( $"{nameof(request)} - empty string" );

            _deSerialisedModel = JsonSerializer.Deserialize<TPayloadModel>(request);
			if (_deSerialisedModel == null)
                throw new ArgumentException($"{nameof(request)} - invalid string");
		}
        public virtual void ValidateRequest()
        {
            bool valid = true;
            if (!valid) throw new ApplicationException("Invalid request.");
        }
		public virtual void LogRequest(ServiceBusMessage msg)
		{
            _log.LogInformation($"FQEN: {_fqen}, MsgId: {msg}, Topic: {_config.GetValue<string>("topic")}.");
        }
    }
}
