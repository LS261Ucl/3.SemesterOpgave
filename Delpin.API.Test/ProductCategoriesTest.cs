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
using System.Collections.Generic;
using System.Linq.Expressions;
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

        //[Fact]
        //public async Task GetAll_ShouldReturnAllCatagories_WhenCatagoriesExist()
        //{
            
        //    var catagoryId = Guid.NewGuid();
        //    var catagoryName = "test";
        //    var catagoryMock = new Mock<ProductCategory>();
        //    List<ProductCategory> categories = new List<ProductCategory>()
        //    {
        //        new ProductCategory{Id = catagoryId, Name = catagoryName}
        //    };

        //    var productCatagoryOrderBy = new ProductCategoryOrderBy();
        //    //Test 1
        //    //_fooRepositoryMock.Setup(o => o.GetAllAsync(It.IsAny<Expression<Func<Foo, int>>>()))
        //     //     .ReturnsAsync(idList);

        //    //_repository.SetupIgnoreArgs(x=>x.GetAllAsync(It.IsAny(y=>y!=null))).ReturnsAsync(categories);
        //    //_repository.SetupIgnoreArgs(x => x.GetAllAsync(y=> y.Id != null,false))
        //    // .ReturnsAsync(categories);

        //    Assert.Collection(categories);
        //}

        private readonly IMapper _mapper;

        [Fact]
        public async Task Create_ShouldReturnCatagory_WhenCatagoryIsCreatet()
        {
            // arrange
            var catagoryId = Guid.NewGuid();
            var catagoryName = "Test";

            // Mapping profile skal udskiftes så det passer til 
            var mockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            var mapper = mockMapper.CreateMapper();

            var requestDto = new ProductCategoryDto()
            {
                Id = catagoryId,
                Name = catagoryName,
            };

            var catagoryMap = _mapper.Map<ProductCategory>(requestDto);
            var productCategory = new ProductCategory()
            {
                Id = catagoryId,
                Name = catagoryName
            };
         
            //Does not create anything?
            _repository.SetupIgnoreArgs(x => x.CreateAsync(catagoryMap));
              // .Returns(Task.FromResult(productCategory)); 
            
               
            // act
            //Task<ProductCategory> category = 
            var catagory = await _productCategoriesController.Get(catagoryId);
            var result = catagory.Result as OkObjectResult;
            Assert.NotNull(result);
            var catagoryResult = result.Value as ProductCategoryDto;

            Assert.Equal(catagoryId, catagoryResult.Id);
        }


    }
}
