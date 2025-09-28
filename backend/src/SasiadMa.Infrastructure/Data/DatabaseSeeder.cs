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

        // Create seed items
        var items = CreateSeedItems(users, communities);
        context.Items.AddRange(items);

        await context.SaveChangesAsync();

        // Create seed item images
        var itemImages = CreateSeedItemImages(items);
        context.Set<Core.Entities.ItemImage>().AddRange(itemImages);

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

    private static List<Item> CreateSeedItems(List<User> users, List<Community> communities)
    {
        var items = new List<Item>();

        if (!users.Any() || !communities.Any()) return items;

        var mainCommunity = communities.First(c => c.InvitationCode == "PRZYKLAD");
        var lukasz = users.First(u => u.Email.Value == "lukaszsadlocha@gmail.com");
        var anna = users.FirstOrDefault(u => u.Email.Value == "anna.kowalska@example.com");
        var jan = users.FirstOrDefault(u => u.Email.Value == "jan.nowak@example.com");
        var maria = users.FirstOrDefault(u => u.Email.Value == "maria.wisniewska@example.com");

        // Items from Lukasz
        var lukaszItems = new[]
        {
            new { Id = new Guid("10000000-0000-0000-0000-000000000001"), Name = "Wiertarka udarowa Bosch", Description = "Profesjonalna wiertarka udarowa Bosch Professional GSB 13 RE. Używana kilka razy, w doskonałym stanie. Idealna do wiercenia w betonie, kamieniu i drewnie. Dołączam zestaw wierteł.", Category = ItemCategory.GetByCode("Tools"), Condition = ItemCondition.GetByCode("Excellent") },
            new { Id = new Guid("10000000-0000-0000-0000-000000000002"), Name = "Dmuchawa ogrodowa", Description = "Elektryczna dmuchawa do liści. Moc 2500W, regulowana prędkość nadmuchu. Bardzo przydatna jesienią do sprzątania ogrodu. Lekka i wygodna w użyciu.", Category = ItemCategory.GetByCode("Garden"), Condition = ItemCondition.GetByCode("Good") },
            new { Id = new Guid("10000000-0000-0000-0000-000000000003"), Name = "Robot kuchenny Thermomix", Description = "Thermomix TM6 - najnowszy model robota kuchennego Vorwerk. Używany okazjonalnie, stan bardzo dobry. Potrafi gotować, mieszać, siekać, emulgować i wiele więcej. Dołączam książkę przepisów.", Category = ItemCategory.GetByCode("Kitchen"), Condition = ItemCondition.GetByCode("Excellent") }
        };

        foreach (var itemData in lukaszItems)
        {
            var item = new Item
            {
                Id = itemData.Id,
                Name = itemData.Name,
                Description = itemData.Description,
                Category = itemData.Category,
                Condition = itemData.Condition,
                Status = ItemStatus.Available,
                OwnerId = lukasz.Id,
                CommunityId = mainCommunity.Id,
                IsActive = true,
                MaxBorrowDays = 7,
                RequiresDeposit = false,
                CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 30)),
                UpdatedAt = DateTime.UtcNow
            };
            items.Add(item);
        }

        // Items from Anna
        if (anna != null)
        {
            var annaItems = new[]
            {
                new { Id = new Guid("20000000-0000-0000-0000-000000000001"), Name = "Rowerek dla dziecka", Description = "Rowerek 16\" dla dzieci w wieku 4-6 lat. Stan bardzo dobry, regularnie serwisowany. Czerwono-biały, z koszyczkiem z przodu. Idealny na naukę jazdy.", Category = ItemCategory.GetByCode("Toys"), Condition = ItemCondition.GetByCode("Good") },
                new { Id = new Guid("20000000-0000-0000-0000-000000000002"), Name = "Ekspres do kawy DeLonghi", Description = "Automatyczny ekspres do kawy DeLonghi Magnifica S. Świeżo po przeglądzie, doskonała kawa każdego ranka. Wbudowany młynek, regulacja mocy kawy.", Category = ItemCategory.GetByCode("Kitchen"), Condition = ItemCondition.GetByCode("Excellent") }
            };

            foreach (var itemData in annaItems)
            {
                var item = new Item
                {
                    Id = itemData.Id,
                    Name = itemData.Name,
                    Description = itemData.Description,
                    Category = itemData.Category,
                    Condition = itemData.Condition,
                    Status = ItemStatus.Available,
                    OwnerId = anna.Id,
                    CommunityId = mainCommunity.Id,
                    IsActive = true,
                    MaxBorrowDays = 5,
                    RequiresDeposit = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 20)),
                    UpdatedAt = DateTime.UtcNow
                };
                items.Add(item);
            }
        }

        // Items from Jan
        if (jan != null)
        {
            var janItems = new[]
            {
                new { Id = new Guid("30000000-0000-0000-0000-000000000001"), Name = "Rakiety tenisowe", Description = "Zestaw 2 rakiet tenisowych Wilson Pro Staff. Używane na korcie, ale w dobrym stanie. Idealny na weekendową grę z przyjaciółmi. Dołączam piłki.", Category = ItemCategory.GetByCode("Sports"), Condition = ItemCondition.GetByCode("Good") },
                new { Id = new Guid("30000000-0000-0000-0000-000000000002"), Name = "Laptop Lenovo ThinkPad", Description = "Laptop Lenovo ThinkPad T490. Core i5, 8GB RAM, 256GB SSD. Stan bardzo dobry, bateria trzyma cały dzień. Idealny do pracy zdalnej lub nauki.", Category = ItemCategory.GetByCode("Electronics"), Condition = ItemCondition.GetByCode("Excellent") },
                new { Id = new Guid("30000000-0000-0000-0000-000000000003"), Name = "Kosiarka elektryczna", Description = "Elektryczna kosiarka do trawy Bosch Rotak 37. Szerokość koszenia 37cm, regulowana wysokość cięcia. Bardzo cicha i ekologiczna. Stan bardzo dobry.", Category = ItemCategory.GetByCode("Garden"), Condition = ItemCondition.GetByCode("Good") }
            };

            foreach (var itemData in janItems)
            {
                var item = new Item
                {
                    Id = itemData.Id,
                    Name = itemData.Name,
                    Description = itemData.Description,
                    Category = itemData.Category,
                    Condition = itemData.Condition,
                    Status = Random.Shared.Next(0, 5) == 0 ? ItemStatus.Borrowed : ItemStatus.Available, // 20% chance borrowed
                    OwnerId = jan.Id,
                    CommunityId = mainCommunity.Id,
                    IsActive = true,
                    MaxBorrowDays = 10,
                    RequiresDeposit = itemData.Name.Contains("Laptop"), // Require deposit for expensive items
                    DepositAmount = itemData.Name.Contains("Laptop") ? 1000 : null,
                    CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(5, 45)),
                    UpdatedAt = DateTime.UtcNow
                };
                items.Add(item);
            }
        }

        // Items from Maria
        if (maria != null)
        {
            var mariaItems = new[]
            {
                new { Id = new Guid("40000000-0000-0000-0000-000000000001"), Name = "Książki kucharskie", Description = "Zestaw 5 książek kucharskich: kuchnia włoska, francuska, azjatycka, wegetariańska i desery. Wszystkie w języku polskim, stan doskonały.", Category = ItemCategory.GetByCode("Books"), Condition = ItemCondition.GetByCode("Excellent") },
                new { Id = new Guid("40000000-0000-0000-0000-000000000002"), Name = "Fotel wypoczynkowy", Description = "Wygodny fotel wypoczynkowy w kolorze szarym. Mechanizm rozkładania, regulowany zagłówek. Idealny do czytania książek. Stan bardzo dobry.", Category = ItemCategory.GetByCode("Furniture"), Condition = ItemCondition.GetByCode("Good") },
                new { Id = new Guid("40000000-0000-0000-0000-000000000003"), Name = "Sukienki wizytowe", Description = "Kolekcja eleganckich sukienek w rozmiarze M (38). Różne kolory i style, noszone tylko na specjalne okazje. Idealne na wesela, komunie, imprezy biznesowe.", Category = ItemCategory.GetByCode("Clothing"), Condition = ItemCondition.GetByCode("Excellent") }
            };

            foreach (var itemData in mariaItems)
            {
                var item = new Item
                {
                    Id = itemData.Id,
                    Name = itemData.Name,
                    Description = itemData.Description,
                    Category = itemData.Category,
                    Condition = itemData.Condition,
                    Status = ItemStatus.Available,
                    OwnerId = maria.Id,
                    CommunityId = mainCommunity.Id,
                    IsActive = true,
                    MaxBorrowDays = itemData.Name.Contains("Książki") ? 14 : 3, // Books for longer
                    RequiresDeposit = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 60)),
                    UpdatedAt = DateTime.UtcNow
                };
                items.Add(item);
            }
        }

        return items;
    }

    private static List<Core.Entities.ItemImage> CreateSeedItemImages(List<Core.Entities.Item> items)
    {
        var itemImages = new List<Core.Entities.ItemImage>();

        foreach (var item in items)
        {
            var categoryCode = item.Category?.Code ?? "Other";
            var imageUrl = GetPlaceholderImageUrl(item.Name, categoryCode);
            var itemImage = new Core.Entities.ItemImage
            {
                Id = Guid.NewGuid(),
                ItemId = item.Id,
                Url = imageUrl,
                Alt = $"Image of {item.Name}",
                IsPrimary = true,
                Order = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            itemImages.Add(itemImage);
        }

        return itemImages;
    }

    private static string GetPlaceholderImageUrl(string itemName, string category)
    {
        // Use high-quality placeholder images from Unsplash based on item category and name
        return category.ToLower() switch
        {
            "tools" when itemName.Contains("Wiertarka") => "https://images.unsplash.com/photo-1588075592446-265fd1e6e76f?w=400&h=300&fit=crop&crop=center&auto=format",
            "tools" when itemName.Contains("Dmuchawa") => "https://images.unsplash.com/photo-1416879595882-3373a0480b5b?w=400&h=300&fit=crop&crop=center&auto=format",
            "kitchen" when itemName.Contains("Thermomix") => "https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?w=400&h=300&fit=crop&crop=center&auto=format",
            "kitchen" when itemName.Contains("Ekspres") => "https://images.unsplash.com/photo-1497935586351-b67a49e012bf?w=400&h=300&fit=crop&crop=center&auto=format",
            "toys" when itemName.Contains("Rowerek") => "https://images.unsplash.com/photo-1502744688674-c619d1586c9e?w=400&h=300&fit=crop&crop=center&auto=format",
            "sports" when itemName.Contains("Rakiety") => "https://images.unsplash.com/photo-1551698618-1dfe5d97d256?w=400&h=300&fit=crop&crop=center&auto=format",
            "electronics" when itemName.Contains("Laptop") => "https://images.unsplash.com/photo-1496181133206-80ce9b88a853?w=400&h=300&fit=crop&crop=center&auto=format",
            "garden" when itemName.Contains("Kosiarka") => "https://images.unsplash.com/photo-1558618047-3c8c76ca7d13?w=400&h=300&fit=crop&crop=center&auto=format",
            "books" when itemName.Contains("Książki") => "https://images.unsplash.com/photo-1481627834876-b7833e8f5570?w=400&h=300&fit=crop&crop=center&auto=format",
            "furniture" when itemName.Contains("Fotel") => "https://images.unsplash.com/photo-1506439773649-6e0eb8cfb237?w=400&h=300&fit=crop&crop=center&auto=format",
            "clothing" when itemName.Contains("Sukienki") => "https://images.unsplash.com/photo-1490481651871-ab68de25d43d?w=400&h=300&fit=crop&crop=center&auto=format",

            // Default fallback images by category
            "tools" => "https://images.unsplash.com/photo-1530587191325-3db32d826c18?w=400&h=300&fit=crop&crop=center&auto=format",
            "electronics" => "https://images.unsplash.com/photo-1526738549149-8e07eca6c147?w=400&h=300&fit=crop&crop=center&auto=format",
            "sports" => "https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b?w=400&h=300&fit=crop&crop=center&auto=format",
            "garden" => "https://images.unsplash.com/photo-1416879595882-3373a0480b5b?w=400&h=300&fit=crop&crop=center&auto=format",
            "kitchen" => "https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?w=400&h=300&fit=crop&crop=center&auto=format",
            "books" => "https://images.unsplash.com/photo-1481627834876-b7833e8f5570?w=400&h=300&fit=crop&crop=center&auto=format",
            "toys" => "https://images.unsplash.com/photo-1558060370-d644479cb6f7?w=400&h=300&fit=crop&crop=center&auto=format",
            "furniture" => "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?w=400&h=300&fit=crop&crop=center&auto=format",
            "clothing" => "https://images.unsplash.com/photo-1441986300917-64674bd600d8?w=400&h=300&fit=crop&crop=center&auto=format",
            _ => "https://images.unsplash.com/photo-1560472354-b33ff0c44a43?w=400&h=300&fit=crop&crop=center&auto=format" // Generic item
        };
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}