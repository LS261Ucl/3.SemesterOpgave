using Delpin.MVC.Dto.v1.ProductItems;
using System.Collections.Generic;

namespace Delpin.Mvc.Services
{
    public interface IShoppingCartService
    {
        void AddToShoppingCart(string email, ProductItemDto item);
        void RemoveFromShoppingCart(string email, ProductItemDto item);
        List<ProductItemDto> GetShoppingCart(string email);
    }
}