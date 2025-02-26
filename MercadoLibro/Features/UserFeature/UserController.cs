using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercadoLibro.Features.UserFeature
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(
        UserRepository userRepository
    ) : Controller
    {
        readonly UserRepository _userRepository = userRepository;

        [Authorize(Policy = "Admin")]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.getAll();

            return Ok(users);
        }
    }
}
