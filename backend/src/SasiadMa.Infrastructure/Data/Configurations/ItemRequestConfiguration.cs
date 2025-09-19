using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SasiadMa.Core.Entities;

namespace SasiadMa.Infrastructure.Data.Configurations;

public class ItemRequestConfiguration : IEntityTypeConfiguration<ItemRequest>
{
    public void Configure(EntityTypeBuilder<ItemRequest> builder)
    {
        builder.ToTable("ItemRequests");

        builder.HasKey(ir => ir.Id);

        builder.Property(ir => ir.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ir => ir.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.OwnsOne(ir => ir.Category, category =>
        {
            category.Property(c => c.Code)
                .HasColumnName("CategoryCode")
                .IsRequired()
                .HasMaxLength(50);
        });

        builder.Property(ir => ir.NeededBy)
            .IsRequired();

        builder.Property(ir => ir.MaxBorrowDays)
            .HasDefaultValue(7);

        builder.Property(ir => ir.IsUrgent)
            .HasDefaultValue(false);

        builder.Property(ir => ir.IsFulfilled)
            .HasDefaultValue(false);

        builder.Property(ir => ir.IsActive)
            .HasDefaultValue(true);

        builder.Property(ir => ir.RequesterId)
            .IsRequired();

        builder.Property(ir => ir.CommunityId)
            .IsRequired();

        builder.Property(ir => ir.CreatedAt)
            .IsRequired();

        builder.Property(ir => ir.UpdatedAt)
            .IsRequired();

        // Relationships
        builder.HasOne(ir => ir.Requester)
            .WithMany(u => u.ItemRequests)
            .HasForeignKey(ir => ir.RequesterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ir => ir.Community)
            .WithMany(c => c.ItemRequests)
            .HasForeignKey(ir => ir.CommunityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ir => ir.FulfilledByItem)
            .WithMany()
            .HasForeignKey(ir => ir.FulfilledByItemId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}