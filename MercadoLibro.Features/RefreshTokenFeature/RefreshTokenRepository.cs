using MercadoLibroDB;
using MercadoLibroDB.Models;
using Microsoft.EntityFrameworkCore;

namespace MercadoLibro.Features.RefreshTokenFeature
{
    public class RefreshTokenRepository(
        TransactionDB transactionDB
    )
    {
       readonly MercadoLibroContext _context = transactionDB.Context;

        public async Task AddAsync(RefreshToken refreshToken)
        {
           await _context.RefreshToken.AddAsync(refreshToken);
        }

        public async Task<RefreshToken?> GetRefreshTokenAccess(Guid userId)
        {
            RefreshToken? refreshToken = await _context
                                    .RefreshToken
                                    .FirstOrDefaultAsync(rToken => 
                                        rToken.UserID == userId
                                        && rToken.Revoke == false
                                    );

            return refreshToken;
        }

        public async Task<RefreshToken?> GetRefreshTokenAccess(string token)
        {
            RefreshToken? refreshToken = await _context
                                    .RefreshToken
                                    .FirstOrDefaultAsync(rToken =>
                                        rToken.Token == token
                                    );

            return refreshToken;
        }

        public async Task<RefreshToken> GetOrCreateNewRefreshTokenIfNotExistOrExpired(Guid userId)
        {
            int expirationTime = 7;
            string token;
            DateTime dateNow = DateTime.UtcNow;
            
            RefreshToken? refreshToken = await GetRefreshTokenAccess(userId);

            if (refreshToken != null 
                && refreshToken.ExpireAt > dateNow
                && !refreshToken.Revoke)
                return refreshToken;

            if (refreshToken != null)
            {
                refreshToken.Revoke = true;
                await SaveChangesAsync();
            }

            token = RefreshTokenHelper.GenerateRefreshToken();

            refreshToken = new()
            {
                Token = token,
                CreateAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddDays(expirationTime),
                UserID = userId
            };

            await AddAsync(refreshToken);
            await SaveChangesAsync();

            return refreshToken;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
