using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Delpin.Domain.Entities
{
    public class RentalLine : BaseEntity
    {
        public Guid RentalId { get; set; }
        [ForeignKey(nameof(RentalId))]
        public Rental Rental { get; set; }

        public Guid ProductItemId { get; set; }
        [ForeignKey(nameof(ProductItemId))]
        public ProductItem ProductItem { get; set; }
    }
}