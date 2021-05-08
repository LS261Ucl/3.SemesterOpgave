using Delpin.MVC.Dto.v1.ProductItems;
using System;
using System.Collections.Generic;

namespace Delpin.MVC.Dto.v1.Products
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public byte[] Image { get; set; }
        public ICollection<ProductItemDto> ProductItems { get; set; }
    }
}

