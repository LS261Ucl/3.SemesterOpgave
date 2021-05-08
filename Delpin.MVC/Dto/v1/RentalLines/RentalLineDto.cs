using Delpin.MVC.Dto.v1.ProductItems;
using System;

namespace Delpin.MVC.Dto.v1.RentalLines
{
    public class RentalLineDto
    {
        public Guid Id { get; set; }
        public ProductItemDto ProductItem { get; set; }
    }
}
