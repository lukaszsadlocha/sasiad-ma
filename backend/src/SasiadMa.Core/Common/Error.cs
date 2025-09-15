namespace SasiadMa.Core.Common;

public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided");
    
    public static Error NotFound(string entityName, object id) =>
        new($"{entityName}.NotFound", $"{entityName} with id {id} was not found");
    
    public static Error Validation(string propertyName, string message) =>
        new($"Validation.{propertyName}", message);
    
    public static Error Conflict(string message) =>
        new("Error.Conflict", message);
    
    public static Error Unauthorized(string message = "Unauthorized access") =>
        new("Error.Unauthorized", message);
    
    public static Error Forbidden(string message = "Forbidden access") =>
        new("Error.Forbidden", message);
}
