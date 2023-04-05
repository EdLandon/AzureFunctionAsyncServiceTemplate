using Azure.Messaging.ServiceBus;
using ServiceLib;
using System.Diagnostics;

namespace Service1
{
    public class ApplesProcessor : QueuePayloadModelProcessor
    {
        public override ApplesPayloadModelOut Process(ServiceBusReceivedMessage msg)
        {
            base.Validate(msg);
            ApplesPayloadModelIn payload = base.Deserialise<ApplesPayloadModelIn>(msg.Body);
            ValidateMessage(payload);
            return ProcessMessage(payload);
        }

        private void ValidateMessage(ApplesPayloadModelIn msg)
        {
            Debug.WriteLine(msg.name);
        }

        private ApplesPayloadModelOut ProcessMessage(ApplesPayloadModelIn msg)
        {
            return new ApplesPayloadModelOut();
        }
    }
}
