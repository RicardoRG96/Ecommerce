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

        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken)
        {
            return await _context.RefreshTokens
                .Where(r => r.Token == token)
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
