namespace TastyRestaurant.WebApi.Contracts.V1.Responses;

public record MenuItemResponse(
    int Id,
    string Name,
    MenuItemCategoryResponse Category,
    decimal Price,
    string Image
    );