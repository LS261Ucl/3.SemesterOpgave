using Delpin.MVC.Dto.v1.ProductItems;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Delpin.Mvc.Models.Products
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public ICollection<ProductItemDto> ProductItems { get; set; }
    }
}
