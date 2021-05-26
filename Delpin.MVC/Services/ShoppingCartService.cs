using Delpin.MVC.Dto.v1.ProductItems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Delpin.Mvc.Services
{
    // Service that holds the data of every users shopping cart and created date

    // A better solution would be to store it in the cookies of the user on the client,
    // but due to time limitations it was not implemented like that 

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly Dictionary<string, List<ProductItemDto>> _shoppingCarts = new Dictionary<string, List<ProductItemDto>>();
        private readonly Dictionary<string, DateTime> _createdAt = new Dictionary<string, DateTime>();

        // Adds a product item to a users shopping cart
        // If no shopping cart found it will create a new
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

        // Checks to see if the item is in the shopping cart and will remove it if it is.
        public void RemoveFromShoppingCart(string email, ProductItemDto item)
        {
            if (_shoppingCarts.GetValueOrDefault(email) == null || _shoppingCarts.GetValueOrDefault(email)?.Count <= 0)
                return;

            var productItems = _shoppingCarts.GetValueOrDefault(email);

            var itemToRemove = productItems?.FirstOrDefault(x => x.Id == item.Id);

            productItems?.Remove(itemToRemove);

            _shoppingCarts[email] = productItems;
        }

        // Returns the items from the shopping cart
        public List<ProductItemDto> GetShoppingCart(string email)
        {
            return _shoppingCarts.GetValueOrDefault(email) ?? new List<ProductItemDto>();
        }

        // Getter for the createdAt dictionary
        public Dictionary<string, DateTime> GetCreatedAt()
        {
            return _createdAt;
        }

        // Removes the shopping cart from both Dictionaries
        public void RemoveShoppingCart(string email)
        {
            _shoppingCarts.Remove(email);
            _createdAt.Remove(email);
        }
    }
}
