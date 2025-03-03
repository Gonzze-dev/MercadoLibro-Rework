using MercadoLibro.Filters;
using Microsoft.AspNetCore.Mvc;
using MercadoLibro.Features.AuthFeature.Filters;
using MercadoLibro.Features.AuthFeature.DTOs.Request;
using MercadoLibro.Features.AuthFeature.DTOs.Service;
using MercadoLibroDB.Models;

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
            
            TokenResponse response = await _authService.SingUp(singUpRequest);

            return Ok(response);
        }

        [HttpGet("Login")]
        [LoginExceptionFilter]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            TokenResponse response = await _authService.Login(loginRequest);

            return Ok(response);
        }

        [HttpGet("SilentAuthentication")]
        [LoginExceptionFilter]
        public async Task<IActionResult> SilentAuthentication(string refreshToken)
        {
            string token = await _authService.SilentAuthentication(refreshToken);

            return Ok(token);
        }
    }
}
