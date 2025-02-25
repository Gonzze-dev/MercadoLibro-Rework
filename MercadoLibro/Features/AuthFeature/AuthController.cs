using MercadoLibro.Filters;
using Microsoft.AspNetCore.Mvc;
using MercadoLibro.Features.AuthFeature.Filters;

namespace MercadoLibro.Features.AuthFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(
        AuthService authService
    ) : Controller
    {
        readonly AuthService _authService = authService;

        [HttpPost("SingUp")]
        [TransactionExceptionFilter]
        [ServiceFilter(typeof(TransactionFilter))]
        public async Task<IActionResult> SingUp(
            string name,
            string email,
            string password
        )
        {
            string token = await _authService.SingUp(name, email, password);

            return Ok(token);
        }

        [HttpGet("Login")]
        [LoginExceptionFilter]
        public async Task<IActionResult> Login(
            string email,
            string password
        )
        {
            string token = await _authService.Login(email, password);

            return Ok(token);
        }
    }
}
