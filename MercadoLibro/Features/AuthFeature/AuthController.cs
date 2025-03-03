using MercadoLibro.Filters;
using Microsoft.AspNetCore.Mvc;
using MercadoLibro.Features.AuthFeature.DTOs.Request;
using MercadoLibro.Features.AuthFeature.DTOs.Service;
using MercadoLibro.DTOs;
using MercadoLibro.Utils;
using MercadoLibro.Features.AuthFeature.Helpers;
using System.Linq;
using MercadoLibro.Features.AuthFeature.Utils;
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
        public async Task<IActionResult> SignUp(SignUpRequest signUpRequest)
        {
            string silentAuhtUri = $"{ApiBaseUrl.Url}/Auth/SilentAuthentication";

            GeneralResponse<TokenResponse> response = await _authService.SignUp(signUpRequest);

            if (response.Errors.Count != 0)
            {
                var statusCode = response.Errors.First().StatusCode;

                return StatusCode(statusCode, new {errors = response.Errors});
            }

            if (response.Data is null)
                throw new Exception("Response data object is null");

            CookieConfig tokenCookie = CookieHelper.CreateTokenCookie(response.Data.Token);
            CookieConfig refreshTokenCookie = CookieHelper.CreateRefreshTokenCookie(response.Data.RefreshToken);

            Response.Cookies.Append(
                tokenCookie.Key,
                tokenCookie.Value,
                tokenCookie.Options
            );

            Response.Cookies.Append(
                refreshTokenCookie.Key,
                refreshTokenCookie.Value,
                refreshTokenCookie.Options
            );

            return Created(silentAuhtUri, new { message = "User created successfully" });
        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            GeneralResponse<TokenResponse> response = await _authService.Login(loginRequest);

            if (response.Errors.Count != 0)
            {
                var statusCode = response.Errors.First().StatusCode;

                return StatusCode(statusCode, response);
            }

            if (response.Data is null)
                throw new Exception("Response data object is null");

            var tokenCookie = CookieHelper.CreateTokenCookie(response.Data.Token);
            var refreshTokenCookie = CookieHelper.CreateRefreshTokenCookie(response.Data.RefreshToken);

            Response.Cookies.Append(
                tokenCookie.Key,
                tokenCookie.Value,
                tokenCookie.Options
            );

            Response.Cookies.Append(
                refreshTokenCookie.Key,
                refreshTokenCookie.Value,
                refreshTokenCookie.Options
            );
            
            return Ok(new {message = "Login successfully" });
        }

        [HttpGet("SilentAuthentication")]
        public async Task<IActionResult> SilentAuthentication()
        {
            bool existRefreshToken = HttpContext
                                        .Request
                                        .Cookies
                                        .TryGetValue("refresh_token", out string? cookieValue);

            if (!existRefreshToken || cookieValue == null)
                return StatusCode(400, "refresh token not exist in the cookies");

            GeneralResponse<string> response = await _authService.SilentAuthentication(cookieValue);

            if (response.Errors.Count != 0)
            {
                var statusCode = response.Errors.First().StatusCode;

                return StatusCode(statusCode, new { errors = response.Errors});
            }

            if (response.Data is null)
                throw new Exception("response data object is null");

            var tokenCookie = CookieHelper.CreateTokenCookie(response.Data);

            Response.Cookies.Append(
                tokenCookie.Key,
                tokenCookie.Value,
                tokenCookie.Options
            );

            return Ok(new {message = "Authentication completed successfully"});
        }
    }
}
