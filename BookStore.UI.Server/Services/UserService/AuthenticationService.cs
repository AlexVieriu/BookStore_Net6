using BookStore.UI.Server.Providers;
using BookStore.UI.Server.Services.Base;
using BookStore.UI.Server.Static;

namespace BookStore.UI.Server.Services.User;
public class AuthenticationService : IAuthenticationService
{
    private readonly IClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly ApiAuthentificationStateProvider _stateProvider;

    public AuthenticationService(IClient httpClient,
                                 ILocalStorageService localStorage,
                                 ApiAuthentificationStateProvider stateProvider)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _stateProvider = stateProvider;
    }

    public async Task<string> Login(LoginUser loginUser)
    {
        try
        {
            var userResponse = await _httpClient.LoginAsync(loginUser);
            await _localStorage.SetItemAsync(Token.accessToken, userResponse.Token);

            await _stateProvider.LoggedIn();

            return "Logged In";
        }
        catch (ApiException ex)
        {
            return $"{ex.Response} - {ex.StatusCode}";
        }
    }

    public async Task Logout()
    {
        await _stateProvider.LoggedOut();
    }
}
