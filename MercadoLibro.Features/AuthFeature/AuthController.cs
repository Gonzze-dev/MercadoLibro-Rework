using Microsoft.AspNetCore.Mvc;
using MercadoLibro.Features.AuthFeature.DTOs.Request;
using MercadoLibro.Features.AuthFeature.DTOs.Service;
using MercadoLibro.Features.AuthFeature.Utils;
using MercadoLibro.Features.AuthFeature.DTOs;
using System.Web;
using Microsoft.Extensions.Configuration;
using MercadoLibro.Features.General.Utils;
using MercadoLibro.Features.General.Filters;

namespace MercadoLibro.Features.AuthFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(
        AuthService authService,
        IConfiguration configuration,
        SocialRedHelper socialRedHelper
    ) : Controller
    {
        readonly AuthService _authService = authService;
        readonly IConfiguration _configuration = configuration;
        readonly SocialRedHelper _socialRedHelper = socialRedHelper;

        [HttpPost("SignUp")]
        [TransactionExceptionFilter]
        [ServiceFilter(typeof(TransactionFilter))]
        public async Task<IActionResult> SignUp(SignUpRequest signUpRequest)
        {
            CookieConfig tokenCookie;
            CookieConfig refreshTokenCookie;
            string silentAuhtUri = ApiUrl.SILENT_AUTH;

            TokenResponse? response = await _authService.SignUp(signUpRequest);

            if (_authService.HasErrors())
            {
                var statusCode = _authService.Errors.First().StatusCode;

                return StatusCode(statusCode, new {errors = _authService.Errors});
            }

            if (response is null)
                throw new Exception("Response data object is null");

            tokenCookie = CookieHelper.CreateTokenCookie(response.Token);
            refreshTokenCookie = CookieHelper.CreateRefreshTokenCookie(response.RefreshToken);

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

        [HttpPost("SignUpSocialRed")]
        public async Task<IActionResult> SignUp(SignUpSocialRequest req)
        {
            string idToken = req.IdToken;
            string authMethod = req.AuthMethod.ToLower();
            string silentAuhtUri = ApiUrl.SILENT_AUTH;

            SocialRedHelper.SocialAuthMethod socialAuthMethod;

            SocialPayload? payload;
            TokenResponse response;

            CookieConfig tokenCookie;
            CookieConfig refreshTokenCookie;

            socialAuthMethod = _socialRedHelper.keyValuePairs[authMethod];

            payload = await socialAuthMethod(_configuration, idToken);

            if (payload is null || _socialRedHelper.HasErrors())
                return StatusCode(400, new { errors = _socialRedHelper.Errors });

            response = await _authService.SignUp(payload);

            if(response is null)
                throw new Exception("Response data object is null");

            tokenCookie = CookieHelper.CreateTokenCookie(response.Token);
            refreshTokenCookie = CookieHelper.CreateRefreshTokenCookie(response.RefreshToken);

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

            return Created(silentAuhtUri, new { message = $"{authMethod} User created successfully" });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            CookieConfig tokenCookie;
            CookieConfig refreshTokenCookie;

            TokenResponse? response = await _authService.Login(loginRequest);

            if (_authService.HasErrors())
            {
                var statusCode = _authService.Errors.First().StatusCode;

                return StatusCode(statusCode, _authService.Errors);
            }

            if (response is null)
                throw new Exception("Response data object is null");

            tokenCookie = CookieHelper.CreateTokenCookie(response.Token);
            refreshTokenCookie = CookieHelper.CreateRefreshTokenCookie(response.RefreshToken);

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

        [HttpPost("SilentAuthentication")]
        public async Task<IActionResult> SilentAuthentication(RefreshTokenRequest req)
        {
            CookieConfig tokenCookie;

            if (string.IsNullOrEmpty(req.RefreshToken))
                return StatusCode(400, "refresh token not exist in the cookies");

            string refreshToken = HttpUtility.UrlDecode(req.RefreshToken);

            string? response = await _authService.SilentAuthentication(refreshToken);

            if (_authService.HasErrors())
            {
                var statusCode = _authService.Errors.First().StatusCode;

                return StatusCode(statusCode, new { errors = _authService.Errors});
            }

            if (response is null)
                throw new Exception("response data object is null");

            tokenCookie = CookieHelper.CreateTokenCookie(response);

            Response.Cookies.Append(
                tokenCookie.Key,
                tokenCookie.Value,
                tokenCookie.Options
            );

            return Ok(new {message = "Authentication completed successfully"});
        }
    }
}
