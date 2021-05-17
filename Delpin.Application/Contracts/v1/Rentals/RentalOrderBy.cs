using Delpin.Application.Interfaces;
using Delpin.Domain.Entities;
using System;
using System.Linq;

namespace Delpin.Application.Contracts.v1.Rentals
{
    public class RentalOrderBy : IOrderBy<Rental>
    {
        public Func<IQueryable<Rental>, IOrderedQueryable<Rental>> Sorting(string orderBy)
        {
            switch (orderBy)
            {
                case "endDate":
                    return x => x.OrderByDescending(r => r.EndDate);
                case "customerId":
                    return x => x.OrderBy(r => r.CustomerId);
                case "postalCode":
                    return x => x.OrderBy(r => r.PostalCode);
                default:
                    return x => x.OrderByDescending(r => r.StartDate);
            }
        }
    }
}