namespace BookStore.Api.Repositories.Contracts;
public interface IBookRepository : IBaseRepository<Book>
{
    Task<List<BookReadDto>> GetAllBooksAsync();
    Task<BookReadDto> GetBookAsync(int id);
}
