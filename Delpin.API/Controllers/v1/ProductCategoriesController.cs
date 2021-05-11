using AutoMapper;
using Delpin.Application.Contracts.v1.ProductCategories;
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
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IGenericRepository<ProductCategory> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductCategoriesController> _logger;

        public ProductCategoriesController(IGenericRepository<ProductCategory> categoryRepository, IMapper mapper,
            ILogger<ProductCategoriesController> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductCategoryDto>>> GetAll(string orderBy)
        {
            var categories = await _categoryRepository
                .GetAllAsync(orderBy: new ProductCategoryOrderBy().Sorting(orderBy));

            return Ok(_mapper.Map<IReadOnlyList<ProductCategoryDto>>(categories));
        }

        [HttpGet("{id:guid}", Name = "GetProductCategory")]
        public async Task<ActionResult<ProductCategoryDto>> Get(Guid id)
        {
            var category = await _categoryRepository
                .GetAsync(x => x.Id == id, x => x.Include(p => p.ProductGroups));

            if (category == null)
            {
                _logger.LogInformation($"No {nameof(ProductCategory)} was found with id: {id}");
                return NotFound();
            }

            return Ok(_mapper.Map<ProductCategoryDto>(category));
        }

        [Authorize(Policy = "IsSuperUser")]
        [HttpPost]
        public async Task<ActionResult<ProductCategoryDto>> Create([FromBody] CreateProductCategoryDto requestDto)
        {
            var category = _mapper.Map<ProductCategory>(requestDto);

            bool created = await _categoryRepository.CreateAsync(category);

            if (!created)
            {
                _logger.LogInformation(
                    $"Unable to create {nameof(ProductCategory)}. Please check model and try again.");
                return BadRequest();
            }

            return CreatedAtAction(nameof(Get), new { id = category.Id }, _mapper.Map<ProductCategoryDto>(category));
        }

        [Authorize(Policy = "IsSuperUser")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, UpdateProductCategoryDto requestDto)
        {
            var categoryToUpdate = await _categoryRepository.GetAsync(x => x.Id == id);

            if (categoryToUpdate == null)
            {
                _logger.LogInformation($"No {nameof(ProductCategory)} was found with id: {id}");
                return NotFound();
            }

            _mapper.Map(requestDto, categoryToUpdate);

            bool updated = await _categoryRepository.UpdateAsync(categoryToUpdate);

            if (!updated)
            {
                _logger.LogInformation($"Error updating {nameof(ProductCategory)} id: {id}");
                return BadRequest();
            }

            return NoContent();
        }

        [Authorize(Policy = "IsSuperUser")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            bool deleted = await _categoryRepository.DeleteAsync(id);

            if (!deleted)
            {
                _logger.LogInformation($"Unable to find/delete {nameof(ProductCategory)} with id: {id}");
                return NotFound();
            }

            return NoContent();
        }
    }
}