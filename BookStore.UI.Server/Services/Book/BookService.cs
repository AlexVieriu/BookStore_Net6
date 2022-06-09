namespace BookStore.UI.Server.Services.Book;
public class BookService : BaseHttpService, IBookService
{
    private readonly IClient _client;
    private readonly ILocalStorageService _localStorage;

    public BookService(IClient client, ILocalStorageService localStorage)
        : base(client, localStorage)
    {
        _client = client;
        _localStorage = localStorage;
    }

    public async Task<Response<int>> CreateBook(BookCreateDto book)
    {
        Response<int> response;
        try
        {
            await GetBearerToken();
            await _client.BookPOSTAsync(book);            

            response = new Response<int>()
            {
                Success = true,
                Message = "Book Created"
            };
        }
        catch (ApiException ex)
        {
            response = ConvertApiException<int>(ex);
        }
        return response;
    }

    public async Task<Response<int>> DeleteBook(int id)
    {
        Response<int> response;
        try
        {
            await GetBearerToken();
            await _client.BookDELETEAsync(id);

            response = new Response<int>()
            {
                Success = true,
                Message = "Book Deleted"
            };
        }
        catch (ApiException ex)
        {
            response = ConvertApiException<int>(ex);
        }
        return response;
    }

    public async Task<Response<BookReadDtoVirtualizeResponse>> GetWithPG(QueryParameters queryParams)
    {
        Response<BookReadDtoVirtualizeResponse> response;
        try
        {
            await GetBearerToken();
            var data = await _client.BooksWithPgAsync(queryParams.StartIndex, queryParams.PageSize);
            response = new Response<BookReadDtoVirtualizeResponse>()
            {
                Data = data,
                Success = true
            };
        }
        catch (ApiException ex)
        {
            response = ConvertApiException<BookReadDtoVirtualizeResponse>(ex);
        }
        
        return response;
    }

    public async Task<Response<BookReadDto>> GetBook(int id)
    {
        Response<BookReadDto> response;
        try
        {
            await GetBearerToken();
            var data = await _client.BookGETAsync(id);

            response = new Response<BookReadDto>()
            {
                Success = true,
                Data = data
            };
        }
        catch (ApiException ex)
        {
            response = ConvertApiException<BookReadDto>(ex);
        }
        return response;
    }

    public async Task<Response<List<BookReadDto>>> GetBooks()
    {
        Response<List<BookReadDto>> response;
        try
        {
            await GetBearerToken();
            var data = (await _client.BooksAsync()).ToList();

            response = new Response<List<BookReadDto>>()
            {
                Success = true,
                Data = data
            };
        }
        catch(ApiException ex)
        {
            response = ConvertApiException<List<BookReadDto>>(ex); 
        }
        return response;
    }

    public async Task<Response<int>> UpdateBook(int id, BookUpdateDto book)
    {
        Response<int> response;
        try
        {
            await GetBearerToken();
            await _client.BookPUTAsync(id, book);

            response = new Response<int>()
            {
                Success = true,
                Message = "Book Updated"
            };
        }
        catch(ApiException ex)
        {
            response = ConvertApiException<int>(ex);
        }
        return response;
    }
}
