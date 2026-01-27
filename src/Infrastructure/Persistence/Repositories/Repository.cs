using Application.Abstractions.Data.Repositories;
using Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await _context.Set<TEntity>().FindAsync(id, cancellationToken);
        }

        public async Task<PaginatedList<TEntity>> GetAllWithPaginationAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().AsQueryable<TEntity>();

            int count = await query.CountAsync(cancellationToken);
            List<TEntity> items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return PaginatedList<TEntity>.Create(items, count, pageNumber, pageSize);
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }
    }
}
