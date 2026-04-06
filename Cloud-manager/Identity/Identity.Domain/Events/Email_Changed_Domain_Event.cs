using Identity.Domain.ValueObjects.Email;

namespace Identity.Domain.Events;

public record Email_Changed_Domain_Event(System.Guid UserId, Email NewEmail);