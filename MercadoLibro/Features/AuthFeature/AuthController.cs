using MercadoLibro.Filters;
using Microsoft.AspNetCore.Mvc;
using MercadoLibro.Features.AuthFeature.Filters;
using MercadoLibro.Features.AuthFeature.DTOs;

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
        public async Task<IActionResult> SingUp(SingUpRequest singUpRequest)
        {
            
            string token = await _authService.SingUp(singUpRequest);

            return Ok(token);
        }

        [HttpGet("Login")]
        [LoginExceptionFilter]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            string token = await _authService.Login(loginRequest);

            return Ok(token);
        }
    }
}
