namespace Identity.Domain.Email;

public record EmailChangedDomainEvent(Guid UserId, Email NewEmail);