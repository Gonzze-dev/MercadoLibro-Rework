using MercadoLibro.Features.CartFeature.Service;
using MercadoLibroDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MercadoLibro.Features.CartFeature.Controlleres
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController(
        CartService cartService
    ) : ControllerBase
    {
        readonly CartService _cartService = cartService;
        
        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult>Get()
        {
            string? userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            Cart? cart = await _cartService.Get(userEmail);

            if (_cartService.HasErrors())
            {
                int statusCode = _cartService.Errors.First().StatusCode;

                return StatusCode(statusCode, new { errors = _cartService.Errors });
            }

            return Ok(cart);
        }

        [Authorize]
        [HttpPost("add/{isbn}/{quantity}")]
        public async Task<IActionResult> Add(
            string isbn,
            int quantity
        )
        {
            string? userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            await _cartService.Add(userEmail, isbn, quantity);

            if (_cartService.HasErrors())
            {
                int statusCode = _cartService.Errors.First().StatusCode;

                return StatusCode(statusCode, new { errors = _cartService.Errors });
            }

            return Ok("The book has be added sussesfully");
        }

        [Authorize]
        [HttpDelete("delete/{isbn}")]
        public async Task<IActionResult> Delete(
            string isbn
        )
        {
            string? userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            await _cartService.Delete(userEmail, isbn);

            if (_cartService.HasErrors())
            {
                int statusCode = _cartService.Errors.First().StatusCode;

                return StatusCode(statusCode, new { errors = _cartService.Errors });
            }

            return Ok("The book has be deleted sussesfully");
        }
    }
}
