using AutoMapper;
using Delpin.Application.Contracts.v1.ProductCategories;
using Delpin.Application.Interfaces;
using Delpin.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public ProductCategoriesController(IGenericRepository<ProductCategory> categoryRepository, IMapper mapper, ILogger<ProductCategoriesController> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductCategoryDto>>> GetAll(string orderBy)
        {
            var categories = await _categoryRepository.GetAllAsync();

            return Ok(_mapper.Map<IReadOnlyList<ProductCategoryDto>>(categories));
        }

        [HttpPost]
        public async Task<ActionResult<ProductCategoryDto>> Create([FromBody] CreateProductCategoryDto requestDto)
        {
            var category = _mapper.Map<ProductCategory>(requestDto);

            bool created = await _categoryRepository.CreateAsync(category);

            if (!created)
                return BadRequest();

            return Ok(_mapper.Map<ProductCategoryDto>(category));
        }
    }
}
