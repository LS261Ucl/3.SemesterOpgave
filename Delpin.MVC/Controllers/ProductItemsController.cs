using Delpin.Mvc.Extensions;
using Delpin.MVC.Dto.v1.ProductItems;
using Delpin.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Delpin.Mvc.Controllers
{
    public class ProductItemsController : Controller
    {
        private readonly IHttpService _httpService;

        public ProductItemsController(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public IActionResult Create(Guid productId, string productName)
        {
            ViewData["ProductId"] = productId;
            ViewData["ProductName"] = productName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductItemDto productItemDto)
        {
            try
            {
                var response = await _httpService.Create<CreateProductItemDto, ProductItemDto>("ProductItems", productItemDto, User.GetToken());

                if (!response.Success)
                {
                    return View(productItemDto);
                }

                return RedirectToAction("Details", "Products", new { id = productItemDto.ProductId });
            }
            catch
            {
                return View(productItemDto);
            }
        }

        public async Task<IActionResult> Update(Guid id, Guid productId)
        {
            var response = await _httpService.Get<ProductItemDto>($"ProductItems/{id}", User.GetToken());

            if (!response.Success)
                return RedirectToAction("Error", "Home");

            ViewData["ProductId"] = productId;

            UpdateProductItemDto updateItemDto = new UpdateProductItemDto
            {
                IsAvailable = response.Response.IsAvailable,
                PostalCode = response.Response.PostalCity.PostalCode,
                LastService = response.Response.LastService
            };

            return View(updateItemDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid id, Guid productId, UpdateProductItemDto updateItemDto)
        {
            var response = await _httpService.Update($"ProductItems/{id}", updateItemDto, User.GetToken());

            if (!response.Success)
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("Details", "Products", new { id = productId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var responseItem = await _httpService.Get<ProductItemDto>($"ProductItems/{id}", User.GetToken());

            if (!responseItem.Success)
            {
                return RedirectToAction("Error", "Home");
            }

            var productId = responseItem.Response.ProductId;

            await _httpService.Delete($"ProductItems/{id}", User.GetToken());

            return RedirectToAction("Details", "Products", new { id = productId });
        }
    }
}
