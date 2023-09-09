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
            public const string Get = $"{Base}/guests/{{orderId}}";
            public const string Create = $"{Base}/guests/";
            public const string Update = $"{Base}/guests/{{orderId}}";
        }

        public static class Orders
        {
            public const string GetAll = $"{Base}/orders/";
            public const string Get = $"{Base}/orders/{{orderId}}";
            public const string Create = $"{Base}/orders/";
            public const string Update = $"{Base}/orders/{{orderId}}";
        }

        public static class MenuItems
        {
            public const string GetAll = $"{Base}/menuitems/";
        }
    }
}