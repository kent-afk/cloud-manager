using System.Threading.Tasks;

namespace Identity.Domain.Events.Publisher;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : class;
}