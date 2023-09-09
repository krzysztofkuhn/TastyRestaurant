using TastyRestaurant.WebApi.Application.Filters;
using TastyRestaurant.WebApi.Domain.Entities;
using TastyRestaurant.WebApi.Domain.Enums;

namespace TastyRestaurant.WebApi.Application.Services;

public class OrdersService : IOrdersService
{
    public async Task<IEnumerable<Order>> GetAllOrdersAsync(GetAllOrdersFilter filter, PaginationFilter pagination)
    {
        var fakeOrderList = new List<Order>
        {
            new Order
            {
                Id = 1,
                Status = OrderStatusEnum.Created,
                UserId = 2,
                CreationDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        MenuItem = new MenuItem
                        {
                            Name = "Śledzie w śmietanie",
                            Category = new MenuItemCategory
                            {
                                Id = 1,
                                Name = "Main dishes"
                            },
                            Image = "sledzie.jpg",
                            Price = 10.23m
                        }
                    }
                }
            }
        };

        return fakeOrderList;
    }

    public async Task<Order> GetOrderByIdAsync(uint orderId)
    {
        return new Order
        {
            Id = 1,
            Status = OrderStatusEnum.Created,
            UserId = 2,
            CreationDate = DateTime.Now,
            UpdateDate = DateTime.Now,
            OrderItems = new List<OrderItem>
            {
                new OrderItem
                {
                    MenuItem = new MenuItem
                    {
                        Name = "Śledzie w śmietanie",
                        Category = new MenuItemCategory
                        {
                            Id = 1,
                            Name = "Main dishes"
                        },
                        Image = "sledzie.jpg",
                        Price = 10.23m
                    }
                }
            }
        };
    }
}