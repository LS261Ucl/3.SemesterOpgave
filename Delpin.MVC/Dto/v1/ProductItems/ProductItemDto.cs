using Delpin.MVC.Dto.v1.PostalCities;
using System;

namespace Delpin.MVC.Dto.v1.ProductItems
{
    public class ProductItemDto
    {
        public Guid Id { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime LastService { get; set; }
        public PostalCityDto PostalCity { get; set; }
    }
}