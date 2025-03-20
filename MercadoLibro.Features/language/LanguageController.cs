using MercadoLibroDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercadoLibro.Features.language
{
    [ApiController]
    public class LanguageController(
        LanguageService languageService    
    ) : ControllerBase
    {
        readonly LanguageService _service = languageService;

        [HttpGet]
        [Route("api/language")]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Language> languages = await _service.GetAll();

            return Ok(languages);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost()]
        [Route("api/language/{name}")]
        public async Task<IActionResult> Add(
            string name
        )
        {
            var language = await _service.Add(name);

            if (_service.Errors.Count > 0)
            {
                int statusCode = _service.Errors.First().StatusCode;
                return StatusCode(statusCode, _service.Errors);
            }

            return Ok(language);
        }
        [Authorize(Policy = "Admin")]
        [HttpPut()]
        [Route("api/language/{name}/{newName}")]
        public async Task<IActionResult> Update(
            string name,
            string newName
        )
        {
            var language = await _service.Update(name, newName);

            if (_service.Errors.Count > 0)
            {
                int statusCode = _service.Errors.First().StatusCode;
                return StatusCode(statusCode, _service.Errors);
            }

            return Ok(language);
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete()]
        [Route("api/language/{name}")]
        public async Task<IActionResult> Delete(
            string name
        )
        {
            var language = await _service.Delete(name);

            if (_service.Errors.Count > 0)
            {
                int statusCode = _service.Errors.First().StatusCode;
                return StatusCode(statusCode, _service.Errors);
            }

            return Ok(language);
        }

    }
}
