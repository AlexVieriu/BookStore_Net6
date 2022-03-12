using System.ComponentModel.DataAnnotations;

namespace BookStore.Api.Model.User;
public class LoginUser
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }    
}
