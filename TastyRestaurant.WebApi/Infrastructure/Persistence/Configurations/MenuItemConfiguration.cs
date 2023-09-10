using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TastyRestaurant.WebApi.Domain.Entities;

namespace TastyRestaurant.WebApi.Infrastructure.Persistence.Configurations;

public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(50);
        builder.Property(x => x.Price).HasColumnType("money");
        builder.Property(x => x.Image).HasMaxLength(255);

        builder.HasOne(x => x.Category)
            .WithMany()
            .IsRequired();
    }
}