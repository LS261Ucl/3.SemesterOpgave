using Delpin.Application.Interfaces;
using Delpin.Domain.Entities;
using System;
using System.Linq;

namespace Delpin.Application.Contracts.v1.ProductCategories
{
    public class ProductCategoryOrderBy : IOrderBy<ProductCategory>
    {
        // order by descending names 
        public Func<IQueryable<ProductCategory>, IOrderedQueryable<ProductCategory>> Sorting(string orderBy)
        {
            switch (orderBy)
            {
                case "nameDesc":
                    return x => x.OrderByDescending(p => p.Name);
                default:
                    return x => x.OrderBy(p => p.Name);
            }
        }
    }
}
