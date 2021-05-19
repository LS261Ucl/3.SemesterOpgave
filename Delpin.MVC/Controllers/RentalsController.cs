using Delpin.Mvc.Extensions;
using Delpin.Mvc.Models.Rentals;
using Delpin.Mvc.Services;
using Delpin.MVC.Dto.v1.ProductItems;
using Delpin.MVC.Dto.v1.Products;
using Delpin.MVC.Dto.v1.RentalLines;
using Delpin.MVC.Dto.v1.Rentals;
using Delpin.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delpin.Mvc.Controllers
{
    public class RentalsController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IHttpService _httpService;

        public RentalsController(IShoppingCartService shoppingCartService, IHttpService httpService)
        {
            _shoppingCartService = shoppingCartService;
            _httpService = httpService;
        }

        [HttpGet]
        public async Task<IActionResult> ShoppingCart(string email)
        {
            var shoppingCart = _shoppingCartService.GetShoppingCart(email);

            if (shoppingCart == null || shoppingCart.Count <= 0)
                return View(new CreateRentalViewModel());

            var rental = new CreateRentalViewModel();

            foreach (var item in shoppingCart)
            {
                rental.RentalLines.Add(new CreateRentalLineDto { ProductItemId = item.Id });

                var productResponse = await _httpService.Get<ProductDto>($"Products/{item.ProductId}", User.GetToken());

                rental.Products.Add(productResponse.Response);
            }

            return View(rental);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShoppingCart(CreateRentalViewModel rentalViewModel)
        {
            CreateRentalDto rentalDto = new CreateRentalDto
            {
                CustomerId = rentalViewModel.CustomerId,
                StartDate = rentalViewModel.StartDate,
                EndDate = rentalViewModel.EndDate,
                Address = rentalViewModel.Address,
                PostalCode = rentalViewModel.PostalCode,
                RentalLines = new List<CreateRentalLineDto>()
            };

            foreach (var item in _shoppingCartService.GetShoppingCart(User.GetEmail()))
            {
                rentalDto.RentalLines.Add(new CreateRentalLineDto { ProductItemId = item.Id });

                var updateProductItem = new UpdateProductItemDto
                {
                    IsAvailable = false,
                    PostalCode = rentalViewModel.PostalCode,
                    LastService = item.LastService
                };

                await _httpService.Update($"ProductItems/{item.Id}", updateProductItem, User.GetToken());
            }

            try
            {
                var response = await _httpService.Create<CreateRentalDto, RentalDto>("Rentals", rentalDto, User.GetToken());

                if (!response.Success)
                {
                    return View(rentalViewModel);
                }

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View(rentalViewModel);
            }
        }
    }
}
