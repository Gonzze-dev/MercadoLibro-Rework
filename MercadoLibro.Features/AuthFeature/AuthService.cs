using MercadoLibro.Features.AuthFeature.DTOs;
using MercadoLibro.Features.AuthFeature.DTOs.Request;
using MercadoLibro.Features.AuthFeature.DTOs.Service;
using MercadoLibro.Features.AuthFeature.Utils;
using MercadoLibro.Features.General.DTOs;
using MercadoLibro.Features.General.Utils;
using MercadoLibro.Features.RefreshTokenFeature;
using MercadoLibro.Features.UserFeature.Repository;
using MercadoLibroDB.Models;

namespace MercadoLibro.Features.AuthFeature
{
    public class AuthService(
        UserRepository userRepository,
        UserAuthRepository userAuthRepository,
        RefreshTokenRepository refreshTokenRepository,
        JWToken jwToken
    )

    {
        public List<ErrorHttp> Errors { get; set; } = [];

        readonly UserRepository _userRepository = userRepository;
        readonly UserAuthRepository _userAuthRepository = userAuthRepository;
        readonly RefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
        readonly JWToken _jwToken = jwToken;

        public async Task<TokenResponse?> SignUp(SignUpRequest signUpRequest)
        {
            string name = signUpRequest.Name;
            string email = signUpRequest.Email;
            string password = signUpRequest.Password;

            TokenResponse response;

            string token;
            string refreshToken;

            User? user = await _userRepository.Get(email);

            if (user != null)
            {
                bool exists = await _userAuthRepository.Exists("local", user.Id);

                if (exists){
                    Errors.Add(new ErrorHttp("User already exists", 409));
                    return null;
                }
            }

            if (user == null)
            {
                user = new(name, email);

                await _userRepository.AddAsync(user);

                await _userRepository.SaveChangesAsync();
            }

            password = AuthHelper.HashPassword(password);

            UserAuth userAuth = new()
            {
                Password = password,
                UserID = user.Id
            };

            await _userAuthRepository.AddAsync(userAuth);

            await _userAuthRepository.SaveChangesAsync();

            token = _jwToken.GenerateToken(userAuth, user);

            refreshToken = RefreshTokenHelper.GenerateRefreshToken();

            RefreshToken refresTokenEntity = new()
            {
                Token = refreshToken,
                ExpireAt = TokenGlobalConfig.GetRefreshTokenLife(),
                UserID = user.Id
            };

            await _refreshTokenRepository.AddAsync(refresTokenEntity);

            await _refreshTokenRepository.SaveChangesAsync();

            response = new()
            {
                Token = token,
                RefreshToken = refreshToken,
            };

            return response;
        }

        public async Task<TokenResponse> SignUp(SocialPayload socialPayload)//SingUp - Auth register method
        {
            string name = socialPayload.Name;
            string email = socialPayload.Email;
            string authMethod = socialPayload.AuthMethod;

            UserAuth? userAuth = null;
            TokenResponse response;

            string token;
            string refreshToken;

            User? user = await _userRepository.Get(email);

            if (user is not null)
            {
                userAuth = await _userAuthRepository.Get(user.Id, authMethod);

            }else
            {
                user = new(name, email);

                await _userRepository.AddAsync(user);

                await _userRepository.SaveChangesAsync();
            }

            if (userAuth is null)
            {
                userAuth = new()
                {
                    AuthMethod = authMethod,
                    UserID = user.Id
                };

                await _userAuthRepository.AddAsync(userAuth);

                await _userRepository.SaveChangesAsync();
            }

            token = _jwToken.GenerateToken(userAuth, user);
            refreshToken = RefreshTokenHelper.GenerateRefreshToken();

            RefreshToken refreshTokenEntity = new()
            {
                Token = refreshToken,
                ExpireAt = TokenGlobalConfig.GetRefreshTokenLife(),
                UserID = user.Id,
            };

            await _refreshTokenRepository.AddAsync(refreshTokenEntity);

            await _refreshTokenRepository.SaveChangesAsync();

            response = new()
            {
                Token = token,
                RefreshToken = refreshToken,
            };

            return response;
        }

        public async Task<TokenResponse?> Login(LoginRequest loginRequest)
        {
            string email = loginRequest.Email;
            string password = loginRequest.Password;

            TokenResponse response;

            Guid userId;
            string token;
            RefreshToken refreshToken;

            if (string.IsNullOrEmpty(email))
                Errors.Add(new ErrorHttp($"{nameof(email)} must not be empty", 400));

            if (string.IsNullOrEmpty(password))
                Errors.Add(new ErrorHttp($"{nameof(email)} must not be empty", 400));

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            User? user = await _userRepository.Get(email);

            if (user == null)
            {
                Errors.Add(new ErrorHttp("The user is not registered", 409));
                return null;
            }
            
            userId = user.Id;

            UserAuth? userAuth = await _userAuthRepository.Get(userId);

            if (userAuth == null)
            {
                Errors.Add(new ErrorHttp("User authentication not found", 401));
                return null;
            }

            refreshToken = await _refreshTokenRepository.GetOrCreateNewRefreshTokenIfNotExistOrExpired(userId);

            if (userAuth.Password == null)
            {
                Errors.Add(new ErrorHttp("An unexpected error occurred, user corrupted", 500));
                return null;
            }

            var isMath = AuthHelper.VerifyPassword(password, userAuth.Password);

            if (!isMath)
            {
                Errors.Add(new ErrorHttp("Invalid password", 401));
                return null;
            }

            token = _jwToken.GenerateToken(userAuth, user);

            response= new()
            {
                Token = token,
                RefreshToken = refreshToken.Token,
            };
            
            return response;
        }

        public async Task<string?> SilentAuthentication(string refresTokebn)
        {
            string response = "";

            Guid userId;
            RefreshToken? refreshToken = await _refreshTokenRepository.GetRefreshTokenAccess(refresTokebn);

            if (refreshToken == null)
            {
                Errors.Add(new ErrorHttp("Invalid refresh token", 401));
                return response;
            }

            if (refreshToken.ExpireAt < DateTime.UtcNow)
            {
                Errors.Add(new ErrorHttp("Refresh token expired", 401));
                return null;
            }
            userId = refreshToken.UserID;

            UserAuth? userAuth = await _userAuthRepository.Get(userId);
            User? user = await _userRepository.Get(userId);

            if (userAuth == null || user == null) {
                Errors.Add(new ErrorHttp("User not found", 404));
                return null;
            }

            response = _jwToken.GenerateToken(userAuth, user);

            return response;
        }

        public bool HasErrors() =>
            Errors.Count > 0;
    }

}
