namespace TastyRestaurant.WebApi.Application.Authentication.Models;

public record GenerateJwtTokenResponse(string Token, DateTime ValidTo);