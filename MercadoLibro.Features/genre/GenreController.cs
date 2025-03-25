using MercadoLibro.Features.genre.DTOs;
using MercadoLibroDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace MercadoLibro.Features.genre
{
    [ApiController]
    public class GenreController(
        GenreService service
    ) : ControllerBase
    {
        readonly GenreService _service = service;

        [HttpGet("api/genre")]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Genre> genres = await _service.GetAll();

            return Ok(genres);
        }

        [HttpPost("api/genre")]
        public async Task<IActionResult> Add(
            [FromBody] GenreReq req
        )
        {
            string name = req.Name;

            await _service.Add(name);

            if (_service.HasErrors())
            {
                int statusCode = _service.Errors.First().StatusCode;
                return StatusCode(statusCode, _service.Errors);
            }

            return Ok($"Genre \"{name}\" is added sussesfully");
        }

        [HttpPut("api/genre")]
        public async Task<IActionResult> Update(
            [FromBody] UpdateGenreReq req
        )
        {
            string oldName = req.OldName;
            string newName = req.NewName;

            await _service.Update(oldName, newName);

            if (_service.HasErrors())
            {
                int statusCode = _service.Errors.First().StatusCode;
                return StatusCode(statusCode, _service.Errors);
            }

            return Ok($"The old genre \"{oldName}\" is " +
                        $"updated \"{newName}\" sussesfully");
        }

        [HttpDelete("api/genre")]
        public async Task<IActionResult> Delete(
            [FromBody] GenreReq req
        )
        {
            string name = req.Name;
            await _service.Delete(name);

            if (_service.HasErrors())
            {
                int statusCode = _service.Errors.First().StatusCode;
                return StatusCode(statusCode, _service.Errors);
            }

            return Ok($"Genre \"{name}\" is deleted sussesfully");
        }
    }
}
