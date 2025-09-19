using Microsoft.EntityFrameworkCore;
using SasiadMa.Core.Entities;
using SasiadMa.Core.Enums;
using SasiadMa.Core.ValueObjects;

namespace SasiadMa.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Ensure database is created (important for in-memory)
        await context.Database.EnsureCreatedAsync();

        // Check if users already exist
        if (await context.Users.AnyAsync())
        {
            return; // Database has been seeded
        }

        // Create seed users
        var users = await CreateSeedUsers();
        context.Users.AddRange(users);

        // Create seed communities
        var communities = CreateSeedCommunities();
        context.Communities.AddRange(communities);

        await context.SaveChangesAsync();

        // Create community memberships
        var memberships = CreateCommunityMemberships(users, communities);
        context.CommunityMembers.AddRange(memberships);

        await context.SaveChangesAsync();
    }

    private static Task<List<User>> CreateSeedUsers()
    {
        var users = new List<User>();

        // Create main user - Lukasz Sadłocha
        var lukaszId = new Guid("11111111-1111-1111-1111-111111111111");
        var lukaszEmail = Email.Create("lukaszsadlocha@gmail.com");
        if (lukaszEmail.IsSuccess)
        {
            var lukasz = new User
            {
                Id = lukaszId,
                FirstName = "Łukasz",
                LastName = "Sadłocha",
                Email = lukaszEmail.Value,
                PasswordHash = HashPassword("Password123!"), // Default password
                IsEmailConfirmed = true,
                Role = UserRole.SystemAdmin,
                Reputation = ReputationScore.Initial(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            users.Add(lukasz);
        }

        // Create a few additional test users
        var testUsers = new[]
        {
            new { Id = new Guid("22222222-2222-2222-2222-222222222222"), FirstName = "Anna", LastName = "Kowalska", Email = "anna.kowalska@example.com" },
            new { Id = new Guid("33333333-3333-3333-3333-333333333333"), FirstName = "Jan", LastName = "Nowak", Email = "jan.nowak@example.com" },
            new { Id = new Guid("44444444-4444-4444-4444-444444444444"), FirstName = "Maria", LastName = "Wiśniewska", Email = "maria.wisniewska@example.com" }
        };

        foreach (var userData in testUsers)
        {
            var email = Email.Create(userData.Email);
            if (email.IsSuccess)
            {
                var user = new User
                {
                    Id = userData.Id,
                    FirstName = userData.FirstName,
                    LastName = userData.LastName,
                    Email = email.Value,
                    PasswordHash = HashPassword("Password123!"),
                    IsEmailConfirmed = true,
                    Role = UserRole.User,
                    Reputation = ReputationScore.Initial(),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                users.Add(user);
            }
        }

        return Task.FromResult(users);
    }

    private static List<Community> CreateSeedCommunities()
    {
        var communities = new List<Community>();

        var mainCommunityId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var mainCommunity = new Community
        {
            Id = mainCommunityId,
            Name = "Osiedle Przykładowe",
            Description = "Wspólnota sąsiedzka dla mieszkańców osiedla przykładowego",
            InvitationCode = "PRZYKLAD",
            Address = "ul. Przykładowa 1",
            City = "Warszawa",
            PostalCode = "00-000",
            IsPublic = false,
            IsActive = true,
            MaxMembers = 100,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        communities.Add(mainCommunity);

        var testCommunityId = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        var testCommunity = new Community
        {
            Id = testCommunityId,
            Name = "Testowa Wspólnota",
            Description = "Wspólnota testowa dla celów demonstracyjnych",
            InvitationCode = "TEST2024",
            Address = "ul. Testowa 10",
            City = "Kraków",
            PostalCode = "30-000",
            IsPublic = true,
            IsActive = true,
            MaxMembers = 50,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        communities.Add(testCommunity);

        return communities;
    }

    private static List<CommunityMember> CreateCommunityMemberships(List<User> users, List<Community> communities)
    {
        var memberships = new List<CommunityMember>();

        if (users.Any() && communities.Any())
        {
            var lukasz = users.First(u => u.Email.Value == "lukaszsadlocha@gmail.com");
            var mainCommunity = communities.First(c => c.InvitationCode == "PRZYKLAD");

            // Make Lukasz admin of the main community
            var lukaszMembership = new CommunityMember
            {
                Id = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                UserId = lukasz.Id,
                CommunityId = mainCommunity.Id,
                IsAdmin = true,
                IsActive = true,
                JoinedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            memberships.Add(lukaszMembership);

            // Add other users as regular members
            var membershipIndex = 0;
            foreach (var user in users.Skip(1))
            {
                var membership = new CommunityMember
                {
                    Id = new Guid($"dddddddd-dddd-dddd-dddd-dddddddddd{membershipIndex:D2}"),
                    UserId = user.Id,
                    CommunityId = mainCommunity.Id,
                    IsAdmin = false,
                    IsActive = true,
                    JoinedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                memberships.Add(membership);
                membershipIndex++;
            }
        }

        return memberships;
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}