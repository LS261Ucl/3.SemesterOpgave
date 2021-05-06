using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delpin.Domain.Entities;

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
