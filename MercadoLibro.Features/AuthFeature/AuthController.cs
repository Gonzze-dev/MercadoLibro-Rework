using Microsoft.AspNetCore.Mvc;
using MercadoLibro.Features.AuthFeature.DTOs.Request;
using MercadoLibro.Features.AuthFeature.DTOs.Service;
using MercadoLibro.Features.AuthFeature.Utils;
using MercadoLibro.Features.AuthFeature.DTOs;
using System.Web;
using Microsoft.Extensions.Configuration;
using MercadoLibro.Features.General.Utils;
using MercadoLibro.Features.General.Filters;
using MercadoLibro.Features.General.DTOs;

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

            GeneralResponse<TokenResponse> response = await _authService.SignUp(signUpRequest);

            if (response.Errors.Count != 0)
            {
                var statusCode = response.Errors.First().StatusCode;

                return StatusCode(statusCode, new {errors = response.Errors});
            }

            if (response.Data is null)
                throw new Exception("Response data object is null");

            tokenCookie = CookieHelper.CreateTokenCookie(response.Data.Token);
            refreshTokenCookie = CookieHelper.CreateRefreshTokenCookie(response.Data.RefreshToken);

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
            GeneralResponse<TokenResponse> response;

            CookieConfig tokenCookie;
            CookieConfig refreshTokenCookie;

            socialAuthMethod = _socialRedHelper.keyValuePairs[authMethod];

            payload = await socialAuthMethod(_configuration, idToken);

            if (payload is null || _socialRedHelper.Errors.Count > 0)
                return StatusCode(400, new { errors = _socialRedHelper.Errors });

            response = await _authService.SignUp(payload);

            if(response.Data is null)
                throw new Exception("Response data object is null");

            tokenCookie = CookieHelper.CreateTokenCookie(response.Data.Token);
            refreshTokenCookie = CookieHelper.CreateRefreshTokenCookie(response.Data.RefreshToken);

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

            GeneralResponse<TokenResponse> response = await _authService.Login(loginRequest);

            if (response.Errors.Count != 0)
            {
                var statusCode = response.Errors.First().StatusCode;

                return StatusCode(statusCode, response);
            }

            if (response.Data is null)
                throw new Exception("Response data object is null");

            tokenCookie = CookieHelper.CreateTokenCookie(response.Data.Token);
            refreshTokenCookie = CookieHelper.CreateRefreshTokenCookie(response.Data.RefreshToken);

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

            GeneralResponse<string> response = await _authService.SilentAuthentication(refreshToken);

            if (response.Errors.Count != 0)
            {
                var statusCode = response.Errors.First().StatusCode;

                return StatusCode(statusCode, new { errors = response.Errors});
            }

            if (response.Data is null)
                throw new Exception("response data object is null");

            tokenCookie = CookieHelper.CreateTokenCookie(response.Data);

            Response.Cookies.Append(
                tokenCookie.Key,
                tokenCookie.Value,
                tokenCookie.Options
            );

            return Ok(new {message = "Authentication completed successfully"});
        }
    }
}
