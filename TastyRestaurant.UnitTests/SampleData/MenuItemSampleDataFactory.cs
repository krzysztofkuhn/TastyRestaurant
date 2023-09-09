using TastyRestaurant.WebApi.Domain.Entities;

namespace TastyRestaurant.UnitTests.SampleData;

internal class MenuItemSampleData
{
    public static MenuItemCategory StartersCategory = MenuItemCategory.Create(Guid.NewGuid(), "Starters");
    public static MenuItemCategory SaladsCategory = MenuItemCategory.Create(Guid.NewGuid(), "Salads");
    public static MenuItemCategory MainCoursesCategory = MenuItemCategory.Create(Guid.NewGuid(), "Main courses");
    public static MenuItemCategory DessertsCategory = MenuItemCategory.Create(Guid.NewGuid(), "Desserts");
    public static MenuItemCategory BeveragesCategory = MenuItemCategory.Create(Guid.NewGuid(), "Beverages");

    public static MenuItem FrenchOnionSoup = MenuItem.Create(Guid.NewGuid(), "French onion soup", StartersCategory, 24, "frenchOnionSoup.jpg");
    public static MenuItem Prawns = MenuItem.Create(Guid.NewGuid(), "Prawns", StartersCategory, 49, "prawns.jpg");
    public static MenuItem BeefTartare = MenuItem.Create(Guid.NewGuid(), "Beef tartare", StartersCategory, 48, "beefTartare.jpg");
    public static MenuItem SeafoodStarter = MenuItem.Create(Guid.NewGuid(), "Seafood starter", StartersCategory, 52, "seafoodStarter.jpg");
    public static MenuItem BeefCarpaccio = MenuItem.Create(Guid.NewGuid(), "Beef carpaccio", StartersCategory, 49, "beefCarpaccio.jpg");

    public static MenuItem CheeseSalad = MenuItem.Create(Guid.NewGuid(), "3-cheese salad", SaladsCategory, 42, "cheeseSalad.jpg");
    public static MenuItem ChefsSalad = MenuItem.Create(Guid.NewGuid(), "Chef’s salad", SaladsCategory, 44, "cheeseSalad.jpg");
    public static MenuItem GoatCheeseSalad = MenuItem.Create(Guid.NewGuid(), "Goat cheese salad", SaladsCategory, 43, "goatCheeseSalad.jpg");

    public static MenuItem Steak = MenuItem.Create(Guid.NewGuid(), "Steak in green pepper sauce", MainCoursesCategory, 82, "steak.jpg");
    public static MenuItem GrilledChicken = MenuItem.Create(Guid.NewGuid(), "Grilled Chicken", MainCoursesCategory, 49, "grilledChicken.jpg");
    public static MenuItem Burger = MenuItem.Create(Guid.NewGuid(), "Papa Burger", MainCoursesCategory, 48, "papaBurger.jpg");
    public static MenuItem Duck = MenuItem.Create(Guid.NewGuid(), "Confit of duck", MainCoursesCategory, 62, "confitDuck.jpg");
    public static MenuItem Mussels = MenuItem.Create(Guid.NewGuid(), "Mussels", MainCoursesCategory, 57, "mussels.jpg");

    public static MenuItem LemonTart = MenuItem.Create(Guid.NewGuid(), "Lemon tart", DessertsCategory, 22, "lemonTart.jpg");
    public static MenuItem ChocolateCake = MenuItem.Create(Guid.NewGuid(), "Chocolate Cake", DessertsCategory, 28, "chocolateCake.jpg");
    public static MenuItem IceCream = MenuItem.Create(Guid.NewGuid(), "Ice Cream", DessertsCategory, 23, "iceCream.jpg");
    public static MenuItem LavaCake = MenuItem.Create(Guid.NewGuid(), "Lava Cake", DessertsCategory, 28, "lavaCake.jpg");
    public static MenuItem Brownie = MenuItem.Create(Guid.NewGuid(), "Brownie", DessertsCategory, 25, "brownie.jpg");


    public static MenuItem Beer = MenuItem.Create(Guid.NewGuid(), "Beer 0.5L", BeveragesCategory, 13, "beer.jpg");
    public static MenuItem Water = MenuItem.Create(Guid.NewGuid(), "Water 0.5L", BeveragesCategory, 10, "water.jpg");
    public static MenuItem CocaCola = MenuItem.Create(Guid.NewGuid(), "Coca-Cola 0.2L", BeveragesCategory, 11, "cocaCola.jpg");
    public static MenuItem OrangeJuice = MenuItem.Create(Guid.NewGuid(), "Orange Juice 0.2L", BeveragesCategory, 12, "orangeJuice.jpg");

    public static List<MenuItem> All = new()
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
    };
}