using MercadoLibroDB.Models;

namespace MercadoLibro.Features.UserFeature
{
    public class UserService(
        UserRepository userRepository    
    )
    {
        readonly UserRepository _userRepository = userRepository;
        public async Task<User> AddUser(
            string name,
            string email,
            string password
        )
        {
            //veryfing if the email exists
            //hashing the password
            
            User user = new(name, email);
            UserAuth userAuth = new(password);

            var newUser = await _userRepository.AddAsync(user);

            await _userRepository.SaveChangesAsync();

            userAuth.UserID = newUser.Id;

            await _userRepository.AddAsync(userAuth);

            await _userRepository.SaveChangesAsync();

            return newUser;
        }
    }
}
