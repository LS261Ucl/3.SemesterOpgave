using System.ComponentModel.DataAnnotations;

namespace Delpin.Domain.Entities
{
    public class PostalCity
    {
        [Key]
        public string PostalCode { get; set; }

        public string City { get; set; }
    }
}