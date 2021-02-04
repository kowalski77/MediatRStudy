using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport;
using RabbitMQ.Client;

namespace Publisher.App.Infrastructure
{
    public sealed class MessagePublisher : IMessagePublisher
    {
        private readonly IBusControl busControl;

        public MessagePublisher(IBusControl busControl)
        {
            this.busControl = busControl ?? throw new ArgumentNullException(nameof(busControl));
        }

        public Task PublishAsync<T>(T message)
            where T : class
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var routingKey = string.Concat("Study", ".", typeof(T).Name);

            return this.busControl.Publish(message, message.GetType(), context =>
            {
                if (!context.TryGetPayload<RabbitMqSendContext>(out var rabbitMqSendContext))
                {
                    throw new ArgumentException(
                        $"The current publish context does not have a {nameof(RabbitMqSendContext)} when trying to serialize {typeof(T).FullName}");
                }

                rabbitMqSendContext.BasicProperties.Type = typeof(T).FullName;
                rabbitMqSendContext.BasicProperties.Timestamp =
                    new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                context.SetRoutingKey(routingKey);
            });
        }
    }
}