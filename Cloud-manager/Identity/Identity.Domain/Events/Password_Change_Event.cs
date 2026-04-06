using System;

namespace Identity.Domain.Events;

public record Password_Change_Event(Guid UserId, string NewPassword);