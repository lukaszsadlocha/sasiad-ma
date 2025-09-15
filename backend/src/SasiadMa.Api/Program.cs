var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS for frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder.WithOrigins("http://localhost:3001", "http://localhost:3000")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

// Sample endpoints for demo
app.MapGet("/", () => "Sasiad-Ma API is running!");

// Mock Sasiad-Ma endpoints for demo
app.MapGet("/api/communities", () =>
{
    return new[]
    {
        new { Id = "1", Name = "Oak Street Neighborhood", Description = "Friendly neighbors sharing tools and equipment", MemberCount = 15 },
        new { Id = "2", Name = "Downtown Austin Community", Description = "Urban community for apartment dwellers", MemberCount = 23 },
        new { Id = "3", Name = "Sunset Hills", Description = "Family-oriented suburban community", MemberCount = 8 }
    };
}).WithTags("Communities");

app.MapGet("/api/items", () =>
{
    return new[]
    {
        new { Id = "1", Name = "Power Drill", Description = "Cordless power drill with bits", Category = "Tools", Status = "Available", OwnerName = "John Doe" },
        new { Id = "2", Name = "Lawn Mower", Description = "Electric lawn mower, perfect for small yards", Category = "Garden", Status = "Borrowed", OwnerName = "Jane Smith" },
        new { Id = "3", Name = "Baby Stroller", Description = "Lightweight stroller for infants", Category = "Baby", Status = "Available", OwnerName = "Mike Johnson" },
        new { Id = "4", Name = "Ladder", Description = "6-foot step ladder", Category = "Tools", Status = "Available", OwnerName = "Sarah Wilson" }
    };
}).WithTags("Items");

app.MapPost("/api/auth/login", (object loginRequest) =>
{
    return new
    {
        AccessToken = "mock-jwt-token",
        RefreshToken = "mock-refresh-token",
        ExpiresAt = DateTime.UtcNow.AddHours(1),
        User = new
        {
            Id = "1",
            FirstName = "Demo",
            LastName = "User",
            Email = "demo@sasiadma.com",
            ProfileImageUrl = "",
            Role = "User",
            IsEmailConfirmed = true
        }
    };
}).WithTags("Auth");

app.MapGet("/api/notifications", () =>
{
    return new[]
    {
        new { Id = "1", Title = "Borrow Request", Message = "John wants to borrow your power drill", Type = "BORROW_REQUEST", CreatedAt = DateTime.UtcNow.AddMinutes(-30), IsRead = false },
        new { Id = "2", Title = "Item Returned", Message = "Your lawn mower has been returned", Type = "ITEM_RETURNED", CreatedAt = DateTime.UtcNow.AddHours(-2), IsRead = true },
        new { Id = "3", Title = "Welcome!", Message = "Welcome to Sasiad-Ma community!", Type = "COMMUNITY_INVITE", CreatedAt = DateTime.UtcNow.AddDays(-1), IsRead = false }
    };
}).WithTags("Notifications");

app.Run();

