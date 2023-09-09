namespace TastyRestaurant.WebApi.Contracts.V1.Responses;

public record MenuItemResponse(
    Guid Id,
    string Name,
    MenuItemCategoryResponse Category,
    decimal Price,
    string Image
    );