using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SasiadMa.Core.Entities;

namespace SasiadMa.Infrastructure.Data.Configurations;

public class BorrowRequestConfiguration : IEntityTypeConfiguration<BorrowRequest>
{
    public void Configure(EntityTypeBuilder<BorrowRequest> builder)
    {
        builder.ToTable("BorrowRequests", t =>
        {
            t.HasCheckConstraint("CK_BorrowRequest_BorrowerRating", "[BorrowerRating] IS NULL OR ([BorrowerRating] >= 1 AND [BorrowerRating] <= 5)");
            t.HasCheckConstraint("CK_BorrowRequest_LenderRating", "[LenderRating] IS NULL OR ([LenderRating] >= 1 AND [LenderRating] <= 5)");
        });

        builder.HasKey(br => br.Id);
        builder.Property(br => br.ItemId) .IsRequired();
        builder.Property(br => br.BorrowerId) .IsRequired();
        builder.Property(br => br.LenderId) .IsRequired();
        builder.Property(br => br.RequestedStartDate) .IsRequired();
        builder.Property(br => br.RequestedEndDate) .IsRequired();
        builder.Property(br => br.Status) .HasConversion<string>() .IsRequired();
        builder.Property(br => br.Message) .HasMaxLength(1000);
        builder.Property(br => br.ResponseMessage) .HasMaxLength(1000);
        builder.Property(br => br.BorrowerFeedback) .HasMaxLength(1000);
        builder.Property(br => br.LenderFeedback) .HasMaxLength(1000);
        builder.Property(br => br.BorrowerRating);
        builder.Property(br => br.LenderRating);
        builder.Property(br => br.IsDepositPaid) .HasDefaultValue(false);
        builder.Property(br => br.CreatedAt) .IsRequired();
        builder.Property(br => br.UpdatedAt) .IsRequired();

        // Relationships
        builder.HasOne(br => br.Item)
            .WithMany(i => i.BorrowRequests)
            .HasForeignKey(br => br.ItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(br => br.Borrower)
            .WithMany(u => u.BorrowRequests)
            .HasForeignKey(br => br.BorrowerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(br => br.Lender)
            .WithMany(u => u.LendRequests)
            .HasForeignKey(br => br.LenderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}