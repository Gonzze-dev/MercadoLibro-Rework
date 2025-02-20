using MercadoLibroDB.Models;

namespace MercadoLibro.Features.UserFeature
{
    public class UserService(
        UserRepository userRepository    
    )
    {
        readonly UserRepository _userRepository = userRepository;
        public async Task<User> SingUp( //AddUser - Normal register method
            string name,
            string email,
            string password
        )
        {
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
                
                _ = await _userRepository.AddAsync(user) ?? throw new Exception("Error creating user");
                
                await _userRepository.SaveChangesAsync();
            }

            password = UserHelper.HashPassword(password);

            UserAuth userAuth = new(password)
            {
                UserID = user.Id
            };

            await _userRepository.AddAsync(userAuth);

            await _userRepository.SaveChangesAsync();

            return user;
        }

        public async Task<User> SingUp( //SingUp - Auth register method
            string name,
            string email,
            string providerId,
            string authMethod
        )
        {
            User? user = await _userRepository.GetUserByEmail(email);

            if (user == null)
            {
                user = new(name, email);

                _ = await _userRepository.AddAsync(user) ?? throw new Exception("Error creating user");

                await _userRepository.SaveChangesAsync();
            }

            UserAuth userAuth = new(providerId, authMethod)
            {
                UserID = user.Id
            };

            await _userRepository.AddAsync(userAuth);

            await _userRepository.SaveChangesAsync();

            return user;
        }

        public async Task<User> Login(
            string email, 
            string password
        )
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                throw new ArgumentNullException("Email and password must not be empty");

            var user = await _userRepository.GetUserByEmail(email) 
                        ?? throw new KeyNotFoundException("The user is not registered");

            UserAuth userAuth = await _userRepository.GetUserAuthByUserIdAndAuthMethod(user.Id) 
                            ?? throw new KeyNotFoundException("User authentication method not found");

            if (userAuth.Password == null) 
                throw new InvalidOperationException("An unexpected error occurred, user corrupted");

            var isMath = UserHelper.VerifyPassword(password, userAuth.Password);
            
            if (!isMath) 
                throw new UnauthorizedAccessException("Invalid password");

            return user;
        }

        public async Task<User> Login(
            string email, 
            string providerId, 
            string authMethod
        )
        {
            throw new NotImplementedException("Login by Auth");
        }
    }
}
