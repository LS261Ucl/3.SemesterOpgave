using Delpin.Domain.Entities;
using System;
using System.Linq;

namespace Delpin.Application.Contracts.v1.ProductItems
{
    public class ProductItemOrderBy
    {
        public Func<IQueryable<ProductItem>, IOrderedQueryable<ProductItem>> Sorting(string orderBy)
        {
            switch (orderBy)
            {
                case "postal":
                    return x => x.OrderBy(p => p.PostalCode);
                case "postalDesc":
                    return x => x.OrderByDescending(p => p.PostalCode);
                default:
                    return null;
            }
        }
    }
}
