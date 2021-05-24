using AutoMapper;
using Delpin.API.Controllers.v1;
using Delpin.Application.Contracts.v1.ProductItems;
using Delpin.Application.Interfaces;
using Delpin.Domain.Entities;
using Delpin.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;


namespace ApiControllerTest
{
    [TestClass]
    public class ProductItemControllerTest
    {
        public DelpinContext CreateInMemoryDb(string dbname)
        {
            var options = new DbContextOptionsBuilder<DelpinContext>()
                .UseInMemoryDatabase(dbname)
                .Options;
            DelpinContext delpinContext = new DelpinContext(options);
            delpinContext.Database.EnsureCreatedAsync();
            return delpinContext;
        }

        private readonly ProductItemsController pic;
        private Mock<IGenericRepository<ProductItem>> _ItemRepositoryMock = new Mock<IGenericRepository<ProductItem>>();
        private Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private Mock<ILogger<ProductItemsController>> _loggerMock = new Mock<ILogger<ProductItemsController>>();

        public ProductItemControllerTest()
        {
            pic = new ProductItemsController(_ItemRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        //[Fact] //moq plus unit
        //public void TestMethod1()
        //{
        //    var context = CreateInMemoryDb(Guid.NewGuid().ToString());
        //    context.ProductItems.Add(new ProductItem()
        //    {
        //        Id = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        //        IsAvailable = true,
        //        Product = new Product()
        //        {
        //            Name = "test"
        //        }
        //    });
        //    context.SaveChanges();
        //    ProductItemsController _pic = new ProductItemsController(_ItemRepositoryMock.Object, _mapperMock, _loggerMock.Object);
        //    pIC.Get(context);
        //}

        [Fact]//Ren moq
        public async Task GetProductItem_ById_ReturnTrue()
        {
            // Arrange
            var productItemId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var postalCity = new PostalCity()
            {
                PostalCode = "7100",
                City = "Vejle"
            };


            var productItemDto = new ProductItemDto
            {
                Id = productItemId,
                IsAvailable = true,
                LastService = DateTime.Now,
                ProductId = productId,
                PostalCity = postalCity

            };


            //_ItemRepositoryMock.Setup(expression:x => x.GetAsync(productItemId))
            //    .ReturnAsync(productItemDto);

            // Act
            var productItem = await pic.Get(productItemId);

            // Asseret
            Assert.Equals(productItemId, productItem);

        }


    }
}
