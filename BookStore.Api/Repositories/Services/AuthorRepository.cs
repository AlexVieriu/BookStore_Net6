namespace BookStore.Api.Repositories.Services;
public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
{
    private readonly BookStoreContext _context;
    private readonly IMapper _mapper;

    public AuthorRepository(BookStoreContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AuthorReadDto> GetAuthorAsync(int id)
    {
        return await _context.Authors.Include(q => q.Books)
                                     .ProjectTo<AuthorReadDto>(_mapper.ConfigurationProvider)
                                     .FirstOrDefaultAsync(q => q.Id == id);
    }
}
