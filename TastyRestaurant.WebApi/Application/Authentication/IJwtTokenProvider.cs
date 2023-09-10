using Microsoft.AspNetCore.Identity;
using TastyRestaurant.WebApi.Application.Authentication.Models;

namespace TastyRestaurant.WebApi.Application.Authentication;

public interface IJwtTokenProvider
{
    GenerateJwtTokenResponse Generate(IdentityUser user, IList<string> userRoles);
}