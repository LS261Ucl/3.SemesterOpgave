using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delpin.Application.Contracts.v1.ProductItems
{
    public class UpdateProductItemDto
    {
        public bool IsAvailable { get; set; }
        public DateTime LastService { get; set; }
        public Guid ProductId { get; set; }
        public string PostalCode { get; set; }
       
    }
}
