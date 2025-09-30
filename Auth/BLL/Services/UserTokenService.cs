using BLL.DTO;
using BLL.DTO.User;
using BLL.Services.Base;
using BLL.Services.Interfaces;
using DAL.Entities;
using DAL.Repositories.Interfaces;

namespace BLL.Services
{
    public class UserTokenService : BaseService, IUserTokenService
    {
        private readonly IJwtService _jwtService;
        private readonly ICryptographyService _cryptographyService;

        public UserTokenService(
            IUnitOfWork unitOfWork,
            ICryptographyService cryptographyService,
            IJwtService jwtService) : base(unitOfWork)
        {
            _jwtService = jwtService;
            _cryptographyService = cryptographyService;
        }

        public async Task<object?> AuthenticateAsync(LoginDTO model, string? deviceInfo = null, string? ipAddress = null)
        {
            var user = await _unitOfWork.Users.GetByUserNameAsync(model.Username);
            if (user == null) return null;
            if (!_cryptographyService.VerifyHashedPassword(user.PasswordHash, model.Password)) return null;
            var sessionId = Guid.NewGuid();

            var token = new UserToken
            {
                SessionId = sessionId,
                RefreshToken = await _jwtService.GenerateRefreshTokenAsync(),
                ExpiresAt = DateTime.UtcNow.AddDays(await _jwtService.GetRefreshTokenLifetimeInDays()),
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                LastUsedAt = DateTime.UtcNow,
                DeviceInfo = deviceInfo,
                IpAddress = ipAddress
            };

            await _unitOfWork.UserTokens.InsertOrUpdateAsync(token);
            await _unitOfWork.CommitAsync();

            return new
            {
                AccessToken = await _jwtService.GenerateAccessTokenAsync(user),
                token.RefreshToken,
                token.SessionId,
                User = user
            };
        }

        public async Task<object> RefreshAsync(string refreshToken)
        {
            var token = await _unitOfWork.UserTokens.GetByTokenWithUserAsync(refreshToken);

            if (token is null || token.RefreshToken != refreshToken)
                return new { Message = "RefreshAsync token is not correct" };

            if (token.RevokedAt != null && token.RevokedAt <= DateTime.UtcNow)
                return new { Message = "RefreshAsync token is not active" };

            if (token.ExpiresAt < DateTime.UtcNow)
                return new { Message = "RefreshAsync token has expired" };

            var newRefreshToken = await _jwtService.GenerateRefreshTokenAsync();
            var lifetime = await _jwtService.GetRefreshTokenLifetimeInDays();

            var updatedToken = await ReplaceRefreshTokenAsync(token, newRefreshToken, lifetime);

            return new
            {
                AccessToken = await _jwtService.GenerateAccessTokenAsync(token.User!),
                updatedToken.RefreshToken,
                updatedToken.SessionId
            };
        }

        public async Task<object?> SignOutAsync(string refreshToken)
        {
            var token = await _unitOfWork.UserTokens.GetByTokenWithUserAsync(refreshToken);

            if (token is null || token.RefreshToken != refreshToken)
                return new { Message = "SignOutAsync token is not correct" };

            token.RevokedAt = DateTime.UtcNow;
            await _unitOfWork.UserTokens.InsertOrUpdateAsync(token);
            await _unitOfWork.CommitAsync();

            return null;
        }

        private async Task<UserToken> ReplaceRefreshTokenAsync(UserToken token, string newRefreshToken, int lifetimeDays)
        {
            // НЕ перезаписываем CreatedAt — это дата создания сессии.
            token.RefreshToken = newRefreshToken;
            token.ExpiresAt = DateTime.UtcNow.AddDays(lifetimeDays);
            token.LastUsedAt = DateTime.UtcNow;

            await _unitOfWork.UserTokens.InsertOrUpdateAsync(token);
            await _unitOfWork.CommitAsync();

            return token;
        }

        // --- Active sessions ---
        public async Task<IEnumerable<object>> GetActiveSessionsAsync(Guid userId)
        {
            var tokens = await _unitOfWork.UserTokens.GetByUserIdAsync(userId);

            var activeSessions = tokens
                .Where(t => t.RevokedAt == null && t.ExpiresAt > DateTime.UtcNow)
                .Select(t => new
                {
                    t.Id,
                    t.SessionId,
                    t.CreatedAt,
                    t.LastUsedAt,
                    t.ExpiresAt
                })
                .OrderByDescending(t => t.LastUsedAt ?? t.CreatedAt)
                .ToList();

            return activeSessions;
        }


        public async Task RevokeSessionAsync(Guid userId, Guid sessionId)
        {
            var tokens = await _unitOfWork.UserTokens.GetBySessionIdAsync(userId, sessionId);
            foreach (var t in tokens)
            {
                if (t.RevokedAt == null)
                {
                    t.RevokedAt = DateTime.UtcNow;
                    await _unitOfWork.UserTokens.InsertOrUpdateAsync(t);
                }
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task RevokeAllSessionsAsync(Guid userId)
        {
            var tokens = await _unitOfWork.UserTokens.GetByUserIdAsync(userId);
            foreach (var t in tokens)
            {
                if (t.RevokedAt == null)
                {
                    t.RevokedAt = DateTime.UtcNow;
                    await _unitOfWork.UserTokens.InsertOrUpdateAsync(t);
                }
            }
            await _unitOfWork.CommitAsync();
        }
    }
}
