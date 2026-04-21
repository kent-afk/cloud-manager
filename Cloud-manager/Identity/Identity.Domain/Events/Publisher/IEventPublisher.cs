namespace Identity.Domain.Events.Publisher;

public interface IEventPublisher
{
    Task PublishAsync(IEvent @event, CancellationToken cancellationToken);
}