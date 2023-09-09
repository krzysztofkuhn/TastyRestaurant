using NSubstitute;
using TastyRestaurant.UnitTests.SampleData;
using TastyRestaurant.WebApi.Application.Commands;
using TastyRestaurant.WebApi.Application.Exceptions;
using TastyRestaurant.WebApi.Application.Models;
using TastyRestaurant.WebApi.Domain.Entities;
using TastyRestaurant.WebApi.Domain.Enums;
using TastyRestaurant.WebApi.Domain.Repositories;

namespace TastyRestaurant.UnitTests.Application
{
    public class CreateOrderCommandUnitTests
    {
        #region ARRANGE

        // system under test
        private readonly CreateOrderCommandHandler _sut;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMenuItemRepository _menuItemRepository;

        public CreateOrderCommandUnitTests()
        {
            _orderRepository = Substitute.For<IOrderRepository>();
            _userRepository = Substitute.For<IUserRepository>();
            _menuItemRepository = Substitute.For<IMenuItemRepository>();

            _sut = new CreateOrderCommandHandler(_orderRepository, _userRepository, _menuItemRepository);
        }
        #endregion

        [Fact]
        public async Task Handle_Calls_Order_Repo_On_Success()
        {
            // arrange
            // make user repo return valid user
            var validUserId = Guid.NewGuid();
            _userRepository.GetAsync(validUserId).Returns(new Guest { Id = validUserId });
            // make menu item repo return all menu items
            _menuItemRepository.GetAllAsync().Returns(MenuItemSampleData.All);
            //make order repo save created order
            Order? createdRepoOrder = default;
            _orderRepository.When(x => x.AddAsync(Arg.Any<Order>())).Do(args => createdRepoOrder = (Order)args[0]);
            //items to add
            var orderItemModels = new List<OrderItemModel>
            {
                new (MenuItemSampleData.Beer.Id, 1),
                new (MenuItemSampleData.FrenchOnionSoup.Id, 3)
            };
            //create command
            CreateOrderCommand command = new CreateOrderCommand(validUserId, orderItemModels);

            // act
            Order createdOrder = await _sut.Handle(command, CancellationToken.None);

            // assert
            await _orderRepository.Received(1).AddAsync(Arg.Any<Order>()); //check if order repo add async method has been called once
            Assert.Equal(OrderStatusEnum.Created, createdOrder.Status);
            Assert.Equal(validUserId, createdOrder.UserId);
            Assert.Equal(2, createdOrder.OrderItems.Count);
            Assert.Contains(OrderItem.Create(MenuItemSampleData.Beer, 1), createdOrder.OrderItems);
            Assert.Contains(OrderItem.Create(MenuItemSampleData.FrenchOnionSoup, 3), createdOrder.OrderItems);
            Assert.Equal(createdOrder, createdRepoOrder);
        }

        [Fact]
        public async Task Handle_Throws_UserNotFoundException_When_Invalid_UserId()
        {
            // arrange
            // make user repo return null for valid user id
            var invalidUserId = Guid.NewGuid();
            _userRepository.GetAsync(invalidUserId).Returns(default(Guest));

            //items to add
            var orderItemModels = new List<OrderItemModel>
            {
                new (MenuItemSampleData.Beer.Id, 1),
                new (MenuItemSampleData.FrenchOnionSoup.Id, 3)
            };
            //create command
            CreateOrderCommand command = new CreateOrderCommand(invalidUserId, orderItemModels);

            // act/assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _sut.Handle(command, CancellationToken.None));
        }        
        
        [Fact]
        public async Task Handle_Throws_MenuItemNotFoundException_When_Invalid_MenuItemId()
        {
            // arrange
            // make user repo return valid user
            var validUserId = Guid.NewGuid();
            _userRepository.GetAsync(validUserId).Returns(new Guest { Id = validUserId });
            //items to add
            var notExistingMenuItemGuid = Guid.NewGuid();
            var orderItemModels = new OrderItemModel[] { new (notExistingMenuItemGuid, 1) };
            //create command
            CreateOrderCommand command = new CreateOrderCommand(validUserId, orderItemModels);

            // act/assert
            await Assert.ThrowsAsync<MenuItemNotFoundException>(() => _sut.Handle(command, CancellationToken.None));
        }
    }
}