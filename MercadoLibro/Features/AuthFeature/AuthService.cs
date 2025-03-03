using MercadoLibro.Features.AuthFeature.DTOs.Request;
using MercadoLibro.Features.AuthFeature.DTOs.Service;
using MercadoLibro.Features.RefreshTokenFeature;
using MercadoLibro.Features.UserFeature;
using MercadoLibro.Utils;
using MercadoLibroDB.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace MercadoLibro.Features.AuthFeature
{
    public class AuthService(
        UserRepository userRepository,
        RefreshTokenRepository refreshTokenRepository,
        JWToken jwToken
    )
    {
        readonly UserRepository _userRepository = userRepository;
        readonly RefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
        readonly JWToken _jwToken = jwToken;

        public async Task<TokenResponse> SingUp(SingUpRequest singUpRequest)
        {
            string name = singUpRequest.Name;
            string email = singUpRequest.Email;
            string password = singUpRequest.Password;

            string token;
            string refreshToken;
            int refreshTokenLifeDays = 7;

            int minPasswordLength = 8;

            User? user = await _userRepository.GetUserByEmail(email);

            if (user != null)
            {
                bool exists = await _userRepository.ExistsWithAuthMethod("local", user.Id);

                if (exists)
                    throw new Exception("User already exists");
            }

            if (password.Length < minPasswordLength)
                throw new Exception("Password must be at least 8 characters long");

            if (user == null)
            {
                user = new(name, email);

                _ = await _userRepository.AddAsync(user)
                    ?? throw new Exception("Error creating user");

                await _userRepository.SaveChangesAsync();
            }

            password = AuthHelper.HashPassword(password);

            UserAuth userAuth = new(password)
            {
                UserID = user.Id
            };

            await _userRepository.AddAsync(userAuth);

            await _userRepository.SaveChangesAsync();

            token = _jwToken.GenerateToken(userAuth);

            refreshToken = RefreshTokenHelper.GenerateRefreshToken();

            RefreshToken refresTokenEntity = new()
            {
                Token = refreshToken,
                CreateAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddDays(refreshTokenLifeDays),
                UserID = user.Id
            };

            await _refreshTokenRepository.AddAsync(refresTokenEntity);

            await _refreshTokenRepository.SaveChangesAsync();

            TokenResponse tokenResponse = new()
            {
                Token = token,
                RefreshToken = refreshToken,
            };

            return tokenResponse;
        }

        public async Task<TokenResponse> SingUp(SingUpAuthRequest singUpAuthRequest)//SingUp - Auth register method
        {
            string name = singUpAuthRequest.Name;
            string email = singUpAuthRequest.Email;
            string providerId = singUpAuthRequest.ProviderId;
            string authMethod = singUpAuthRequest.AuthMethod;

            string token;
            string refreshToken;
            int refreshTokenLifeDays = 7;

            User? user = await _userRepository.GetUserByEmail(email);

            if (user == null)
            {
                user = new(name, email);

                _ = await _userRepository.AddAsync(user)
                    ?? throw new Exception("Error creating user");

                await _userRepository.SaveChangesAsync();
            }

            UserAuth userAuth = new(providerId, authMethod)
            {
                UserID = user.Id
            };

            await _userRepository.AddAsync(userAuth);

            await _userRepository.SaveChangesAsync();

            token = _jwToken.GenerateToken(userAuth);
            refreshToken = RefreshTokenHelper.GenerateRefreshToken();

            RefreshToken refreshTokenEntity = new()
            {
                Token = refreshToken,
                CreateAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddDays(refreshTokenLifeDays),
                UserID = user.Id,
            };
            
            await _refreshTokenRepository.AddAsync(refreshTokenEntity);

            await _refreshTokenRepository.SaveChangesAsync();

            TokenResponse tokenResponse = new()
            {
                Token = token,
                RefreshToken = refreshToken,
            };

            return tokenResponse;
        }

        public async Task<TokenResponse> Login(LoginRequest loginRequest)
        {
            string email = loginRequest.Email;
            string password = loginRequest.Password;

            string token;
            RefreshToken refreshToken;

            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(
                    nameof(email),
                    "must not be empty"
                );

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(
                    nameof(password),
                    "must not be empty"
                );

            Guid userId;

            var user = await _userRepository.GetUserByEmail(email)
                        ?? throw new KeyNotFoundException("The user is not registered");
            
            userId = user.Id;

            UserAuth userAuth = await _userRepository.GetUserAuth(userId)
                            ?? throw new KeyNotFoundException("User authentication method not found");
            
            refreshToken = await _refreshTokenRepository.GetOrCreateNewRefreshTokenIfNotExistOrExpired(userId);

            if (userAuth.Password == null)
                throw new InvalidOperationException("An unexpected error occurred, user corrupted");

            var isMath = AuthHelper.VerifyPassword(password, userAuth.Password);

            if (!isMath)
                throw new UnauthorizedAccessException("Invalid password");

            token = _jwToken.GenerateToken(userAuth);

            TokenResponse tokenResponse = new()
            {
                Token = token,
                RefreshToken = refreshToken.Token,
            };
            
            return tokenResponse;
        }

        public async Task<string> Login(
            string email,
            string providerId,
            string authMethod
        )
        {
            throw new NotImplementedException("Login by Auth");
        }

        public async Task<string> SilentAuthentication(string refresTokebn)
        {
            string token;

            RefreshToken refreshToken = await _refreshTokenRepository.GetRefreshTokenAccess(refresTokebn)
                                        ?? throw new Exception("Invalid refresh token");

            if (refreshToken.ExpireAt < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token expired");

            UserAuth userAuth = await _userRepository.GetUserAuth(refreshToken.UserID)
                                ?? throw new KeyNotFoundException("User not found");

            token = _jwToken.GenerateToken(userAuth);

            return token;
        }
    }

}
