using BookStore.Api.Model;

namespace BookStore.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AuthorController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger<AuthorController> _logger;
    private readonly IAuthorRepository _authorRepo;

    public AuthorController(IMapper mapper, ILogger<AuthorController> logger, IAuthorRepository authorRepo)
    {
        _mapper = mapper;
        _logger = logger;
        _authorRepo = authorRepo;
    }

    // GET: api/Author/?startindex=0&pagesize=15
    [HttpGet]
    [Route("/api/AuthorsWithPg")]
    public async Task<ActionResult<VirtualizeResponse<AuthorReadDto>>> GetAuthorsByPage([FromQuery] QueryParameters queryParam)
    {
        try
        {
            return await _authorRepo.GetAllAsync<AuthorReadDto>(queryParam);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error at {nameof(GetAuthorsByPage)} : {ErrorMsg.Error500(ex)}");
            return StatusCode(500, ErrorMsg.Error500(ex));
        }
    }

    // GET: api/Authors
    [HttpGet]
    [Route("/api/Authors")]
    public async Task<ActionResult<List<AuthorReadDto>>> GetAuthors()
    {
        try
        {
            var authors = await _authorRepo.GetAllAsync();
            var authorsDto = _mapper.Map<List<AuthorReadDto>>(authors);

            return authorsDto;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error at {nameof(GetAuthors)} - {ErrorMsg.Error500(ex)}");
            return StatusCode(500, ErrorMsg.Error500(ex));
        }
    }

    // GET: api/Author/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuthorReadDto>> GetAuthor(int id)
    {
        try
        {
            var author = await _authorRepo.GetAuthorAsync(id);

            if (author == null)
                return NotFound($"Author not found: id={id}");

            return author;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error at {nameof(GetAuthor)}");
            return StatusCode(500, ErrorMsg.Error500(ex));
        }
    }

    // POST: api/Author
    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> CreateAuthor([FromBody] AuthorCreateDto authorDto)
    {
        try
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var author = _mapper.Map<Author>(authorDto);
            await _authorRepo.AddAsync(author);

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error at {nameof(CreateAuthor)} - {ErrorMsg.Error500(ex)}");
            return StatusCode(500, ErrorMsg.Error500);
        }
    }

    // PUT: api/Author/5
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateAuthor([FromBody] AuthorUpdateDto authorDto, int id)
    {
        try
        {
            if (ModelState.IsValid == false || authorDto.Id != id)
                return BadRequest();

            var author = await _authorRepo.GetAsync(id);
            if (author == null)
                return NotFound();

            _mapper.Map(authorDto, author);

            await _authorRepo.UpdateAsync(author);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error at {nameof(UpdateAuthor)} - {ErrorMsg.Error500(ex)}");
            return StatusCode(500, ErrorMsg.Error500);
        }
    }

    // DELETE: api/Author/5
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        try
        {
            var author = await _authorRepo.GetAsync(id);
            if (author == null)
                return NotFound();

            await _authorRepo.DeleteAsync(id);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error at {nameof(DeleteAuthor)} - {ErrorMsg.Error500(ex)}");
            return StatusCode(500, ErrorMsg.Error500);
        }
    }
}
