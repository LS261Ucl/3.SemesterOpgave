using System;
using System.Collections.Generic;

namespace Delpin.Domain.Entities
{
    public class ProductGroup : BaseEntity
    {
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public Guid ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}