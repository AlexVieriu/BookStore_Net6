using BookStore.Api.Model;

namespace BookStore.Api.Repositories.Contracts;

public interface IBaseRepository<T> where T : class
{
    Task<T> AddAsync(T entity);
    Task DeleteAsync(int id);
    Task<T> GetAsync(int? id);
    Task<List<T>> GetAllAsync();
    Task UpdateAsync(T entity);
    Task<VirtualizeResponse<TResult>> GetAllAsync<TResult>(QueryParameters queryParam) where TResult : class;
}