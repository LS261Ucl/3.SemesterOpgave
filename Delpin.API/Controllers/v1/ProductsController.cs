using AutoMapper;
using Delpin.Application.Contracts.v1.Products;
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
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IGenericRepository<Product> productRepository, IMapper mapper,
            ILogger<ProductsController> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAll(string orderBy, string productGroup)
        {
            var products = await _productRepository
                .GetAllAsync(!string.IsNullOrEmpty(productGroup) ? x => x.ProductGroup.Name == productGroup : null,
                    orderBy: new ProductOrderBy().Sorting(orderBy));

            return Ok(_mapper.Map<IReadOnlyList<ProductDto>>(products));
        }

        [HttpGet("{id:guid}", Name = "GetProduct")]
        public async Task<ActionResult<ProductDto>> Get(Guid id)
        {
            var products = await _productRepository.GetAsync(x => x.Id == id,
                x => x.Include(p => p.ProductItems).ThenInclude(p => p.PostalCity));

            if (products == null)
            {
                _logger.LogInformation($"No {nameof(Product)} was found with id: {id}");
                return NotFound();
            }

            return Ok(_mapper.Map<ProductDto>(products));
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductDto requestDto)
        {
            var product = _mapper.Map<Product>(requestDto);

            bool created = await _productRepository.CreateAsync(product);

            if (!created)
            {
                _logger.LogInformation($"Unable to create {nameof(Product)}. Please check model and try again.");
                return BadRequest();
            }

            return CreatedAtAction(nameof(Get), new { id = product.Id }, _mapper.Map<ProductDto>(product));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, UpdateProductDto requestDto)
        {
            var productToUpdate = await _productRepository.GetAsync(x => x.Id == id);

            if (productToUpdate == null)
            {
                _logger.LogInformation($"No {nameof(Product)} was found with id: {id}");
                return NotFound();
            }

            _mapper.Map(requestDto, productToUpdate);

            bool updated = await _productRepository.UpdateAsync(productToUpdate);

            if (!updated)
            {
                _logger.LogInformation($"Error updating {nameof(Product)} id: {id}");
                return BadRequest();
            }

            return NoContent();
        }

        [Authorize(Policy = "IsSuperUser")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            bool deleted = await _productRepository.DeleteAsync(id);

            if (!deleted)
            {
                _logger.LogInformation($"Unable to find/delete {nameof(Product)} with id: {id}");
                return NotFound();
            }

            return NoContent();
        }


    }
}
