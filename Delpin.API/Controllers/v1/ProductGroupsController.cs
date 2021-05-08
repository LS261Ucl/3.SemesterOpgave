using AutoMapper;
using Delpin.Application.Contracts.v1.ProductGroups;
using Delpin.Application.Interfaces;
using Delpin.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delpin.API.Controllers.v1
{

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductGroupsController : ControllerBase
    {
        private readonly IGenericRepository<ProductGroup> _groupRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductGroupsController> _logger;

        public ProductGroupsController(IGenericRepository<ProductGroup> groupRepository, IMapper mapper,
            ILogger<ProductGroupsController> logger)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductGroupDto>>> GetAll(string orderBy)
        {
            var groups = await _groupRepository
               .GetAllAsync(orderBy: new ProductGroupOrderBy().Sorting(orderBy));

            return Ok(_mapper.Map<IReadOnlyList<ProductGroupDto>>(groups));
        }

        [HttpGet("{id:guid}", Name = "GetProductGroup")]
        public async Task<ActionResult<ProductGroupDto>> Get(Guid id)
        {
            var group = await _groupRepository.GetAsync(x => x.Id == id);

            if (group == null)
            {
                _logger.LogInformation($"No {nameof(ProductGroup)} was found with id: {id}");
                return NotFound();
            }

            return Ok(_mapper.Map<ProductGroupDto>(group));
        }

        [HttpPost]
        public async Task<ActionResult<ProductGroupDto>> Create([FromBody] CreateProductGroupDto requestDto)
        {
            var group = _mapper.Map<ProductGroup>(requestDto);

            bool created = await _groupRepository.CreateAsync(group);

            if (!created)
            {
                _logger.LogInformation($"Unable to create {nameof(ProductGroup)}. Please check model and try again.");
                return BadRequest();
            }

            return CreatedAtAction(nameof(Get), new { id = group.Id }, _mapper.Map<ProductGroup>(group));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update (Guid id, UpdateProductGroupDto requestDto )
        {
            var groupToUpdate = await _groupRepository.GetAsync(x => x.Id == id);

            if (groupToUpdate == null)
            {
                _logger.LogInformation($"No {nameof(ProductGroup)} was found with id: {id}");
                return NotFound();
            }

            _mapper.Map(requestDto, groupToUpdate);

            bool update = await _groupRepository.UpdateAsync(groupToUpdate);

            if (!update)
            {
                _logger.LogInformation($"Error updating {nameof(ProductGroup)} id: {id}");
                return BadRequest();
            }

            return NoContent();
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            bool deleted = await _groupRepository.DeleteAsync(id);

            if (!deleted)
            {
                _logger.LogInformation($"Unable to find/delete {nameof(ProductCategory)} with id: {id}");
                return NotFound();
            }

            return NoContent();
        }
    }
}
