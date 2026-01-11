using Domain.Entities.Users;

namespace Application.Abstractions.Data.Repositories.Users
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<RefreshToken?> GetByTokenAsync(string token, long userId, CancellationToken cancellationToken);

        Task<bool> IsLatestTokenAsync(string token, long userId, CancellationToken cancellationToken);

        Task<bool> DeleteByUserIdAsync(long userId, CancellationToken cancellationToken);
    }
}
