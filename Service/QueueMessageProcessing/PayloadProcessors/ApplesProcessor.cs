using Azure.Messaging.ServiceBus;
using ServiceLib;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Service1
{
    public class ApplesProcessor : QueuePayloadModelProcessor
    {
        private IService1 _service1;

        ApplesProcessor(IService1 service1)
        {
            _service1 = service1;
        }

        public override async Task<ApplesPayloadModelOut> ProcessAsync(ServiceBusReceivedMessage msg)
        {
            base.Validate(msg);
            ApplesPayloadModelIn payload = base.Deserialise<ApplesPayloadModelIn>(msg.Body);
            ValidateMessage(payload);
            return await ProcessMessage(payload);
        }

        private void ValidateMessage(ApplesPayloadModelIn msg)
        {
            Debug.WriteLine(msg.name);
        }

        private async Task<ApplesPayloadModelOut> ProcessMessage(ApplesPayloadModelIn msg)
        {
            return await _service1.CallSomeService("someUrlSearchString");
        }
    }
}
