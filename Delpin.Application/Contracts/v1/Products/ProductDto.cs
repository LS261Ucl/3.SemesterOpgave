using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delpin.Domain.Entities;

namespace Delpin.Application.Contracts.v1.Products
{
    public class ProductDto
    {    
        public string Name { get; set; }
        public decimal Price { get; set; }
        public byte[] Image { get; set; }

        public Guid ProductGroupId { get; set; }
        public ProductGroup ProductGroup { get; set; }
        public ICollection<ProductItem> ProductItems { get; set; }
    }
}

