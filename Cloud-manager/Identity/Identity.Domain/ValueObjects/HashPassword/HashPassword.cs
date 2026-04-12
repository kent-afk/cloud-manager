namespace Identity.Domain.ValueObjects.HashPassword;

public record HashPassword
{
    public string Value { get; private init; }
    
    private HashPassword(string value) => Value = value;
    
    public static HashPassword Create(string hash)
    {
        if (string.IsNullOrEmpty(hash))
        {
            throw new System.ArgumentException("Password can't be null or empty");
        }

        return new HashPassword(hash);
    }
}