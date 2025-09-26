using System.Text.RegularExpressions;
using FluentResults;

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
            return Result.Fail("Email cannot be empty");
        }

        if (!EmailRegex.IsMatch(email))
        {
            return Result.Fail("Invalid email format");
        }

        return new Email(email.ToLowerInvariant());
    }

    public static implicit operator string(Email email) => email.Value;
}
