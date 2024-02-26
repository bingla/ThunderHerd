using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using ThunderHerd.DataAccess.Interfaces;

namespace ThunderHerd.DataAccess.Repositories
{
    public class GeneralRepository<T> : IGeneralRepository<T> where T : class
    {
        protected readonly ThunderHerdContext _context;
        private readonly DbSet<T> _set;

        public GeneralRepository(ThunderHerdContext context)
        {
            _context = context;
            _set = context.Set<T>();
        }

        public ValueTask<T?> FindAsync(object id, CancellationToken cancellationToken = default)
        {
            return _set.FindAsync(id, cancellationToken);
        }

        public async Task<T> CreateAsync(T item, CancellationToken cancellationToken)
        {
            var tracker = _set.Add(item);
            _ = await _context.SaveChangesAsync(cancellationToken);
            return tracker.Entity;
        }

        public async Task CreateAsync(IEnumerable<T> items, CancellationToken cancellationToken = default)
        {
            _set.AddRange(items);
            _ = await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<T?> UpdateAsync(object oldItemId, T newItem, CancellationToken cancellationToken = default)
        {
            var oldItem = await FindAsync(oldItemId, cancellationToken);
            if (oldItem == default)
            {
                return default;
            }

            _set.Entry(oldItem).CurrentValues.SetValues(newItem);
            _ = _context.SaveChangesAsync(cancellationToken);
            return oldItem;
        }

        public async Task<T?> DeleteAsync(object id, CancellationToken cancellationToken = default)
        {
            var item = await FindAsync(id, cancellationToken);
            if (item == default)
            {
                return default;
            }

            var tracker = _set.Remove(item);
            _ = await _context.SaveChangesAsync(cancellationToken);
            return tracker.Entity;
        }
    }
}
