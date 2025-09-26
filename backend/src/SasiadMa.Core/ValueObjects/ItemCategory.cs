using FluentResults;

namespace SasiadMa.Core.ValueObjects;

public record ItemCategory
{
    public static readonly ItemCategory Tools = new("tools", "Tools");
    public static readonly ItemCategory Electronics = new("electronics", "Electronics");
    public static readonly ItemCategory Sports = new("sports", "Sports & Recreation");
    public static readonly ItemCategory Garden = new("garden", "Garden & Outdoor");
    public static readonly ItemCategory Kitchen = new("kitchen", "Kitchen & Dining");
    public static readonly ItemCategory Baby = new("baby", "Baby & Kids");
    public static readonly ItemCategory Books = new("books", "Books & Media");
    public static readonly ItemCategory Furniture = new("furniture", "Furniture");
    public static readonly ItemCategory Clothing = new("clothing", "Clothing & Accessories");
    public static readonly ItemCategory Other = new("other", "Other");

    public string Code { get; init; }
    public string Name { get; init; }

    private ItemCategory(string code, string name)
    {
        Code = code;
        Name = name;
    }

    public static Result<ItemCategory> Create(string code)
    {
        var category = GetByCode(code);
        if (category == null)
        {
            return Result.Fail($"Invalid category code: {code}");
        }

        return category;
    }

    public static ItemCategory? GetByCode(string code)
    {
        return GetAll().FirstOrDefault(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
    }
    
    public static bool TryGetByCode(string code, out ItemCategory? category)
    {
        category = GetByCode(code);
        return category != null;
    }

    public static IEnumerable<ItemCategory> GetAll()
    {
        yield return Tools;
        yield return Electronics;
        yield return Sports;
        yield return Garden;
        yield return Kitchen;
        yield return Baby;
        yield return Books;
        yield return Furniture;
        yield return Clothing;
        yield return Other;
    }
}
