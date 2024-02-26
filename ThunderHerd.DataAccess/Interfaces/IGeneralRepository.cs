namespace ThunderHerd.DataAccess.Interfaces
{
    public interface IGeneralRepository<T> where T : class
    {
        ValueTask<T?> FindAsync(object id, CancellationToken cancellationToken = default);
        Task<T> CreateAsync(T item, CancellationToken cancellationToken = default);
        Task CreateAsync(IEnumerable<T> items, CancellationToken cancellationToken = default);
        Task<T?> UpdateAsync(object oldItemId, T newItem, CancellationToken cancellationToken = default);
        Task<T?> DeleteAsync(object id, CancellationToken cancellationToken = default);
    }
}