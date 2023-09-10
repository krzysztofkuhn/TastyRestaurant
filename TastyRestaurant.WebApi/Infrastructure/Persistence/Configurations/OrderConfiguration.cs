using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TastyRestaurant.WebApi.Domain.Entities;

namespace TastyRestaurant.WebApi.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.OrderItems)
            .WithOne()
            .IsRequired();

    }
}