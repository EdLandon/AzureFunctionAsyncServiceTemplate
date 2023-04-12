using Autofac;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceLib;

namespace ServerlessLib
{
    public class QueueServerlessBase<TFunc>:QueueBase
    {
        protected ILogger<TFunc> _log;
        protected IConfiguration _config;

        public ServiceBusMessage ProcessInboxMessage(ServiceBusReceivedMessage msg, ServiceBusMessageActions messageActions)
        {
            ServiceBusMessage retVal = null;
            try
            {
                string fullyQualifiedEventName = GetFQEN(msg);
                _log.LogInformation($"FQEN: {fullyQualifiedEventName}, MsgId: {msg}.");
                object payload = ProcessMessage(msg, fullyQualifiedEventName);
                Tuple<string, string> outEventNames = GetOutEventNames(fullyQualifiedEventName);
                QueueMessage(out retVal, payload, outEventNames.Item1, outEventNames.Item2);
            }
            catch (TransientException)
            {
                throw; // Service bus handles retries ...
            }
            catch (Exception ex)
            {
                string desc = $"Source: {ex.TargetSite}, Message: {ex.Message}.";
                messageActions.DeadLetterMessageAsync(msg, "Failed to deserialise message", desc);
            }
            return retVal;
        }

        private string GetFQEN(ServiceBusReceivedMessage msg)
        {
            string fullyQualifiedEventName = msg.ApplicationProperties["FQEN"].ToString();
            if (string.IsNullOrEmpty(fullyQualifiedEventName))
                throw new InvalidOperationException("Missing FQEN from message header.");
            return fullyQualifiedEventName;
        }

        private object ProcessMessage(ServiceBusReceivedMessage msg, string fqen)
        {
            QueuePayloadModelProcessor processor = ResolveNamedReg(fqen) as QueuePayloadModelProcessor;
            return processor.ProcessAsync(msg);
        }

        protected QueuePayloadModelProcessor ResolveNamedReg(string key)
        {
             return ContainerProvider.Container.ResolveNamed<QueuePayloadModelProcessor>(key);
        }

        private Tuple<string,string> GetOutEventNames(string fqen)
        {
            IConfigurationSection section = _config.GetSection("requests").GetChildren().FirstOrDefault(s => s["inboxFQEN"] == fqen);
            string fqenOut = section["outboxFQEN"];
            string eventShortName = fqenOut.Split('_')[3];
            return new Tuple<String, string>(fqenOut, eventShortName);
        }
    }
}