using Delpin.MVC.Dto.v1.ProductItems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Delpin.Mvc.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly Dictionary<string, List<ProductItemDto>> _shoppingCarts = new Dictionary<string, List<ProductItemDto>>();
        private readonly Dictionary<string, DateTime> _createdAt = new Dictionary<string, DateTime>();

        public void AddToShoppingCart(string email, ProductItemDto item)
        {
            if (!_shoppingCarts.ContainsKey(email))
            {
                _shoppingCarts.TryAdd(email, new List<ProductItemDto> { item });
                _createdAt.TryAdd(email, DateTime.UtcNow);
                return;
            }

            var productItems = _shoppingCarts.GetValueOrDefault(email);

            if (productItems == null || productItems.FirstOrDefault(x => x.Id == item.Id) != null)
                return;

            productItems.Add(item);

            _shoppingCarts.TryAdd(email, productItems);
        }

        public void RemoveFromShoppingCart(string email, ProductItemDto item)
        {
            if (_shoppingCarts.GetValueOrDefault(email) == null || _shoppingCarts.GetValueOrDefault(email)?.Count <= 0)
                return;

            var productItems = _shoppingCarts.GetValueOrDefault(email);

            var itemToRemove = productItems?.FirstOrDefault(x => x.Id == item.Id);

            productItems?.Remove(itemToRemove);

            _shoppingCarts[email] = productItems;
        }

        public List<ProductItemDto> GetShoppingCart(string email)
        {
            return _shoppingCarts.GetValueOrDefault(email) ?? new List<ProductItemDto>();
        }

        public Dictionary<string, DateTime> GetCreatedAt()
        {
            return _createdAt;
        }

        public void RemoveShoppingCart(string email)
        {
            _shoppingCarts.Remove(email);
            _createdAt.Remove(email);
        }
    }
}
