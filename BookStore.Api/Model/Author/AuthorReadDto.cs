namespace BookStore.Api.Model.Author;
public class AuthorReadDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Bio { get; set; }

    public List<BookReadDto> Books { get; set; }
}
