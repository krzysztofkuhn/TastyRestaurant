using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TastyRestaurant.WebApi.Application.Authentication.Models;
using TastyRestaurant.WebApi.Domain.Entities;
using TastyRestaurant.WebApi.Domain.Enums;

namespace TastyRestaurant.WebApi.Infrastructure.Persistence;
public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlite())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole(UserRoles.Admin);
        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }
        var userRole = new IdentityRole(UserRoles.User);
        if (_roleManager.Roles.All(r => r.Name != userRole.Name))
        {
            await _roleManager.CreateAsync(userRole);
        }

        // Default users
        var adminGuid = Guid.Parse("57b31239-7655-40bd-adb5-7b81d6c2a905");
        var now = DateTime.Now;
        var administrator = new ApplicationUser
        {
            Id = adminGuid.ToString(),
            UserName = "administrator@localhost",
            Email = "administrator@localhost",
            FirstName = "Admin",
            LastName = "Admin",
            CreationDate = now,
            UpdateDate = now
        };
        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "admin123Adm");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }

        var guestGuid = Guid.Parse("a076b161-44aa-4f44-bea0-9a3098fc5a01");
        var guest = new ApplicationUser
        {
            Id = guestGuid.ToString(),
            UserName = "guest@localhost",
            Email = "guest@localhost",
            FirstName = "Guest",
            LastName = "Guest",
            CreationDate = now,
            UpdateDate = now
        };
        if (_userManager.Users.All(u => u.UserName != guest.UserName))
        {
            await _userManager.CreateAsync(guest, "guest123G");
        }

        // Default data
        // Seed, if necessary
        if (!_context.MenuItemCategories.Any())
        {
            MenuItemCategory StartersCategory = MenuItemCategory.Create(Guid.Parse("75d4e7d7-b331-4a31-8397-129652cb9163"), "Starters");
            MenuItemCategory SaladsCategory = MenuItemCategory.Create(Guid.Parse("0ae65b93-b17f-459b-9395-deca88226534"), "Salads");
            MenuItemCategory MainCoursesCategory = MenuItemCategory.Create(Guid.Parse("9d75678d-001f-414c-bab7-52a8a9d4e777"), "Main courses");
            MenuItemCategory DessertsCategory = MenuItemCategory.Create(Guid.Parse("85f5a14f-2f8c-4cf2-960e-b0b0565eff8a"), "Desserts");
            MenuItemCategory BeveragesCategory = MenuItemCategory.Create(Guid.Parse("314bc4c6-6a0f-461e-866b-e4eda0374da3"), "Beverages");

            MenuItem FrenchOnionSoup = MenuItem.Create(Guid.Parse("8f43beb5-41ac-41b9-a04b-cce8d01f25f6"), "French onion soup", StartersCategory, 24, "frenchOnionSoup.jpg");
            MenuItem Prawns = MenuItem.Create(Guid.Parse("5efecc1c-4169-4a4c-a9fa-d3d8c0b68be0"), "Prawns", StartersCategory, 49, "prawns.jpg");
            MenuItem BeefTartare = MenuItem.Create(Guid.Parse("6020664b-ee3b-4926-8c30-371829572d81"), "Beef tartare", StartersCategory, 48, "beefTartare.jpg");
            MenuItem SeafoodStarter = MenuItem.Create(Guid.Parse("48593572-45f3-477c-aeb8-bba0c54c05b4"), "Seafood starter", StartersCategory, 52, "seafoodStarter.jpg");
            MenuItem BeefCarpaccio = MenuItem.Create(Guid.Parse("ecf53663-2966-4b98-95d3-ce0f3fe05906"), "Beef carpaccio", StartersCategory, 49, "beefCarpaccio.jpg");

            MenuItem CheeseSalad = MenuItem.Create(Guid.Parse("30499129-b9df-448b-8fd5-ee3579172403"), "3-cheese salad", SaladsCategory, 42, "cheeseSalad.jpg");
            MenuItem ChefsSalad = MenuItem.Create(Guid.Parse("eb574d77-b5c8-4181-9efd-9edcb7ca94f9"), "Chef’s salad", SaladsCategory, 44, "cheeseSalad.jpg");
            MenuItem GoatCheeseSalad = MenuItem.Create(Guid.Parse("44c33fcd-84ea-47bb-a970-c02c835bfe1a"), "Goat cheese salad", SaladsCategory, 43, "goatCheeseSalad.jpg");

            MenuItem Steak = MenuItem.Create(Guid.Parse("ee8c89d9-77b9-486b-bb18-b5632c9ad55a"), "Steak in green pepper sauce", MainCoursesCategory, 82, "steak.jpg");
            MenuItem GrilledChicken = MenuItem.Create(Guid.Parse("02ce0303-3029-4490-8992-dd9cefee3fec"), "Grilled Chicken", MainCoursesCategory, 49, "grilledChicken.jpg");
            MenuItem Burger = MenuItem.Create(Guid.Parse("03d5bb2e-ed5b-4766-a224-992dbe4d037c"), "Papa Burger", MainCoursesCategory, 48, "papaBurger.jpg");
            MenuItem Duck = MenuItem.Create(Guid.Parse("350e4bc5-1ec5-42fa-ab61-e0d7faec3dcc"), "Confit of duck", MainCoursesCategory, 62, "confitDuck.jpg");
            MenuItem Mussels = MenuItem.Create(Guid.Parse("081413bb-b63a-4336-a9c5-b0e361a06178"), "Mussels", MainCoursesCategory, 57, "mussels.jpg");

            MenuItem LemonTart = MenuItem.Create(Guid.Parse("2c11b06e-e169-46a9-9bde-146c736e3491"), "Lemon tart", DessertsCategory, 22, "lemonTart.jpg");
            MenuItem ChocolateCake = MenuItem.Create(Guid.Parse("7a0515fb-2b29-4120-821f-bb1167b70a65"), "Chocolate Cake", DessertsCategory, 28, "chocolateCake.jpg");
            MenuItem IceCream = MenuItem.Create(Guid.Parse("56a42c4a-a0c5-472c-8506-e5eb7f3f1152"), "Ice Cream", DessertsCategory, 23, "iceCream.jpg");
            MenuItem LavaCake = MenuItem.Create(Guid.Parse("3c493684-1e95-4ee2-81c8-efd7942ce236"), "Lava Cake", DessertsCategory, 28, "lavaCake.jpg");
            MenuItem Brownie = MenuItem.Create(Guid.Parse("531bced1-7105-4413-bfe2-7cc59860a8ff"), "Brownie", DessertsCategory, 25, "brownie.jpg");

            MenuItem Beer = MenuItem.Create(Guid.Parse("98063fa9-07b6-48e0-9c3e-504e5844ac68"), "Beer 0.5L", BeveragesCategory, 13, "beer.jpg");
            MenuItem Water = MenuItem.Create(Guid.Parse("5ea0b6dd-4507-4eb9-b0cd-3c22528e2614"), "Water 0.5L", BeveragesCategory, 10, "water.jpg");
            MenuItem CocaCola = MenuItem.Create(Guid.Parse("6e08efe1-3f95-4527-8853-ee23bf70bd49"), "Coca-Cola 0.2L", BeveragesCategory, 11, "cocaCola.jpg");
            MenuItem OrangeJuice = MenuItem.Create(Guid.Parse("50d1c164-3e30-4219-a8cd-7a0e9b9c4f11"), "Orange Juice 0.2L", BeveragesCategory, 12, "orangeJuice.jpg");

            await _context.MenuItemCategories.AddRangeAsync(new[]
            {
                StartersCategory,
                SaladsCategory,
                MainCoursesCategory,
                DessertsCategory,
                BeveragesCategory
            });

            await _context.MenuItems.AddRangeAsync(new[]
            {
                FrenchOnionSoup,
                Prawns,
                BeefTartare,
                SeafoodStarter,
                BeefCarpaccio,
                CheeseSalad,
                ChefsSalad,
                GoatCheeseSalad,
                Steak,
                GrilledChicken,
                Burger,
                Duck,
                Mussels,
                LemonTart,
                ChocolateCake,
                IceCream,
                LavaCake,
                Brownie,
                Beer,
                Water,
                CocaCola,
                OrangeJuice
            });

            var orders = new[]
            {
                CreateOrderWithStatus(OrderStatusEnum.Created, Guid.Parse("d58e386d-aeef-4da8-9380-e8788490e888"), guestGuid, FrenchOnionSoup),
                CreateOrderWithStatus(OrderStatusEnum.Created, Guid.Parse("0d2efc63-c322-42a2-9fb2-2bccce718bce"), guestGuid, Prawns, OrangeJuice),
                CreateOrderWithStatus(OrderStatusEnum.Created, Guid.Parse("dba48868-30c7-4368-bc2d-dd9312a94f62"), guestGuid, Steak, Steak),
                CreateOrderWithStatus(OrderStatusEnum.Ready, Guid.Parse("afeb48a2-89c9-4733-935f-574cde6b966d"), guestGuid, Steak, Beer, Beer),
                CreateOrderWithStatus(OrderStatusEnum.Completed, Guid.Parse("16730350-63a5-4e83-a5e7-157c2f42e31a"), guestGuid, IceCream, LavaCake),
                CreateOrderWithStatus(OrderStatusEnum.Cancelled, Guid.Parse("623b83b4-489c-47bc-aed4-8d82c62d240a"), guestGuid, Water, Duck, Burger),
            };
            _context.Orders.AddRange(orders);

            await _context.SaveChangesAsync();
        }
    }

    private static Order CreateOrderWithStatus(OrderStatusEnum status, Guid orderId, Guid userId, params MenuItem[] selectedItems)
    {
        var orders = selectedItems.Select(x => OrderItem.Create(x, 1));
        var order = Order.Create(orderId, userId, orders);

        switch (status)
        {
            case OrderStatusEnum.Created:
                return order;
            case OrderStatusEnum.Ready:
                order.Ready();
                return order;
            case OrderStatusEnum.Completed:
                order.Ready();
                order.Complete();
                return order;
            case OrderStatusEnum.Cancelled:
                order.Cancel();
                return order;
            default:
                throw new ArgumentOutOfRangeException(nameof(status), status, null);
        }
    }
}