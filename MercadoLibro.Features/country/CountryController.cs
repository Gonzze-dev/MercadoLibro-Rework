using MercadoLibroDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercadoLibro.Features.country
{
    [ApiController]
    public class CountryController(
        CountryService service    
    ): ControllerBase
    {
        readonly CountryService _service = service;

        [Authorize(Policy = "Admin")]
        [HttpGet("api/country")]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Country> Country = await _service.GetAll();

            return Ok(Country);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("api/country/{name}")]
        public async Task<IActionResult> Add(
            string name
        )
        {
            Country? country = await _service.Add(name);

            if (_service.HasErrors())
            {
                int statusCode = _service.Errors.First().StatusCode;
                return StatusCode(statusCode, _service.Errors);
            }

            return Ok(country);
        }

        [Authorize(Policy = "Admin")]
        [HttpPut("api/country/{name}/{newName}")]
        public async Task<IActionResult> Update(
            string name,
            string newName
        )
        {
            Country? country = await _service.Update(name, newName);

            if (_service.HasErrors())
            {
                int statusCode = _service.Errors.First().StatusCode;
                return StatusCode(statusCode, _service.Errors);
            }

            return Ok(country);
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("api/country/{name}")]
        public async Task<IActionResult> Delete(
            string name
        )
        {
            Country? country = await _service.Delete(name);

            if (_service.HasErrors())
            {
                int statusCode = _service.Errors.First().StatusCode;
                return StatusCode(statusCode, _service.Errors);
            }

            return Ok(country);
        }
    }
}
