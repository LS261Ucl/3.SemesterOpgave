using AutoMapper;
using Delpin.Application.Interfaces;
using Delpin.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Delpin.Application.Contracts.v1.Rentals;

namespace Delpin.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RentalsController : ControllerBase
    {
        private readonly IGenericRepository<Rental> _rentalRepository;
        private readonly IGenericRepository<RentalLine> _rentalLineRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RentalsController> _logger;

        public RentalsController(IGenericRepository<Rental> rentalRepository, IMapper mapper,
            ILogger<RentalsController> logger, IGenericRepository<RentalLine> rentalLineRepository)
        {
            _rentalRepository = rentalRepository;
            _mapper = mapper;
            _logger = logger;
            _rentalLineRepository = rentalLineRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<RentalDto>>> GetAll(string orderBy)
        {
            var rentals = await _rentalRepository
                .GetAllAsync(includes: x => x.Include(r => r.PostalCity),
                    orderBy: new RentalOrderBy().Sorting(orderBy));

            return Ok(_mapper.Map<IReadOnlyList<RentalDto>>(rentals));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RentalDto>> Get(Guid id)
        {
            var rental = await _rentalRepository.GetAsync(x => x.Id == id,
                x => x.Include(r => r.PostalCity).Include(r => r.RentalLines).ThenInclude(rl => rl.ProductItem).ThenInclude(pi => pi.PostalCity));

            if (rental == null)
            {
                _logger.LogInformation($"No {nameof(Rental)} was found with id: {id}");
                return NotFound();
            }

            return Ok(_mapper.Map<RentalDto>(rental));
        }

        [HttpPost]
        public async Task<ActionResult<RentalDto>> Create([FromBody] CreateRentalDto requestDto)
        {
            var rental = _mapper.Map<Rental>(requestDto);

            bool created = await _rentalRepository.CreateAsync(rental);

            if (!created)
            {
                _logger.LogInformation($"Unable to create {nameof(Rental)}. Please check model and try again.");
                return BadRequest();
            }

            return CreatedAtAction(nameof(Get), new { id = rental.Id }, _mapper.Map<RentalDto>(rental));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, UpdateRentalDto requestDto, bool clearRentalLines = false)
        {
            var rentalToUpdate = await _rentalRepository.GetAsync(x => x.Id == id, x =>
                x.Include(r => r.RentalLines));

            if (rentalToUpdate == null)
            {
                _logger.LogInformation($"No {nameof(Rental)} was found with id: {id}");
                return NotFound();
            }

            if (clearRentalLines && rentalToUpdate.RentalLines.Any())
            {
                foreach (var rentalLine in rentalToUpdate.RentalLines)
                {
                    await _rentalLineRepository.DeleteAsync(rentalLine.Id);
                }
            }

            _mapper.Map(requestDto, rentalToUpdate);

            bool updated = await _rentalRepository.UpdateAsync(rentalToUpdate);

            if (!updated)
            {
                _logger.LogInformation($"Error updating {nameof(Rental)} id: {id}");
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            bool deleted = await _rentalRepository.DeleteAsync(id);

            if (!deleted)
            {
                _logger.LogInformation($"Unable to find/delete {nameof(Rental)} with id: {id}");
                return NotFound();
            }

            return NoContent();
        }
    }
}
