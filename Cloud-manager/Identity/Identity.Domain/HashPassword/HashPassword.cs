namespace Identity.Domain.HashPassword;

public record HashPassword()
{
    public string Value { get; init; }
    
    private HashPassword(string value) : this() => Value = value;

    public static HashPassword CreateFromHash(string hash)
    {
        if (string.IsNullOrEmpty(hash))
        {
            throw new ArgumentException("Password can't be null or empty");
        }

        return new HashPassword();
    }
}