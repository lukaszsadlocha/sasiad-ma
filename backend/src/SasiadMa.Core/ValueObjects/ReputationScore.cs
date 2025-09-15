using SasiadMa.Core.Common;

namespace SasiadMa.Core.ValueObjects;

public record ReputationScore
{
    public int Value { get; init; }
    public int TotalTransactions { get; init; }
    public double Average => TotalTransactions > 0 ? (double)Value / TotalTransactions : 0;

    private ReputationScore(int value, int totalTransactions)
    {
        Value = value;
        TotalTransactions = totalTransactions;
    }

    public static Result<ReputationScore> Create(int value, int totalTransactions)
    {
        if (totalTransactions < 0)
        {
            return Error.Validation(nameof(ReputationScore), "Total transactions cannot be negative");
        }

        if (value < 0)
        {
            return Error.Validation(nameof(ReputationScore), "Reputation value cannot be negative");
        }

        if (value > totalTransactions * 5) // Max 5 points per transaction
        {
            return Error.Validation(nameof(ReputationScore), "Reputation value exceeds maximum possible");
        }

        return new ReputationScore(value, totalTransactions);
    }

    public static ReputationScore Initial() => new(0, 0);

    public ReputationScore AddTransaction(int rating)
    {
        if (rating < 1 || rating > 5)
            return this; // Invalid rating, don't change

        return new ReputationScore(Value + rating, TotalTransactions + 1);
    }

    public string GetReputation()
    {
        var avg = Average;
        return avg switch
        {
            >= 4.5 => "Excellent",
            >= 4.0 => "Very Good",
            >= 3.5 => "Good",
            >= 3.0 => "Fair",
            >= 2.0 => "Poor",
            _ => "New User"
        };
    }
}
