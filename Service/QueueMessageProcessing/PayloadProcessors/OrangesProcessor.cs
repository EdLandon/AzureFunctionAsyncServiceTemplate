using Azure.Messaging.ServiceBus;
using ServiceLib;
using System.Diagnostics;

namespace Service1
{
    public class OrangesProcessor : QueuePayloadModelProcessor
    {
        public override OrangesPayloadModelOut Process(ServiceBusReceivedMessage msg)
        {
            base.Validate(msg);
            OrangesPayloadModelIn payload = base.Deserialise<OrangesPayloadModelIn>(msg.Body);
            ValidateMessage(payload);
            return ProcessMessage(payload);
        }

        private void ValidateMessage(OrangesPayloadModelIn msg)
        {
            Debug.WriteLine(msg.name);
        }

        private OrangesPayloadModelOut ProcessMessage(OrangesPayloadModelIn msg)
        {
            return new OrangesPayloadModelOut();
        }
    }
}
