using AutoMapper;
using Castle.Core.Internal;
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
using System.ComponentModel.DataAnnotations;
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
        public async Task GetById_WhenCategoryExists_ShouldReturnCategory()
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
        public async Task GetById_WhenCategoryDoesNotExist_ShouldReturnNothing()
        {
            var categoryId = Guid.NewGuid();

            _repository.SetupIgnoreArgs(x => x.GetAsync(pc => pc.Id == categoryId, null))
                .ReturnsAsync(() => null);

            var category = await _productCategoriesController.Get(categoryId);
            var result = category.Result as NotFoundResult;

            Assert.Equal(result.StatusCode, StatusCodes.Status404NotFound);
        }


        [Fact]
        public async Task Create_WhenCategoryIsCreated_ShouldReturnCategory()
        {
            var categoryName = "Test";

            var requestDto = new CreateProductCategoryDto()
            {
                Name = categoryName,
                Image = new byte[20]
            };

            _repository.Setup(x => x.CreateAsync(It.IsAny<ProductCategory>()))
                .ReturnsAsync(() => true);

            var response = await _productCategoriesController.Create(requestDto);

            var result = response.Result as CreatedAtActionResult;
            var category = result?.Value as ProductCategoryDto;

            Assert.Equal(categoryName, category?.Name);
            Assert.Equal(StatusCodes.Status201Created, result?.StatusCode);
        }

        [Fact]
        public void CreateProductCategoryDto_HasRequiredNameAttribute_ReturnsTrue()
        {
            var propertyInfo = typeof(CreateProductCategoryDto).GetProperty("Name");

            RequiredAttribute attribute = propertyInfo.GetAttribute<RequiredAttribute>();

            Assert.NotNull(attribute);
        }

        [Fact]
        public async Task Update_WhenCategoryIsUpdated_ShouldReturnNoContent()
        {
            var updateId = Guid.NewGuid();
            var updateDto = new UpdateProductCategoryDto
            {
                Name = "Update",
                Image = new byte[23]
            };

            _repository.SetupIgnoreArgs(x => x.GetAsync(x => x.Id == updateId, null))
                .ReturnsAsync(new ProductCategory { Name = "CategoryToUpdate" });

            _repository.Setup(x => x.UpdateAsync(It.IsAny<ProductCategory>()))
                .ReturnsAsync(() => true);

            var response = await _productCategoriesController.Update(updateId, updateDto);

            var result = response as NoContentResult;

            Assert.Equal(StatusCodes.Status204NoContent, result?.StatusCode);
        }

        [Fact]
        public async Task Update_WhenCategoryDoesNotExist_ShouldReturnNotFound()
        {
            var updateId = Guid.NewGuid();
            var updateDto = new UpdateProductCategoryDto
            {
                Name = "Update",
                Image = new byte[23]
            };

            var response = await _productCategoriesController.Update(updateId, updateDto);
            var result = response as NotFoundResult;

            Assert.Equal(StatusCodes.Status404NotFound, result?.StatusCode);
        }

        [Fact]
        public async Task Delete_WhenCategoryExistsDeletesCategory_ShouldReturnNoContent()
        {
            var deleteId = Guid.NewGuid();
            _repository.Setup(x => x.DeleteAsync(deleteId)).ReturnsAsync(() => true);

            var response = await _productCategoriesController.Delete(deleteId);
            var result = response as NoContentResult;

            Assert.Equal(StatusCodes.Status204NoContent, result?.StatusCode);
        }

        [Fact]
        public async Task Delete_WhenCategoryDoesNotExist_ShouldReturnNotFound()
        {
            var deleteId = Guid.NewGuid();
            _repository.Setup(x => x.DeleteAsync(deleteId)).ReturnsAsync(() => false);

            var response = await _productCategoriesController.Delete(deleteId);
            var result = response as NotFoundResult;

            Assert.Equal(StatusCodes.Status404NotFound, result?.StatusCode);
        }
    }
}
