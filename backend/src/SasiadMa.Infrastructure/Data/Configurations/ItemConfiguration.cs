using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SasiadMa.Core.Entities;

namespace SasiadMa.Infrastructure.Data.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Items");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.OwnsOne(i => i.Category, category =>
        {
            category.Property(c => c.Code)
                .HasColumnName("CategoryCode")
                .IsRequired()
                .HasMaxLength(50);
        });

        builder.OwnsOne(i => i.Condition, condition =>
        {
            condition.Property(c => c.Code)
                .HasColumnName("ConditionCode")
                .IsRequired()
                .HasMaxLength(50);
        });

        builder.Property(i => i.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(i => i.Brand)
            .HasMaxLength(50);

        builder.Property(i => i.Model)
            .HasMaxLength(50);

        builder.Property(i => i.UsageInstructions)
            .HasMaxLength(2000);

        builder.Property(i => i.RequiresDeposit)
            .HasDefaultValue(false);

        builder.Property(i => i.MaxBorrowDays)
            .HasDefaultValue(7);

        builder.Property(i => i.IsActive)
            .HasDefaultValue(true);

        builder.Property(i => i.CreatedAt)
            .IsRequired();

        builder.Property(i => i.UpdatedAt)
            .IsRequired();

        // Relationships
        builder.HasOne(i => i.Owner)
            .WithMany(u => u.OwnedItems)
            .HasForeignKey(i => i.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.Community)
            .WithMany(c => c.Items)
            .HasForeignKey(i => i.CommunityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ItemImageConfiguration : IEntityTypeConfiguration<ItemImage>
{
    public void Configure(EntityTypeBuilder<ItemImage> builder)
    {
        builder.ToTable("ItemImages");

        builder.HasKey(ii => ii.Id);

        builder.Property(ii => ii.Url)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(ii => ii.Alt)
            .HasMaxLength(200);

        builder.Property(ii => ii.IsPrimary)
            .HasDefaultValue(false);

        builder.Property(ii => ii.Order)
            .HasDefaultValue(0);

        builder.Property(ii => ii.CloudinaryPublicId)
            .HasMaxLength(100);

        builder.Property(ii => ii.CreatedAt)
            .IsRequired();

        builder.Property(ii => ii.UpdatedAt)
            .IsRequired();

        // Relationships
        builder.HasOne(ii => ii.Item)
            .WithMany(i => i.Images)
            .HasForeignKey(ii => ii.ItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}