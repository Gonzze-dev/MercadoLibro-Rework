

using MercadoLibro.Features.author.DTOs;
using MercadoLibroDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace MercadoLibro.Features.author
{
    [ApiController]
    public class AuthorController(
        AuthorService service    
    ) : ControllerBase
    {
        readonly AuthorService _service = service;

        [HttpGet("api/author")]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Author> authors = await _service.GetAll();

            return Ok(authors);
        }

        [HttpPost("api/author")]
        public async Task<IActionResult> Add(
            [FromBody] AuthorReq req
        )
        {
            string name = req.name;

            await _service.Add(name);

            if(_service.HasErrors())
            {
                int statusCode = _service.Errors.First().StatusCode;
                return StatusCode(statusCode, _service.Errors);
            }

            return Ok($"Author \"{name}\" is added sussesfully");
        }

        [HttpPut("api/author")]
        public async Task<IActionResult> Update(
            [FromBody] UpdateAuthorReq req
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

            return Ok($"The old author \"{oldName}\" is " +
                        $"updated \"{newName}\" sussesfully");
        }

        [HttpDelete("api/author")]
        public async Task<IActionResult> Delete(
            [FromBody] AuthorReq req
        )
        {
            string name = req.name;
            await _service.Delete(name);

            if (_service.HasErrors())
            {
                int statusCode = _service.Errors.First().StatusCode;
                return StatusCode(statusCode, _service.Errors);
            }

            return Ok($"Author \"{name}\" is deleted sussesfully");
        }
    }
}
