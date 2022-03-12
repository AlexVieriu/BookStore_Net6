using BookStore.Api.Model;

namespace BookStore.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IBookRepository _bookRepo;
    private readonly IWebHostEnvironment _webHost;

    public BookController(IMapper mapper,
                          ILogger<BookController> logger,
                          IBookRepository bookRepo,
                          IWebHostEnvironment webHost)
    {
        _mapper = mapper;
        _logger = logger;
        _bookRepo = bookRepo;
        _webHost = webHost;
    }

    // GET: api/Book/?stratindex=0&pagesize=15
    [HttpGet]
    [Route("/api/BooksWithPg")]
    public async Task<ActionResult<VirtualizeResponse<BookReadDto>>> GetBooksByPage([FromQuery] QueryParameters queryParam)
    {
        try
        {
            return await _bookRepo.GetAllAsync<BookReadDto>(queryParam);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error at {(GetBooksByPage)}: {ErrorMsg.Error500(ex)}");
            return StatusCode(500, ErrorMsg.Error500(ex));
        }
    }

    // GET: api/Books
    [HttpGet]
    [Route("/api/Books")]
    public async Task<ActionResult<List<BookReadDto>>> GetBooks()
    {
        try
        {
            var booksDto = await _bookRepo.GetAllBooksAsync();

            return booksDto;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error at {(GetBooks)}: {ErrorMsg.Error500(ex)}");
            return StatusCode(500, ErrorMsg.Error500(ex));
        }
    }

    // GET: api/Books/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookReadDto>> GetBook(int id)
    {
        try
        {
            var bookDto = await _bookRepo.GetBookAsync(id);

            if (bookDto == null)
                return NotFound();

            return bookDto;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error at {(GetBook)}: {ErrorMsg.Error500(ex)}");
            return StatusCode(500, ErrorMsg.Error500(ex));
        }
    }

    // POST: api/Books
    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] BookCreateDto bookDto)
    {
        try
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var book = _mapper.Map<Book>(bookDto);

            if (string.IsNullOrEmpty(bookDto.ImageData) == false)
                book.Image = CreateImg(bookDto.ImageData, bookDto.OriginalImageName);

            await _bookRepo.AddAsync(book);

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error at {(CreateBook)}: {ErrorMsg.Error500(ex)}");
            return StatusCode(500, ErrorMsg.Error500(ex));
        }
    }

    // PUT: api/Books/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] BookUpdateDto bookDto)
    {
        try
        {
            if (ModelState.IsValid == false || bookDto.Id != id)
                return BadRequest(ModelState);

            var book = await _bookRepo.GetAsync(id);

            if (book == null)
                return NotFound();

            if (string.IsNullOrEmpty(bookDto.ImageData) == false)
            {
                // create a new image
                bookDto.Image = CreateImg(bookDto.ImageData, bookDto.OriginalImageName);

                // delete the old img
                var picName = Path.GetFileName(book.Image);
                var imgPath = $"{_webHost.WebRootPath}\\BookCoverImages\\{picName}";
                if (System.IO.File.Exists(imgPath))
                    System.IO.File.Delete(imgPath);
            }

            _mapper.Map(bookDto, book);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error at {(UpdateBook)}: {ErrorMsg.Error500(ex)}");
            return StatusCode(500, ErrorMsg.Error500(ex));
        }
    }

    // DELETE: api/Books/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        try
        {
            var book = await _bookRepo.GetAsync(id);
            if (book == null)
                return NotFound();

            await _bookRepo.DeleteAsync(id);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error at {(DeleteBook)}: {ErrorMsg.Error500(ex)}");
            return StatusCode(500, ErrorMsg.Error500(ex));
        }
    }

    private string? CreateImg(string? imageBase64String, string? originalImage)
    {
        // HttpContext : have all the information of a request
        // IWebHostEnvironment: get the folder structure here

        var appURL = HttpContext.Request.Host.Value;
        var ext = Path.GetExtension(originalImage);
        var imgName = $"{Guid.NewGuid()}{ext}";

        var path = $"{_webHost.WebRootPath}\\BookCoverImages\\{imgName}";

        byte[] imageByte = Convert.FromBase64String(imageBase64String);

        var fileStream = System.IO.File.Create(path);
        fileStream.Write(imageByte, 0, imageByte.Length);
        fileStream.Close();

        return $"https://{appURL}/BookCoverImages/{imgName}";
    }
}
