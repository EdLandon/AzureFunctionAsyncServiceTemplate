using Azure.Messaging.ServiceBus;
using System.Text;
using System.Text.Json;

namespace ServiceLib
{
    public abstract class QueuePayloadModelProcessor
    {
        public abstract object Process(ServiceBusReceivedMessage msg);

        public void Validate(ServiceBusReceivedMessage msg)
        {
            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }
            if( msg.Body == null)
            {
                throw new NullReferenceException(nameof(msg.Body));
            }
            if( msg.Subject == null)
            {
                throw new ArgumentNullException("Subject");
            }
            if (msg.ApplicationProperties["FQEN"] == null)
            {
                throw new ArgumentNullException("FQEN");
            }
        }
        public TPayloadModelIn Deserialise<TPayloadModelIn>(BinaryData o)
        {
            return JsonSerializer.Deserialize<TPayloadModelIn>(Encoding.UTF8.GetString(o));
        }
    }
}
