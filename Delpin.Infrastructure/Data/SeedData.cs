using Delpin.Domain.Entities;
using System;
using System.Linq;

namespace Delpin.Infrastructure.Data
{
    public class SeedData
    {
        public static void SeedDatabase(DelpinContext context)
        {
            if (!context.ProductCategories.Any() && !context.Rentals.Any())
            {
                var postal1 = new PostalCity
                {
                    PostalCode = "7100",
                    City = "Vejle"
                };

                var postal2 = new PostalCity
                {
                    PostalCode = "8000",
                    City = "Aarhus"
                };

                var postal3 = new PostalCity
                {
                    PostalCode = "7000",
                    City = "Fredericia"
                };

                context.PostalCities.AddRange(postal1, postal2, postal3);

                var productCategory1 = new ProductCategory
                {
                    Id = Guid.NewGuid(),
                    Name = "Betonmateriel"
                };

                var productCategory2 = new ProductCategory
                {
                    Id = Guid.NewGuid(),
                    Name = "Bygningsmateriel"
                };

                context.ProductCategories.AddRange(productCategory1, productCategory2);

                var productGroup1 = new ProductGroup
                {
                    Id = Guid.NewGuid(),
                    Name = "Betonjutter",
                    ProductCategory = productCategory1
                };

                var productGroup2 = new ProductGroup
                {
                    Id = Guid.NewGuid(),
                    Name = "Betonfræser",
                    ProductCategory = productCategory1
                };

                var productGroup3 = new ProductGroup
                {
                    Id = Guid.NewGuid(),
                    Name = "Affugter",
                    ProductCategory = productCategory2
                };

                context.ProductGroups.AddRange(productGroup1, productGroup2, productGroup3);

                var product1 = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "AK",
                    Price = 100,
                    ProductGroup = productGroup1
                };

                var product2 = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Schwamborn - BEF 201",
                    Price = 970,
                    ProductGroup = productGroup2
                };

                var product3 = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Dantherm - CDT 22",
                    Price = 115,
                    ProductGroup = productGroup3
                };

                var product4 = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Dantherm - CDT 30 MK III",
                    Price = 145,
                    ProductGroup = productGroup3
                };

                context.Products.AddRange(product1, product2, product3, product4);

                var productItem1 = new ProductItem
                {
                    Id = Guid.NewGuid(),
                    IsAvailable = true,
                    LastService = DateTime.UtcNow,
                    Product = product1,
                    PostalCity = postal1
                };

                var productItem2 = new ProductItem
                {
                    Id = Guid.NewGuid(),
                    IsAvailable = true,
                    LastService = DateTime.UtcNow,
                    Product = product2,
                    PostalCity = postal2
                };

                var productItem3 = new ProductItem
                {
                    Id = Guid.NewGuid(),
                    IsAvailable = true,
                    LastService = DateTime.UtcNow,
                    Product = product3,
                    PostalCity = postal3
                };

                var productItem4 = new ProductItem
                {
                    Id = Guid.NewGuid(),
                    IsAvailable = true,
                    LastService = DateTime.UtcNow,
                    Product = product4,
                    PostalCity = postal1
                };

                var productItem5 = new ProductItem
                {
                    Id = Guid.NewGuid(),
                    IsAvailable = true,
                    LastService = DateTime.UtcNow,
                    Product = product1,
                    PostalCity = postal2
                };

                var productItem6 = new ProductItem
                {
                    Id = Guid.NewGuid(),
                    IsAvailable = true,
                    LastService = DateTime.UtcNow,
                    Product = product2,
                    PostalCity = postal3
                };

                context.ProductItems.AddRange(productItem1, productItem2, productItem3, productItem4, productItem5, productItem6);

                var rental1 = new Rental
                {
                    Id = Guid.NewGuid(),
                    CustomerId = "000001",
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(7),
                    Address = "Horsensgade 4 2. th",
                    PostalCity = postal2,
                };

                var rental2 = new Rental
                {
                    Id = Guid.NewGuid(),
                    CustomerId = "000002",
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(7),
                    Address = "Vejlevej 120",
                    PostalCity = postal3,
                };

                context.Rentals.AddRange(rental1, rental2);

                var rentalLine1 = new RentalLine
                {
                    Id = Guid.NewGuid(),
                    ProductItem = productItem3,
                    Rental = rental1
                };

                var rentalLine2 = new RentalLine
                {
                    Id = Guid.NewGuid(),
                    ProductItem = productItem1,
                    Rental = rental1
                };

                var rentalLine3 = new RentalLine
                {
                    Id = Guid.NewGuid(),
                    ProductItem = productItem4,
                    Rental = rental2
                };

                context.RentalLines.AddRange(rentalLine1, rentalLine2, rentalLine3);

                context.SaveChanges();
            }
        }
    }
}