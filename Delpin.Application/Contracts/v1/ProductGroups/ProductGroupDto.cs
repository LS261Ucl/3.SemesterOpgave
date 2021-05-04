using Delpin.Application.Contracts.v1.ProductCategories;
using Delpin.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Delpin.Application.Contracts.v1.ProductGroups
{
    public class ProductGroupDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public ProductCategoryDto ProductCategory { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}