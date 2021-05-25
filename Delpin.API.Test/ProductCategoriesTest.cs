using AutoMapper;
using Delpin.API.Controllers.v1;
using Delpin.API.Test.Extensions;
using Delpin.Application.Contracts.v1.ProductCategories;
using Delpin.Application.Interfaces;
using Delpin.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Delpin.API.Test
{
    public class ProductCategoriesTest
    {
        private readonly ProductCategoriesController _productCategoriesController;

        private readonly Mock<IGenericRepository<ProductCategory>> _repository =
            new Mock<IGenericRepository<ProductCategory>>();

        private readonly Mock<ILogger<ProductCategoriesController>> _logger =
            new Mock<ILogger<ProductCategoriesController>>();

        public ProductCategoriesTest()
        {
            // Mapping profile skal udskiftes så det passer til 
            var mockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            var mapper = mockMapper.CreateMapper();

            _productCategoriesController = new ProductCategoriesController(_repository.Object, mapper, _logger.Object);
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

            _repository.SetupIgnoreArgs(x => x.GetAsync(pc => pc.Id == categoryId, null))
                .ReturnsAsync(productCategory);

            var category = await _productCategoriesController.Get(categoryId);
            var result = category.Result as OkObjectResult;
            var categoryResult = result!.Value as ProductCategoryDto;

            Assert.Equal(categoryResult!.Id, categoryId);
            Assert.Equal(categoryResult!.Name, categoryName);
        }

        [Fact]
        public async Task GetById_ShouldReturnNothing_WhenCategoryDoesNotExist()
        {
            var categoryId = Guid.NewGuid();

            _repository.SetupIgnoreArgs(x => x.GetAsync(pc => pc.Id == categoryId, null))
                .ReturnsAsync(() => null);

            var category = await _productCategoriesController.Get(categoryId);
            var result = category.Result as NotFoundResult;

            Assert.Equal(result.StatusCode, StatusCodes.Status404NotFound);
        }


        [Fact]
        public async Task Create_ShouldReturnCategory_WhenCategoryIsCreated()
        {
            var categoryName = "Test";

            var requestDto = new CreateProductCategoryDto()
            {
                Name = categoryName
            };

            _repository.Setup(x => x.CreateAsync(new ProductCategory { Name = categoryName }))
                .ReturnsAsync(() => true);

            var response = await _productCategoriesController.Create(requestDto);

            var result = response.Result as CreatedAtActionResult;
            var category = result?.Value as ProductCategoryDto;

            Assert.Equal(categoryName, category?.Name);
        }

        //[Fact]
        //public async Task UpdateById_ShouldReturn_True()
        //{

        //    var catagoryTest1 = new ProductCategory()
        //    {
        //        Id = Guid.NewGuid(),
        //        Name = "Test1"

        //    };

        //    var catagoryTest2 = new ProductCategory()
        //    {
        //        Id = Guid.NewGuid(),
        //        Name = "Test2"
        //    };


        //    _repository.SetupIgnoreArgs(x => x.UpdateAsync(pc => pc.Id == catagoryTest1.Id))
        //        .ReturnsAsync();

        //}

        //[Fact]
        //public async Task DeleteById_ShouldReturnNothing_WhenDeletet()
        //{
        //    var catagoryId = Guid.NewGuid();
        //    var catagoryName = "Test";

        //    var productCatagory = new ProductCategory()
        //    {
        //        Id = catagoryId,
        //        Name = catagoryName
        //    };

        //    _repository.SetupIgnoreArgs(x => x.GetAsync(pc => pc.Id == catagoryId, null))
        //        .ReturnsAsync(productCatagory);

        //    _repository.SetupIgnoreArgs(x => x.DeleteAsync(pc => pc.Id == catagoryId, null))
        //}
    }
}
