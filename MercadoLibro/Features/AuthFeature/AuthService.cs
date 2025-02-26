using MercadoLibro.Features.AuthFeature.DTOs;
using MercadoLibro.Features.UserFeature;
using MercadoLibro.Utils;
using MercadoLibroDB.Models;

namespace MercadoLibro.Features.AuthFeature
{
    public class AuthService(
        UserRepository userRepository,
        JWToken jwToken
    )
    {
        readonly UserRepository _userRepository = userRepository;
        readonly JWToken _jwToken = jwToken;
        public async Task<string> SingUp(SingUpRequest singUpRequest)
        {
            string name = singUpRequest.Name;
            string email = singUpRequest.Email;
            string password = singUpRequest.Password;

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

            string token = _jwToken.GenerateToken(userAuth);

            return token;
        }

        public async Task<string> SingUp(SingUpAuthRequest singUpAuthRequest)//SingUp - Auth register method
        {
            string name = singUpAuthRequest.Name;
            string email = singUpAuthRequest.Email;
            string providerId = singUpAuthRequest.ProviderId;
            string authMethod = singUpAuthRequest.AuthMethod;

            string token;
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

            return token;
        }

        public async Task<string> Login(LoginRequest loginRequest)
        {

            string email = loginRequest.Email;
            string password = loginRequest.Password;

            string token;
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(
                    nameof(loginRequest.Email),
                    "must not be empty"
                );

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(
                    nameof(loginRequest.Password),
                    "must not be empty"
                );

            var user = await _userRepository.GetUserByEmail(email)
                        ?? throw new KeyNotFoundException("The user is not registered");

            UserAuth userAuth = await _userRepository.GetUserAuthByUserIdAndAuthMethod(user.Id)
                            ?? throw new KeyNotFoundException("User authentication method not found");

            if (userAuth.Password == null)
                throw new InvalidOperationException("An unexpected error occurred, user corrupted");

            var isMath = AuthHelper.VerifyPassword(password, userAuth.Password);

            if (!isMath)
                throw new UnauthorizedAccessException("Invalid password");

            token = _jwToken.GenerateToken(userAuth);

            return token;
        }

        public async Task<string> Login(
            string email,
            string providerId,
            string authMethod
        )
        {
            throw new NotImplementedException("Login by Auth");
        }
    }
}
