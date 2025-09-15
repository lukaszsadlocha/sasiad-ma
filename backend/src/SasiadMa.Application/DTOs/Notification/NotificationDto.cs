namespace SasiadMa.Application.DTOs.Notification;

public class NotificationDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Related entity info (optional)
    public Guid? RelatedEntityId { get; set; }
    public string? RelatedEntityType { get; set; }
    public Dictionary<string, object> Data { get; set; } = new();
}

public class CreateNotificationRequest
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public Guid? RelatedEntityId { get; set; }
    public string? RelatedEntityType { get; set; }
    public Dictionary<string, object> Data { get; set; } = new();
}
