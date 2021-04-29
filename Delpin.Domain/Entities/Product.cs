using System;
using System.Collections.Generic;

namespace Delpin.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public byte[] Image { get; set; }

        public Guid ProductGroupId { get; set; }
        public ProductGroup ProductGroup { get; set; }
        public ICollection<ProductItem> ProductItems { get; set; }
    }
}
