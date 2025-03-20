using MercadoLibroDB.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercadoLibro.Features.publisher
{
    [ApiController]
    public class PublisherController(
        PublisherService publisherService
    ) : ControllerBase
    {
        readonly PublisherService _service = publisherService;

        [HttpGet("api/publisher")]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Publisher> publishers = await _service.GetAll();

            return Ok(publishers);
        }

        [HttpPost("api/publisher/{name}")]
        public async Task<IActionResult> Add(
            string name
        )
        {
            Publisher? publisher = await _service.Add(name);

            if (_service.HasErrors())
            {
                int statusCode = _service.Errors.First().StatusCode;
                return StatusCode(statusCode, _service.Errors);
            }

            return Ok(publisher);
        }

        [HttpPut("api/publisher/{name}/{newName}")]
        public async Task<IActionResult> Update(
            string name,
            string newName
        )
        {
            Publisher? publisher = await _service.Update(name, newName);

            if (_service.HasErrors())
            {
                int statusCode = _service.Errors.First().StatusCode;
                return StatusCode(statusCode, _service.Errors);
            }

            return Ok(publisher);
        }

        [HttpDelete("api/publisher/{name}")]
        public async Task<IActionResult> Delete(
            string name
        )
        {
            Publisher? publisher = await _service.Delete(name);

            if (_service.HasErrors())
            {
                int statusCode = _service.Errors.First().StatusCode;
                return StatusCode(statusCode, _service.Errors);
            }

            return Ok(publisher);
        }
    }
}
