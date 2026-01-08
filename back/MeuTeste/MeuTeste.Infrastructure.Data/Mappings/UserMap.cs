using MeuTeste.Domain.Entities;
using MeuTeste.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeuTeste.Infrastructure.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(512); // Aumentado para hash duplo

            builder.Property(u => u.Role)
                .IsRequired()
                .HasDefaultValue(Role.User)
                .HasConversion<int>();

            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);

            builder.Property(u => u.CreatedAt)
                .HasDefaultValue(DateTime.UtcNow);

            builder.Property(u => u.CreatedUser)
                .HasMaxLength(50);

            builder.Property(u => u.UpdatedAt);

            builder.Property(u => u.UpdatedUser)
                .HasMaxLength(50);

            builder.Property(u => u.IsDelete)
                .HasDefaultValue(false);

            builder.HasIndex(u => u.Username)
                .IsUnique();

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.ToTable("Users");
        }
    }
}
