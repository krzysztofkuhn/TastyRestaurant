namespace TastyRestaurant.WebApi.Contracts.V1
{
    public class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = $"{Root}/{Version}";

        public static class Guests
        {
            public const string GetAll = $"{Base}/guests";
            public const string Get = $"{Base}/guests/{{userId:guid}}";
            public const string Create = $"{Base}/guests/";
            public const string Update = $"{Base}/guests/{{userId:guid}}";
        }

        public static class Orders
        {
            public const string GetAll = $"{Base}/orders/";
            public const string Get = $"{Base}/orders/{{orderId:guid}}";
            public const string Create = $"{Base}/orders/";
            public const string Update = $"{Base}/orders/{{orderId:guid}}";
        }

        public static class MenuItems
        {
            public const string GetAll = $"{Base}/menuitems/";
        }
    }
}