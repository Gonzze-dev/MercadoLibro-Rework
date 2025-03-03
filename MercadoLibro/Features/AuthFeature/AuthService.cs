using MercadoLibro.DTOs;
using MercadoLibro.Features.AuthFeature.DTOs.Request;
using MercadoLibro.Features.AuthFeature.DTOs.Service;
using MercadoLibro.Features.AuthFeature.Helpers;
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

        public async Task<GeneralResponse<TokenResponse>> SignUp(SignUpRequest signUpRequest)
        {
            string name = signUpRequest.Name;
            string email = signUpRequest.Email;
            string password = signUpRequest.Password;

            GeneralResponse<TokenResponse> response = new();

            string token;
            string refreshToken;
            int refreshTokenLifeDays = 7;

            int minPasswordLength = 8;

            if (password.Length < minPasswordLength)
            {
                response.Errors.Add(new ErrorHttp("Password must be at least 8 characters long", 400));
                return response;
            }

            User? user = await _userRepository.GetUserByEmail(email);

            if (user != null)
            {
                bool exists = await _userRepository.ExistsWithAuthMethod("local", user.Id);

                if (exists){
                    response.Errors.Add(new ErrorHttp("User already exists", 409));
                    return response;
                }
            }

            if (user == null)
            {
                user = new(name, email);

                await _userRepository.AddAsync(user);

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

            response.Data = new()
            {
                Token = token,
                RefreshToken = refreshToken,
            };

            return response;
        }

        public async Task<GeneralResponse<TokenResponse>> SignUp(SignUpAuthRequest signUpAuthRequest)//SingUp - Auth register method
        {
            string name = signUpAuthRequest.Name;
            string email = signUpAuthRequest.Email;
            string providerId = signUpAuthRequest.ProviderId;
            string authMethod = signUpAuthRequest.AuthMethod;

            GeneralResponse<TokenResponse> response = new();

            string token;
            string refreshToken;
            int refreshTokenLifeDays = 7;

            User? user = await _userRepository.GetUserByEmail(email);

            if (user == null)
            {
                user = new(name, email);

                await _userRepository.AddAsync(user);

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

            response.Data = new()
            {
                Token = token,
                RefreshToken = refreshToken,
            };

            return response;
        }

        public async Task<GeneralResponse<TokenResponse>> Login(LoginRequest loginRequest)
        {
            string email = loginRequest.Email;
            string password = loginRequest.Password;

            GeneralResponse<TokenResponse> response = new();

            Guid userId;
            string token;
            RefreshToken refreshToken;

            if (string.IsNullOrEmpty(email))
                response.Errors.Add(new ErrorHttp($"{nameof(email)} must not be empty", 400));

            if (string.IsNullOrEmpty(password))
                response.Errors.Add(new ErrorHttp($"{nameof(email)} must not be empty", 400));

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return response;

            User? user = await _userRepository.GetUserByEmail(email);

            if (user == null)
            {
                response.Errors.Add(new ErrorHttp("The user is not registered", 409));
                return response;
            }
            
            userId = user.Id;

            UserAuth? userAuth = await _userRepository.GetUserAuth(userId);

            if (userAuth == null)
            {
                response.Errors.Add(new ErrorHttp("User authentication not found", 401));
                return response;
            }

            refreshToken = await _refreshTokenRepository.GetOrCreateNewRefreshTokenIfNotExistOrExpired(userId);

            if (userAuth.Password == null)
            {
                response.Errors.Add(new ErrorHttp("An unexpected error occurred, user corrupted", 500));
                return response;
            }

            var isMath = AuthHelper.VerifyPassword(password, userAuth.Password);

            if (!isMath)
            {
                response.Errors.Add(new ErrorHttp("Invalid password", 401));
                return response;
            }

            token = _jwToken.GenerateToken(userAuth);

            response.Data = new()
            {
                Token = token,
                RefreshToken = refreshToken.Token,
            };
            
            return response;
        }

        public async Task<GeneralResponse<string>> SilentAuthentication(string refresTokebn)
        {
            GeneralResponse<string> response = new();

            RefreshToken? refreshToken = await _refreshTokenRepository.GetRefreshTokenAccess(refresTokebn);

            if (refreshToken == null)
            {
                response.Errors.Add(new ErrorHttp("Invalid refresh token", 401));
                return response;
            }

            if (refreshToken.ExpireAt < DateTime.UtcNow)
            {
                response.Errors.Add(new ErrorHttp("Refresh token expired", 401));
                return response;
            }

            UserAuth? userAuth = await _userRepository.GetUserAuth(refreshToken.UserID);

            if (userAuth == null) {
                response.Errors.Add(new ErrorHttp("User not found", 404));
                return response;
            }

            response.Data = _jwToken.GenerateToken(userAuth);

            return response;
        }
    }

}
