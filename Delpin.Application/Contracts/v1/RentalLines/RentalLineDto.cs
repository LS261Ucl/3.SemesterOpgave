using Delpin.Domain.Entities;
using System;

namespace Delpin.Application.Contracts.v1.RentalLines
{
    public class RentalLineDto
    {
        public Guid Id { get; set; }
        // SKAL LAVES OM TIL PRODUCT ITEM DTO
        public ProductItem ProductItem { get; set; }
    }
}
