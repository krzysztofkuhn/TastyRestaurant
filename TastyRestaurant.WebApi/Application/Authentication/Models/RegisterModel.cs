using System.ComponentModel.DataAnnotations;

namespace TastyRestaurant.WebApi.Application.Authentication.Models;

public class RegisterModel
{
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; init; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; init; }

    [Required(ErrorMessage = "First Name is required")]
    public string? FirstName { get; init; }

    [Required(ErrorMessage = "Last Name is required")]
    public string? LastName { get; init; }

    [Required(ErrorMessage = "Phone Number is required")]
    public string? PhoneNumber { get; init; }
    public DateTime? DateOfBirth { get; set; }
}