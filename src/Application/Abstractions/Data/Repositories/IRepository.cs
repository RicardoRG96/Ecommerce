using SharedKernel;

namespace Application.Abstractions.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken);
        Task<PaginatedList<TEntity>> GetAllAsync(
            int pageNumber, 
            int pageSize, 
            CancellationToken cancellationToken);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
