namespace BookStore.UI.Client.Services.Author;
public class AuthorService : BaseHttpService, IAuthorService
{
    private readonly IClient _client;
    private readonly ILocalStorageService _localStorage;

    public AuthorService(IClient client, ILocalStorageService localStorage)
        : base(client, localStorage)
    {
        _client = client;
        _localStorage = localStorage;
    }

    public async Task<Response<int>> CreateAuthor(AuthorCreateDto author)
    {
        Response<int> response = new();
        try
        {
            await GetBearerToken();
            await _client.AuthorPOSTAsync(author);
        }
        catch (ApiException ex)
        {
            response = ConvertApiException<int>(ex);
        }
        return response;
    }

    public async Task<Response<int>> DeleteAuthor(int idAuthor)
    {
        Response<int> response;
        try
        {
            await GetBearerToken();
            await _client.AuthorDELETEAsync(idAuthor);
            response = new() { Success = true };
        }
        catch (ApiException ex)
        {
            response = ConvertApiException<int>(ex);
        }
        return response;
    }

    public async Task<Response<List<AuthorReadDto>>> GetAuthors()
    {
        Response<List<AuthorReadDto>> response;
        try
        {
            // adding bearer token to the header
            await GetBearerToken();
            var data = await _client.AuthorsAsync();
            response = new()
            {
                Data = data.ToList(),
                Success = true
            };
        }
        catch (ApiException ex)
        {
            response = ConvertApiException<List<AuthorReadDto>>(ex);
        }

        return response;
    }

    public async Task<Response<AuthorReadDtoVirtualizeResponse>> GetWithPG(QueryParameters queryParams)
    {
        Response<AuthorReadDtoVirtualizeResponse> response;
        try
        {
            await GetBearerToken();
            var data = await _client.AuthorsWithPgAsync(queryParams.StartIndex, queryParams.PageSize);
            response = new Response<AuthorReadDtoVirtualizeResponse>
            {
                Data = data,
                Success = true
            };
        }
        catch (ApiException ex)
        {
            response = ConvertApiException<AuthorReadDtoVirtualizeResponse>(ex);
        }

        return response;
    }

    public async Task<Response<AuthorReadDto>> GetAuthor(int authorId)
    {
        Response<AuthorReadDto> response;
        try
        {
            await GetBearerToken();
            var data = await _client.AuthorGETAsync(authorId);
            response = new()
            {
                Data = data,
                Success = true
            };
        }
        catch (ApiException ex)
        {
            response = ConvertApiException<AuthorReadDto>(ex);
        }

        return response;
    }

    public async Task<Response<int>> UpdateAuthor(int id, AuthorUpdateDto author)
    {
        Response<int> response;
        try
        {
            await GetBearerToken();
            await _client.AuthorPUTAsync(id, author);

            response = new()
            {
                Success = true,
                Message = "Update success"
            };
        }
        catch (ApiException ex)
        {
            response = ConvertApiException<int>(ex);
        }

        return response;
    }
}
