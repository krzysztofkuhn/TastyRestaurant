using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TastyRestaurant.WebApi.Application.Authentication.Models;
using TastyRestaurant.WebApi.Domain.Entities;

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
            if (_context.Database.IsSqlServer())
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
        var now = DateTime.Now;
        var administrator = new ApplicationUser
        {
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

        // Default data
        // Seed, if necessary
        if (!_context.MenuItemCategories.Any())
        {
            MenuItemCategory StartersCategory = MenuItemCategory.Create(Guid.NewGuid(), "Starters");
            MenuItemCategory SaladsCategory = MenuItemCategory.Create(Guid.NewGuid(), "Salads");
            MenuItemCategory MainCoursesCategory = MenuItemCategory.Create(Guid.NewGuid(), "Main courses");
            MenuItemCategory DessertsCategory = MenuItemCategory.Create(Guid.NewGuid(), "Desserts");
            MenuItemCategory BeveragesCategory = MenuItemCategory.Create(Guid.NewGuid(), "Beverages");

            MenuItem FrenchOnionSoup = MenuItem.Create(Guid.NewGuid(), "French onion soup", StartersCategory, 24, "frenchOnionSoup.jpg");
            MenuItem Prawns = MenuItem.Create(Guid.NewGuid(), "Prawns", StartersCategory, 49, "prawns.jpg");
            MenuItem BeefTartare = MenuItem.Create(Guid.NewGuid(), "Beef tartare", StartersCategory, 48, "beefTartare.jpg");
            MenuItem SeafoodStarter = MenuItem.Create(Guid.NewGuid(), "Seafood starter", StartersCategory, 52, "seafoodStarter.jpg");
            MenuItem BeefCarpaccio = MenuItem.Create(Guid.NewGuid(), "Beef carpaccio", StartersCategory, 49, "beefCarpaccio.jpg");

            MenuItem CheeseSalad = MenuItem.Create(Guid.NewGuid(), "3-cheese salad", SaladsCategory, 42, "cheeseSalad.jpg");
            MenuItem ChefsSalad = MenuItem.Create(Guid.NewGuid(), "Chef’s salad", SaladsCategory, 44, "cheeseSalad.jpg");
            MenuItem GoatCheeseSalad = MenuItem.Create(Guid.NewGuid(), "Goat cheese salad", SaladsCategory, 43, "goatCheeseSalad.jpg");

            MenuItem Steak = MenuItem.Create(Guid.NewGuid(), "Steak in green pepper sauce", MainCoursesCategory, 82, "steak.jpg");
            MenuItem GrilledChicken = MenuItem.Create(Guid.NewGuid(), "Grilled Chicken", MainCoursesCategory, 49, "grilledChicken.jpg");
            MenuItem Burger = MenuItem.Create(Guid.NewGuid(), "Papa Burger", MainCoursesCategory, 48, "papaBurger.jpg");
            MenuItem Duck = MenuItem.Create(Guid.NewGuid(), "Confit of duck", MainCoursesCategory, 62, "confitDuck.jpg");
            MenuItem Mussels = MenuItem.Create(Guid.NewGuid(), "Mussels", MainCoursesCategory, 57, "mussels.jpg");

            MenuItem LemonTart = MenuItem.Create(Guid.NewGuid(), "Lemon tart", DessertsCategory, 22, "lemonTart.jpg");
            MenuItem ChocolateCake = MenuItem.Create(Guid.NewGuid(), "Chocolate Cake", DessertsCategory, 28, "chocolateCake.jpg");
            MenuItem IceCream = MenuItem.Create(Guid.NewGuid(), "Ice Cream", DessertsCategory, 23, "iceCream.jpg");
            MenuItem LavaCake = MenuItem.Create(Guid.NewGuid(), "Lava Cake", DessertsCategory, 28, "lavaCake.jpg");
            MenuItem Brownie = MenuItem.Create(Guid.NewGuid(), "Brownie", DessertsCategory, 25, "brownie.jpg");


            MenuItem Beer = MenuItem.Create(Guid.NewGuid(), "Beer 0.5L", BeveragesCategory, 13, "beer.jpg");
            MenuItem Water = MenuItem.Create(Guid.NewGuid(), "Water 0.5L", BeveragesCategory, 10, "water.jpg");
            MenuItem CocaCola = MenuItem.Create(Guid.NewGuid(), "Coca-Cola 0.2L", BeveragesCategory, 11, "cocaCola.jpg");
            MenuItem OrangeJuice = MenuItem.Create(Guid.NewGuid(), "Orange Juice 0.2L", BeveragesCategory, 12, "orangeJuice.jpg");

            await _context.MenuItemCategories.AddRangeAsync(new[]
            {
                StartersCategory,
                SaladsCategory,
                MainCoursesCategory,
                DessertsCategory,
                BeveragesCategory
            });

            //await _context.MenuItems.AddRangeAsync(new[]
            //{
            //    FrenchOnionSoup,
            //    Prawns,
            //    BeefTartare,
            //    SeafoodStarter,
            //    BeefCarpaccio,
            //    CheeseSalad,
            //    ChefsSalad,
            //    GoatCheeseSalad,
            //    Steak,
            //    GrilledChicken,
            //    Burger,
            //    Duck,
            //    Mussels,
            //    LemonTart,
            //    ChocolateCake,
            //    IceCream,
            //    LavaCake,
            //    Brownie,
            //    Beer,
            //    Water,
            //    CocaCola,
            //    OrangeJuice
            //});

            await _context.SaveChangesAsync();
        }
    }
}