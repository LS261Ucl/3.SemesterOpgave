using System;
using System.Linq;
using Delpin.Application.Interfaces;
using Delpin.Domain.Entities;

namespace Delpin.Application.Contracts.v1.Products
{
    public class ProductOrderBy : IOrderBy<Product>
    {
        public Func<IQueryable<Product>, IOrderedQueryable<Product>> Sorting(string orderBy)
        {
            switch (orderBy)
            {
                case "name":
                    return x => x.OrderBy(p => p.Name);
                case "nameDesc":
                    return x => x.OrderByDescending(p => p.Name);
                case "price":
                    return x => x.OrderBy(p => p.Price);
                case "priceDesc":
                    return x => x.OrderByDescending(p => p.Price);
                default:
                    return null;
            }
        }
    }
}