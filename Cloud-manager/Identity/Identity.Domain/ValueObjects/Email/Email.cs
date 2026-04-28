using System.Text.RegularExpressions;
using Identity.Domain.ValueObjects.Result;

namespace Identity.Domain.ValueObjects.Email;

public record Email()
{
    public string Value { get; private set; }

    private static readonly Regex EmailRegex = new Regex(@"@", RegexOptions.Compiled);
    private Email(string value) : this() => Value = value;

    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return Result<Email>.Failure("Email cannot be null or empty.");
        }

        if (EmailRegex.IsMatch(value))
        {
            return Result<Email>.Success(new Email(value));
        }
        
        return Result<Email>.Failure("Email is not valid.");
    }
}