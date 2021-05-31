using Delpin.Domain.Entities;
using Delpin.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delpin.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PostalCitiesController : ControllerBase
    {
        private readonly DelpinContext _context;
        private readonly ILogger<PostalCitiesController> _logger;

        public PostalCitiesController(DelpinContext context, ILogger<PostalCitiesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<PostalCity>>> GetAll()
        {
            var postalCities = await _context.PostalCities.ToListAsync();
            return Ok(new List<PostalCity>(postalCities.OrderBy(x => x.PostalCode)));
        }

        [HttpGet("{postalCode}")]
        public async Task<ActionResult<PostalCity>> Get(string postalCode)
        {
            var postalCity = await _context.PostalCities.FindAsync(postalCode);

            if (postalCity == null)
            {
                _logger.LogInformation($"No {nameof(PostalCity)} was found with id: {postalCode}");
                return NotFound();
            }

            return Ok(postalCity);
        }

        [Authorize(Policy = "IsSuperUser")]
        [HttpPost]
        public async Task<ActionResult<PostalCity>> Create([FromBody] PostalCity request)
        {
            await _context.PostalCities.AddAsync(request);
            bool created = await _context.SaveChangesAsync() > 0;

            if (!created)
            {
                _logger.LogInformation($"Unable to create {nameof(PostalCity)}. Please check model and try again.");
                return BadRequest();
            }

            return CreatedAtAction(nameof(Get), new { postalCode = request.PostalCode }, request);
        }

        [Authorize(Policy = "IsSuperUser")]
        [HttpDelete("{postalCode}")]
        public async Task<ActionResult> Delete(string postalCode)
        {
            var postalCityToDelete = await _context.PostalCities.FindAsync(postalCode);

            if (postalCityToDelete == null)
            {
                _logger.LogInformation($"No {nameof(PostalCity)} was found with id: {postalCode}");
                return NotFound();
            }

            _context.PostalCities.Remove(postalCityToDelete);
            bool deleted = await _context.SaveChangesAsync() > 0;

            if (!deleted)
            {
                _logger.LogInformation($"Unable to delete {nameof(PostalCity)}. Please check model and try again.");
                return BadRequest();
            }

            return NoContent();
        }
    }
}
