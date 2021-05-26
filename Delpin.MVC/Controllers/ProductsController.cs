using Delpin.Mvc.Extensions;
using Delpin.Mvc.Helpers;
using Delpin.Mvc.Models.Products;
using Delpin.Mvc.Services;
using Delpin.MVC.Dto.v1.ProductGroups;
using Delpin.MVC.Dto.v1.Products;
using Delpin.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delpin.Mvc.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IHttpService _httpService;
        private readonly ImageConverter _imageConverter;
        private readonly IShoppingCartService _shoppingCartService;

        public ProductsController(IHttpService httpService, ImageConverter imageConverter, IShoppingCartService shoppingCartService)
        {
            _httpService = httpService;
            _imageConverter = imageConverter;
            _shoppingCartService = shoppingCartService;
        }

        // GET: ProductsController
        [HttpGet]
        public async Task<IActionResult> Index(string groupName)
        {
            var response = await _httpService.Get<List<ProductDto>>($"products?productGroup={groupName}", User.GetToken());

            if (!response.Success)
                RedirectToAction("Index", "Home");

            List<ProductViewModel> productViewModels = new List<ProductViewModel>();

            foreach (var product in response.Response)
            {
                string image = string.Empty;

                if (product.Image?.Length > 0)
                {
                    image = _imageConverter.ConvertByteArrayToBase64String(product.Image);
                }

                productViewModels.Add(new ProductViewModel
                {
                    Id = product.Id,
                    Description = product.Description,
                    Name = product.Name,
                    Price = product.Price,
                    Image = image,
                    ProductGroup = product.ProductGroup
                });
            }

            return View(productViewModels);
        }

        // GET: ProductsController/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var response = await _httpService.Get<ProductDto>($"products/{id}", User.GetToken());

            string image = string.Empty;

            if (response.Response.Image != null && response?.Response?.Image?.Length > 0)
            {
                image = _imageConverter.ConvertByteArrayToBase64String(response.Response.Image);
            }

            ProductViewModel productViewModel = new ProductViewModel
            {
                Id = response.Response.Id,
                Name = response.Response.Name,
                Description = response.Response.Description,
                Price = response.Response.Price,
                Image = image,
                ProductGroup = response.Response.ProductGroup,
                ProductItems = response.Response.ProductItems
            };

            var shoppingCart = _shoppingCartService.GetShoppingCart(User.GetEmail());

            if (shoppingCart.Count > 0)
            {
                ViewData["ShoppingCart"] = _shoppingCartService.GetShoppingCart(User.GetEmail());
            }

            return View(productViewModel);
        }

        // GET: ProductsController/Create
        [HttpGet]
        [Authorize(Policy = "IsSuperUser")]
        public async Task<IActionResult> Create(string groupName, string groupId)
        {
            if (!string.IsNullOrEmpty(groupName) && !string.IsNullOrEmpty(groupId))
            {
                ViewData["ProductGroupName"] = groupName;
                ViewData["ProductGroupId"] = groupId;
            }
            else
            {
                var categoryResponse = await _httpService.Get<List<ProductGroupDto>>($"ProductGroups", User.GetToken());
                ViewData["Groups"] = categoryResponse.Response;
            }

            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "IsSuperUser")]
        public async Task<IActionResult> Create(CreateProductViewModel productViewModel)
        {
            CreateProductDto productDto = new CreateProductDto
            {
                Name = productViewModel.Name,
                Price = productViewModel.Price,
                Description = productViewModel.Description,
                ProductGroupId = productViewModel.ProductGroupId,
                Image = await _imageConverter.ConvertImageToByteArray(productViewModel.Image),
            };

            try
            {
                var response = await _httpService.Create<CreateProductDto, ProductDto>("Products", productDto, User.GetToken());

                if (!response.Success)
                {
                    return View(productViewModel);
                }

                var groupResponse =
                    await _httpService.Get<ProductGroupDto>(
                        $"ProductGroups/{productViewModel.ProductGroupId}", User.GetToken());

                return RedirectToAction("Index", "Products", new RouteValueMaker().GetRoute("groupName", groupResponse.Response.Name));
            }
            catch
            {
                return View(productViewModel);
            }
        }

        [HttpGet]
        [Authorize(Policy = "IsSuperUser")]
        public async Task<IActionResult> Update(Guid id)
        {
            var response = await _httpService.Get<ProductDto>($"Products/{id}", User.GetToken());

            UpdateProductViewModel updateProductViewModel = new UpdateProductViewModel
            {
                Name = response.Response.Name,
                Price = response.Response.Price,
                Description = response.Response.Description,
                ProductGroupId = response.Response.ProductGroup.Id,
            };

            var groupResponse = await _httpService.Get<List<ProductGroupDto>>($"ProductGroups", User.GetToken());
            ViewData["Groups"] = groupResponse.Response;

            if (response.Response.Image.Length > 1)
            {
                updateProductViewModel.ImageTemp = response.Response.Image;
            }

            return View(updateProductViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "IsSuperUser")]
        public async Task<IActionResult> Update(Guid id, UpdateProductViewModel updateProductViewModel)
        {
            UpdateProductDto productDto = new UpdateProductDto
            {
                Name = updateProductViewModel.Name,
                Description = updateProductViewModel.Description,
                Price = updateProductViewModel.Price,
                ProductGroupId = updateProductViewModel.ProductGroupId
            };

            if (updateProductViewModel.Image != null && updateProductViewModel.Image.Length > 1)
            {
                productDto.Image = await _imageConverter.ConvertImageToByteArray(updateProductViewModel.Image);
                updateProductViewModel.ImageTemp = Array.Empty<byte>();
            }

            if (updateProductViewModel.ImageTemp != null && updateProductViewModel.ImageTemp.Length > 1)
            {
                productDto.Image = updateProductViewModel.ImageTemp;
            }

            var response = await _httpService.Update($"Products/{id}", productDto, User.GetToken());

            if (!response.Success)
            {
                return RedirectToAction("Error", "Home");
            }

            var groupResponse =
                await _httpService.Get<ProductGroupDto>(
                    $"ProductGroups/{updateProductViewModel.ProductGroupId}", User.GetToken());

            return RedirectToAction("Index", "Products", new RouteValueMaker().GetRoute("groupName", groupResponse.Response.Name));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "IsSuperUser")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _httpService.Delete($"Products/{id}", User.GetToken());

            if (!response.Success)
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("Index", "Products");
        }
    }
}
