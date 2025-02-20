using MercadoLibro.Features.UserFeature.Filters;
using MercadoLibro.Filters;
using Microsoft.AspNetCore.Mvc;

namespace MercadoLibro.Features.UserFeature
{

    [ApiController]
    [Route("[controller]")]
    
    public class UserController(
        UserService userService
    ) : Controller
    {
        readonly UserService _userService = userService;

        [HttpPost("SingUp")]
        [TransactionExceptionFilter]
        [ServiceFilter(typeof(TransactionFilter))]
        public async Task<IActionResult> SingUp(
            string name,
            string email,
            string password
        )
        {
            var user = await _userService.SingUp(name, email, password);
            
            return Ok(user);
        }

        [HttpGet("Login")]
        [LoginExceptionFilter]
        public async Task<IActionResult> Login(
            string email,
            string password
        )
        {
            var user = await _userService.Login(email, password);

            return Ok(user);
        }
    }
}
