using AutoMapper;
using Delpin.API.Controllers.v1;
using Delpin.Application.Contracts.v1.ProductCategories;
using Delpin.Application.Interfaces;
using Delpin.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ApiControllerTest
{
    public class ProductCategoriesTest
    {
        private readonly ProductCategoriesController _productCategoriesController;

        private readonly Mock<IGenericRepository<ProductCategory>> _repository =
            new Mock<IGenericRepository<ProductCategory>>();

        private readonly IMapper _mapper = new Mapper(new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        }));

        private readonly Mock<ILogger<ProductCategoriesController>> _logger =
            new Mock<ILogger<ProductCategoriesController>>();

        public ProductCategoriesTest()
        {
            _productCategoriesController = new ProductCategoriesController(_repository.Object, _mapper, _logger.Object);
        }

        [Fact]
        public async Task GetById_ShouldReturnCategory_WhenCategoryExists()
        {
            var categoryId = Guid.NewGuid();
            var categoryName = "Test name";

            var productCategory = new ProductCategory
            {
                Id = categoryId,
                Name = categoryName
            };

            _repository.Setup(x => x.GetAsync(pc => pc.Id == It.IsAny<Guid>(), null))
                .ReturnsAsync(productCategory);

            var category = await _productCategoriesController.Get(categoryId);

            Assert.AreEqual(categoryId, category.Value.Id);
        }
    }
}
