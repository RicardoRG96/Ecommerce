using Application.Abstractions.Data.Repositories.Users;
using Domain.Entities.Users;
using Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Users
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token, long userId, CancellationToken cancellationToken)
        {
             return await _context.RefreshTokens
               .Where(r => r.Token == token)
               .Where(r => r.UserId == userId)
               .SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> IsLatestTokenAsync(string token, CancellationToken cancellationToken)
        {
            string? latestToken = await _context.RefreshTokens
                .OrderByDescending(r => r.ExpiresOnUtc)
                .Select(r => r.Token)
                .FirstOrDefaultAsync(cancellationToken);

            return latestToken == token;
        }

        public async Task<bool> DeleteByUserId(long userId, CancellationToken cancellationToken)
        {
            await _context.RefreshTokens
                .Where(r => r.UserId == userId)
                .ExecuteDeleteAsync(cancellationToken);

            return true;
        }
    }
}
