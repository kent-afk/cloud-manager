using System.Text.RegularExpressions;

namespace Identity.Domain.Email;

public record Email()
{
    public string Value { get; init; }

    private static readonly Regex EmailRegex = new Regex(@"@", RegexOptions.Compiled);
    private Email(string value) : this() => Value = value;

    public static Email Create(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException("Email can't be null or empty");
        }

        if (EmailRegex.IsMatch(value))
        {
            return new Email(value);
        }
        throw new ArgumentException("Invalid email format");
    }
}