using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Delpin.Domain.Entities
{
    public class ProductItem : BaseEntity
    {
        public bool IsAvailable { get; set; }
        public DateTime LastService { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public string PostalCode { get; set; }
        [ForeignKey(nameof(PostalCode))]
        public PostalCity PostalCity { get; set; }
    }
}