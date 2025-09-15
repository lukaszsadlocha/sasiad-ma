using SasiadMa.Core.Common;

namespace SasiadMa.Core.ValueObjects;

public record ItemCondition
{
    public static readonly ItemCondition New = new("new", "Like New", 5);
    public static readonly ItemCondition Excellent = new("excellent", "Excellent", 4);
    public static readonly ItemCondition Good = new("good", "Good", 3);
    public static readonly ItemCondition Fair = new("fair", "Fair", 2);
    public static readonly ItemCondition Poor = new("poor", "Poor", 1);

    public string Code { get; init; }
    public string Name { get; init; }
    public int Score { get; init; }

    private ItemCondition(string code, string name, int score)
    {
        Code = code;
        Name = name;
        Score = score;
    }

    public static Result<ItemCondition> Create(string code)
    {
        var condition = GetByCode(code);
        if (condition == null)
        {
            return Error.Validation(nameof(ItemCondition), $"Invalid condition code: {code}");
        }

        return condition;
    }

    public static ItemCondition? GetByCode(string code)
    {
        return GetAll().FirstOrDefault(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
    }

    public static IEnumerable<ItemCondition> GetAll()
    {
        yield return New;
        yield return Excellent;
        yield return Good;
        yield return Fair;
        yield return Poor;
    }
}
