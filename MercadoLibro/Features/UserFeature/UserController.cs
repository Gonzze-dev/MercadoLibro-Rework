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

        // GET: User
        [HttpPost("SingUp")]
        [TransactionExceptionFilter]
        [ServiceFilter(typeof(TransactionFilter))]
        public async Task<IActionResult> SingUp(
            string name,
            string email,
            string password
        )
        {
            var user = await _userService.AddUser(name, email, password);
            
            return Ok(user);
        }
    }
}
