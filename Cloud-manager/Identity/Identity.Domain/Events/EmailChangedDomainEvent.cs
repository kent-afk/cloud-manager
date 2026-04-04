using Identity.Domain.ValueObjects.Email;

namespace Identity.Domain.Events;

public record EmailChangedDomainEvent(System.Guid UserId, Email NewEmail);