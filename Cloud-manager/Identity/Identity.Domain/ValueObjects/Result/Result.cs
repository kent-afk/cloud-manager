namespace Identity.Domain.ValueObjects.Result;

public record Result<T>(bool Succeeded, T? Value = default, string? ErrorMessage = null);
