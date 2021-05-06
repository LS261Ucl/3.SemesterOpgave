using System;

namespace Delpin.Application.Contracts.v1.ProductItems
{
    public class UpdateProductItemDto
    {
        public bool IsAvailable { get; set; }
        public DateTime LastService { get; set; }
        public string PostalCode { get; set; }
    }
}
