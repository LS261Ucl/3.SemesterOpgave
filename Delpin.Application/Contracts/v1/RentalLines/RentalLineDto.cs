using Delpin.Application.Contracts.v1.ProductItems;
using System;

namespace Delpin.Application.Contracts.v1.RentalLines
{
    public class RentalLineDto
    {
        public Guid Id { get; set; }
        public ProductItemDto ProductItem { get; set; }
    }
}
