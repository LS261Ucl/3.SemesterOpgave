using System.Collections.Generic;

namespace Delpin.Domain.Entities
{
    public class ProductCategory : BaseEntity
    {
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public ICollection<ProductGroup> ProductGroups { get; set; }
    }
}