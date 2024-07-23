namespace DataLayer.Interfaces
{
    public interface ICrudGenericRepository<T, TId> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(TId id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(TId id);
    }
}
