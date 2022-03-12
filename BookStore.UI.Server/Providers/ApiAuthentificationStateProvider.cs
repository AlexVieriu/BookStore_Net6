using System.Security.Claims;
using BookStore.UI.Server.Static;

namespace BookStore.UI.Server.Providers;
public class ApiAuthentificationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly JwtSecurityTokenHandler _jwt;
    public ApiAuthentificationStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
        _jwt = new();
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        var savedtoken = await _localStorage.GetItemAsync<string>(Token.accessToken);
        if (string.IsNullOrWhiteSpace(savedtoken))
            return new AuthenticationState(user);

        var tokenContent = _jwt.ReadJwtToken(savedtoken);
        if (tokenContent.ValidTo < DateTime.Now)
        {
            await _localStorage.RemoveItemAsync(Token.accessToken);
            return new AuthenticationState(user);
        }

        var claims = await GetClaims();

        user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

        return new AuthenticationState(user);
    }

    public async Task LoggedIn()
    {
        var claims = await GetClaims();
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        var authState = Task.FromResult(new AuthenticationState(user));
        NotifyAuthenticationStateChanged(authState);
    }

    public async Task LoggedOut()
    {
        await _localStorage.RemoveItemAsync(Token.accessToken);
        var nobody = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(nobody));
        NotifyAuthenticationStateChanged(authState);
    }

    private async Task<List<Claim>> GetClaims()
    {
        var savedtoken = await _localStorage.GetItemAsync<string>(Token.accessToken);
        var tokenContent = _jwt.ReadJwtToken(savedtoken);
        var claims = tokenContent.Claims.ToList();
        claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));
        return claims;
    }
}
