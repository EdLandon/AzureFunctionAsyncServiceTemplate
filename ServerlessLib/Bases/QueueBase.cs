using Azure.Messaging.ServiceBus;
using System.Text;
using System.Text.Json;

namespace ServerlessLib
{
    public abstract class QueueBase
    {
        public virtual void QueueMessage(out ServiceBusMessage msg, object payload, string fqen, string eventShortName)
        {
            msg = new ServiceBusMessage
            {
                MessageId = Guid.NewGuid().ToString(),
                Subject = eventShortName,
                Body = new BinaryData(
                    Encoding.UTF8.GetBytes(
                        JsonSerializer.Serialize(payload)
                    )
                )
            };
            msg.ApplicationProperties["FQEN"] = fqen;
        }
    }
}