using Delpin.Application.Interfaces;
using Delpin.Domain.Entities;
using System;
using System.Linq;

namespace Delpin.Application.Contracts.v1.ProductGroups
{
    public class ProductGroupOrderBy : IOrderBy<ProductGroup>
    {
        public Func<IQueryable<ProductGroup>, IOrderedQueryable<ProductGroup>> Sorting(string orderBy)
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
