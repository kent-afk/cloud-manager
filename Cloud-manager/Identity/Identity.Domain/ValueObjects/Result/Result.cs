namespace Identity.Domain.ValueObjects.Result;

public record Result<T>(bool Succeeded, T? Value = default, string? ErrorMessage = null)
{ 
    public bool IsFailure => !Succeeded;
    
    public static Result<T> Success(T value) => new(true, value);
    public static Result<T> Failure(string error) => new(false, ErrorMessage: error);
}
