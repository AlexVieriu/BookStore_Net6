using BookStore.UI.Client.Services.Base;

namespace BookStore.UI.Client.Services.User;
public interface IAuthenticationService
{
    Task<string> Login(LoginUser loginUser);
    Task Logout();
}