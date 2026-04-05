using System.Threading.Tasks;

namespace Identity.Domain.Events.Publisher;

public interface IEventPublisher
{
    Task EmailChangeAsync(EmailChangedDomainEvent @event);
    Task PasswordChangeAsync(PasswordChangeEvent @event);
    Task UserRegisteredAsync(UserRegisteredEvent @event);
}