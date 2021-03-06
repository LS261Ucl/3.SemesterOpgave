using AutoMapper;
using Delpin.Application.Contracts.v1.ProductItems;
using Delpin.Application.Interfaces;
using Delpin.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delpin.API.Controllers.v1
{

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductItemsController : ControllerBase
    {
        private readonly IGenericRepository<ProductItem> _itemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductItemsController> _logger;

        public ProductItemsController(IGenericRepository<ProductItem> itemRepository, IMapper mapper,
            ILogger<ProductItemsController> logger)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductItemDto>>> GetAll(string orderBy, Guid? productId)
        {
            var items = await _itemRepository
                .GetAllAsync(productId != null ? x => x.ProductId == productId : null,
                    x => x.Include(pi => pi.PostalCity).Include(p => p.Product),
                    new ProductItemOrderBy().Sorting(orderBy));

            return Ok(_mapper.Map<IReadOnlyList<ProductItemDto>>(items));
        }

        [HttpGet("{id:guid}", Name = "GetProductItem")]
        public async Task<ActionResult<ProductItemDto>> Get(Guid id)
        {
            var item = await _itemRepository.GetAsync(x => x.Id == id,
                x => x.Include(pi => pi.PostalCity).Include(p => p.Product));

            if (item == null)
            {
                _logger.LogInformation($"No {nameof(ProductItem)} was found with id: {id}");
                return NotFound();
            }

            return Ok(_mapper.Map<ProductItemDto>(item));
        }

        [HttpPost]
        public async Task<ActionResult<ProductItemDto>> Create([FromBody] CreateProductItemDto requestDto)
        {
            var item = _mapper.Map<ProductItem>(requestDto);
            bool created = await _itemRepository.CreateAsync(item);

            if (!created)
            {
                _logger.LogInformation($"Unable to create {nameof(ProductItem)}. Please check model and try again.");
                return BadRequest();
            }

            return CreatedAtAction(nameof(Get), new { id = item.Id }, _mapper.Map<ProductItemDto>(item));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, UpdateProductItemDto requestDto)
        {
            var itemToUpdate = await _itemRepository.GetAsync(x => x.Id == id);

            if (itemToUpdate == null)
            {
                _logger.LogInformation($"No {nameof(ProductItem)} was found with id: {id}");
                return NotFound();
            }

            _mapper.Map(requestDto, itemToUpdate);

            bool updated = await _itemRepository.UpdateAsync(itemToUpdate);

            if (!updated)
            {
                _logger.LogInformation($"Error updating {nameof(ProductItem)} id: {id}");
                return BadRequest();
            }

            return NoContent();
        }

        [Authorize(Policy = "IsSuperUser")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            bool deleted = await _itemRepository.DeleteAsync(id);

            if (!deleted)
            {
                _logger.LogInformation($"Unable to find/delete {nameof(ProductItem)} with id: {id}");
                return NotFound();
            }

            return NoContent();
        }
    }
}
