using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TastyRestaurant.WebApi.Domain.Entities;

namespace TastyRestaurant.WebApi.Infrastructure.Persistence.Configurations;

public class MenuItemCategoryConfiguration : IEntityTypeConfiguration<MenuItemCategory>
{
    public void Configure(EntityTypeBuilder<MenuItemCategory> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(20);
    }
}