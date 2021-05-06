using System;
using System.ComponentModel.DataAnnotations.Schema;
using Delpin.Domain.Entities;

namespace Delpin.Application.Contracts.v1.ProductItems
{
   public class ProductItemDto
    {
        public Guid Id { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime LastService { get; set; }

     //fix dto
        //public Product Product { get; set; }

      
        public PostalCity PostalCity { get; set; }
    }
}