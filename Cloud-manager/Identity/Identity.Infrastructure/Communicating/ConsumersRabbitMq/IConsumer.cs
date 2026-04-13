using RabbitMQ.Client.Events;

namespace Identity.Infrastructure.Communicating.ConsumersRabbitMq;

public interface IConsumer
{
    Task ConsumeAsync(BasicDeliverEventArgs @event);
}