using System.Text.RegularExpressions;
using SasiadMa.Core.Common;

namespace SasiadMa.Core.ValueObjects;

public record Email
{
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled);

    public string Value { get; init; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Error.Validation(nameof(Email), "Email cannot be empty");
        }

        if (!EmailRegex.IsMatch(email))
        {
            return Error.Validation(nameof(Email), "Invalid email format");
        }

        return new Email(email.ToLowerInvariant());
    }

    public static implicit operator string(Email email) => email.Value;
}
