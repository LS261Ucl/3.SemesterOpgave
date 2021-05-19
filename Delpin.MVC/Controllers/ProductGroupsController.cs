using Delpin.Mvc.Extensions;
using Delpin.Mvc.Helpers;
using Delpin.Mvc.Models.ProductGroups;
using Delpin.Mvc.Services;
using Delpin.MVC.Dto.v1.ProductCategories;
using Delpin.MVC.Dto.v1.ProductGroups;
using Delpin.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delpin.Mvc.Controllers
{
    public class ProductGroupsController : Controller
    {
        private readonly IHttpService _httpService;
        private readonly ImageConverter _imageConverter;

        public ProductGroupsController(IHttpService httpService, ImageConverter imageConverter)
        {
            _httpService = httpService;
            _imageConverter = imageConverter;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string categoryName)
        {
            var response = await _httpService.Get<List<ProductGroupDto>>($"ProductGroups?productCategory={categoryName}",
                User.GetToken());

            var productGroupViewModels = new List<ProductGroupViewModel>();

            foreach (var productGroup in response.Response)
            {
                string image = string.Empty;

                if (productGroup.Image.Length >= 1)
                {
                    image = _imageConverter.ConvertByteArrayToBase64String(productGroup.Image);
                }

                productGroupViewModels.Add(new ProductGroupViewModel
                {
                    Id = productGroup.Id,
                    Name = productGroup.Name,
                    Image = image,
                    ProductCategory = productGroup.ProductCategory
                });
            }



            return View(productGroupViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string categoryName, string categoryId)
        {
            if (!string.IsNullOrEmpty(categoryName) && !string.IsNullOrEmpty(categoryId))
            {
                ViewData["ProductCategoryName"] = categoryName;
                ViewData["ProductCategoryId"] = categoryId;
            }
            else
            {
                var categoryResponse = await _httpService.Get<List<ProductCategoryDto>>($"ProductCategories", User.GetToken());
                ViewData["Categories"] = categoryResponse.Response;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductGroupViewModel productGroupViewModel)
        {
            var productGroupDto = new CreateProductGroupDto
            {
                Name = productGroupViewModel.Name,
                Image = await _imageConverter.ConvertImageToByteArray(productGroupViewModel.Image),
                ProductCategoryId = productGroupViewModel.ProductCategoryId
            };

            try
            {
                var response = await _httpService.Create<CreateProductGroupDto, ProductGroupDto>("ProductGroups", productGroupDto, User.GetToken());

                if (!response.Success)
                {
                    return View(productGroupViewModel);
                }

                var categoryResponse =
                    await _httpService.Get<ProductCategoryDto>(
                        $"ProductCategories/{productGroupViewModel.ProductCategoryId}", User.GetToken());

                return RedirectToAction("Index", "ProductGroups", new RouteValueMaker().GetRoute("categoryName", categoryResponse.Response.Name));
            }
            catch
            {
                return View(productGroupViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var response = await _httpService.Get<ProductGroupDto>($"ProductGroups/{id}", User.GetToken());

            var updateGroupViewModel = new UpdateProductGroupViewModel
            {
                Name = response.Response.Name,
                ProductCategoryId = response.Response.ProductCategory.Id
            };

            var categoryResponse = await _httpService.Get<List<ProductCategoryDto>>($"ProductCategories", User.GetToken());
            ViewData["Categories"] = categoryResponse.Response;

            if (response.Response.Image.Length > 1)
            {
                updateGroupViewModel.ImageTemp = response.Response.Image;
            }

            return View(updateGroupViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid id, UpdateProductGroupViewModel updateGroupViewModel)
        {
            var productGroupDto = new UpdateProductGroupDto
            {
                Name = updateGroupViewModel.Name,
                ProductCategoryId = updateGroupViewModel.ProductCategoryId
            };

            if (updateGroupViewModel.Image != null && updateGroupViewModel.Image.Length > 1)
            {
                productGroupDto.Image = await _imageConverter.ConvertImageToByteArray(updateGroupViewModel.Image);
                updateGroupViewModel.ImageTemp = Array.Empty<byte>();
            }

            if (updateGroupViewModel.ImageTemp != null && updateGroupViewModel.ImageTemp.Length > 1)
            {
                productGroupDto.Image = updateGroupViewModel.ImageTemp;
            }

            var response = await _httpService.Update($"ProductGroups/{id}", productGroupDto, User.GetToken());

            if (!response.Success)
            {
                return RedirectToAction("Error", "Home");
            }

            var categoryResponse =
                await _httpService.Get<ProductCategoryDto>(
                    $"ProductCategories/{updateGroupViewModel.ProductCategoryId}", User.GetToken());

            return RedirectToAction("Index", "ProductGroups", new RouteValueMaker().GetRoute("categoryName", categoryResponse.Response.Name));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _httpService.Delete($"ProductGroups/{id}", User.GetToken());

            if (!response.Success)
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("Index", "ProductGroups");
        }
    }
}
