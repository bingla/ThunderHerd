namespace ThunderHerd.DataAccess.Interfaces
{
    public interface IGeneralRepository<T> where T : class
    {
        ValueTask<T?> FindAsync(object id, CancellationToken cancellationToken);
        Task<T> CreateAsync(T item, CancellationToken cancellationToken);
        Task<T?> UpdateAsync(object oldItemId, T newItem, CancellationToken cancellationToken);
        Task<T?> DeleteAsync(object id, CancellationToken cancellationToken);
    }
}