using Delpin.MVC.Dto.v1.ProductGroups;
using Delpin.MVC.Dto.v1.ProductItems;
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
        public string Image { get; set; }
        public ProductGroupDto ProductGroup { get; set; }
        public ICollection<ProductItemDto> ProductItems { get; set; }
    }
}
