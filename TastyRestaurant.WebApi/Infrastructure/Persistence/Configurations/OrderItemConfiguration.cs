using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TastyRestaurant.WebApi.Domain.Entities;

namespace TastyRestaurant.WebApi.Infrastructure.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.MenuItem)
            .WithMany()
            .IsRequired();

    }
}