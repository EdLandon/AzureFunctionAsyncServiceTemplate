using Azure.Messaging.ServiceBus;
using ServiceLib;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Service1
{
    public class OrangesProcessor : QueuePayloadModelProcessor
    {
        public override async Task<OrangesPayloadModelOut> ProcessAsync(ServiceBusReceivedMessage msg)
        {
            base.Validate(msg);
            OrangesPayloadModelIn payload = base.Deserialise<OrangesPayloadModelIn>(msg.Body);
            ValidateMessage(payload);
            return await ProcessMessage(payload);
        }

        private void ValidateMessage(OrangesPayloadModelIn msg)
        {
            Debug.WriteLine(msg.name);
        }

        private async Task<OrangesPayloadModelOut> ProcessMessage(OrangesPayloadModelIn msg)
        {
            return new OrangesPayloadModelOut();
        }
    }
}
