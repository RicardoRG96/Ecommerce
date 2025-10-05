namespace Application.Abstractions.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken);
        void Update(TEntity entity, CancellationToken cancellationToken);
        void Delete(TEntity entity, CancellationToken cancellationToken);
    }
}
