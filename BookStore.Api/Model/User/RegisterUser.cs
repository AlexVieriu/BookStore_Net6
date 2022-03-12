using System.ComponentModel.DataAnnotations;

namespace BookStore.Api.Model.User;
public class RegisterUser : LoginUser
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Role { get; set; }
}
