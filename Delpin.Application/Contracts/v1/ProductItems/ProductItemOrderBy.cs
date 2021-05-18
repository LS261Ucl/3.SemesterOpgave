using Delpin.Application.Interfaces;
using Delpin.Domain.Entities;
using System;
using System.Linq;

namespace Delpin.Application.Contracts.v1.ProductItems
{
    public class ProductItemOrderBy : IOrderBy<ProductItem>
    {
        public Func<IQueryable<ProductItem>, IOrderedQueryable<ProductItem>> Sorting(string orderBy)
        {
            switch (orderBy)
            {
                case "postalDesc":
                    return x => x.OrderByDescending(p => p.PostalCode);
                default:
                    return x => x.OrderBy(p => p.PostalCode);
            }
        }
    }
}
