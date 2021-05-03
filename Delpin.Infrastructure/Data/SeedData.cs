using System;
using System.Linq;
using Delpin.Domain.Entities;

namespace Delpin.Infrastructure.Data
{
    public class SeedData
    {
        public static void SeedDatabase(DelpinContext context)
        {
            if (!context.ProductCategories.Any() && !context.Rentals.Any())
            {
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
                
                
            }
        }
    }
}