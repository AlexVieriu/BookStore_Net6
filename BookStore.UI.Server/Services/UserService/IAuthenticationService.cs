using BookStore.UI.Server.Services.Base;

namespace BookStore.UI.Server.Services.User;
public interface IAuthenticationService
{
    Task<string> Login(LoginUser loginUser);
    Task Logout();
}