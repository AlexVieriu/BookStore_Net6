namespace BookStore.UI.Client.Services.Author;
public interface IAuthorService
{
    Task<Response<int>> CreateAuthor(AuthorCreateDto author);
    Task<Response<int>> DeleteAuthor(int idAuthor);
    Task<Response<AuthorReadDto>> GetAuthor(int authorId);
    Task<Response<List<AuthorReadDto>>> GetAuthors();
    Task<Response<AuthorReadDtoVirtualizeResponse>> GetWithPG(QueryParameters queryParams);
    Task<Response<int>> UpdateAuthor(int id, AuthorUpdateDto author);
}
