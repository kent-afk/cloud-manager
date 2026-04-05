using System;

namespace Identity.Domain.Events;

public record PasswordChangeEvent(Guid UserId, string NewPassword);