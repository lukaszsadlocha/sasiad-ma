using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SasiadMa.Core.Entities;

namespace SasiadMa.Infrastructure.Data.Configurations;

public class CommunityConfiguration : IEntityTypeConfiguration<Community>
{
    public void Configure(EntityTypeBuilder<Community> builder)
    {
        builder.ToTable("Communities");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.InvitationCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(c => c.InvitationCode)
            .IsUnique();

        builder.Property(c => c.ImageUrl)
            .HasMaxLength(5000000); // Increased to handle base64 encoded images

        builder.Property(c => c.Address)
            .HasMaxLength(200);

        builder.Property(c => c.City)
            .HasMaxLength(100);

        builder.Property(c => c.PostalCode)
            .HasMaxLength(10);

        builder.Property(c => c.IsPublic)
            .HasDefaultValue(false);

        builder.Property(c => c.IsActive)
            .HasDefaultValue(true);

        builder.Property(c => c.MaxMembers)
            .HasDefaultValue(1000);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired();
    }
}

public class CommunityMemberConfiguration : IEntityTypeConfiguration<CommunityMember>
{
    public void Configure(EntityTypeBuilder<CommunityMember> builder)
    {
        builder.ToTable("CommunityMembers");

        builder.HasKey(cm => cm.Id);

        builder.Property(cm => cm.UserId)
            .IsRequired();

        builder.Property(cm => cm.CommunityId)
            .IsRequired();

        builder.Property(cm => cm.IsAdmin)
            .HasDefaultValue(false);

        builder.Property(cm => cm.IsActive)
            .HasDefaultValue(true);

        builder.Property(cm => cm.JoinedAt)
            .IsRequired();

        builder.Property(cm => cm.CreatedAt)
            .IsRequired();

        builder.Property(cm => cm.UpdatedAt)
            .IsRequired();

        // Relationships
        builder.HasOne(cm => cm.User)
            .WithMany(u => u.CommunityMemberships)
            .HasForeignKey(cm => cm.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cm => cm.Community)
            .WithMany(c => c.Members)
            .HasForeignKey(cm => cm.CommunityId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique constraint for user-community combination
        builder.HasIndex(cm => new { cm.UserId, cm.CommunityId })
            .IsUnique();
    }
}