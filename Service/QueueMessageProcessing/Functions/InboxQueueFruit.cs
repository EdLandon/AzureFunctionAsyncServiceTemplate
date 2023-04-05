using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServerlessLib;

namespace Service1.Functions
{
    public class InboxQueueFruitFunc : QueueServerlessBase<InboxQueueFruitFunc>
    {
        public InboxQueueFruitFunc(ILogger<InboxQueueFruitFunc> log, IConfiguration configuration)
        {
            _log = log;
            _config = configuration;
        }

        [FunctionName(nameof(InboxQueueFruitFuncRun))]
        public void InboxQueueFruitFuncRun(
            [ServiceBusTrigger(queueName: "%requests:inboxQueue%", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage inMsg, ServiceBusMessageActions messageActions,
            [ServiceBus("%requests:outboxTopic%", ServiceBusEntityType.Topic, Connection = "ServiceBusConnection")] 
            out ServiceBusMessage outMsg)
        {
            outMsg = base.ProcessInboxMessage(inMsg, messageActions);
        }
    }
}
