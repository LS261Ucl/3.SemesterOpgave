using System;

namespace Delpin.MVC.Dto.v1.Products
{
    public class UpdateProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Guid ProductGroupId { get; set; }
        public byte[] Image { get; set; }
    }
}
