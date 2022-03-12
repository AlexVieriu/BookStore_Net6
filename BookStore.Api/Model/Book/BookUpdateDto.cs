namespace BookStore.Api.Model.Book;
public class BookUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public int AuthorId { get; set; }

    [Required]
    [StringLength(50)]
    public string Title { get; set; }

    [Required]
    [Range(1800, int.MaxValue)]
    public int Year { get; set; }

    [Required]
    public string Isbn { get; set; }

    [Required]
    [StringLength(250, MinimumLength = 10)]
    public string Summary { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public decimal Price { get; set; }

    public string? Image { get; set; }
    public string? ImageData { get; set; }
    public string? OriginalImageName { get; set; }
}
