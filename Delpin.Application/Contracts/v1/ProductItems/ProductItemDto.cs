using Delpin.Domain.Entities;
using System;

namespace Delpin.Application.Contracts.v1.ProductItems
{
    public class ProductItemDto
    {
        public Guid Id { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime LastService { get; set; }
        public PostalCity PostalCity { get; set; }
        public Guid ProductId { get; set; }
    }
}