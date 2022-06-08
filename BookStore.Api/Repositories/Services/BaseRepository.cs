using BookStore.Api.Model;

namespace BookStore.Api.Repositories.Services;
public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly BookStoreContext _context;
    private readonly IMapper _mapper;

    public BaseRepository(BookStoreContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // return entity with the Id
    public async Task<T> AddAsync(T entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetAsync(id);
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<VirtualizeResponse<TResult>> GetAllAsync<TResult>(QueryParameters queryParam)
        where TResult : class
    {
        var totalSize = await _context.Set<T>().CountAsync();
        var items = await _context.Set<T>().Skip(queryParam.StartIndex)                    
                                           .Take(totalSize)
                                           .ProjectTo<TResult>(_mapper.ConfigurationProvider) // optional 
                                           .ToListAsync();

        return new VirtualizeResponse<TResult> { Items = items, TotalSize = totalSize };
    }

    public async Task<T> GetAsync(int? id)
    {
        if (id == null)
            return null;

        return await _context.Set<T>().FindAsync(id);
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Update(entity);
        await _context.SaveChangesAsync();
    }
}
