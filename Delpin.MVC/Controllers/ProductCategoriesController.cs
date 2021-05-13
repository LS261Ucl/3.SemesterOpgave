using Delpin.Mvc.Extensions;
using Delpin.Mvc.Models.ProductCategories;
using Delpin.Mvc.Services;
using Delpin.MVC.Dto.v1.ProductCategories;
using Delpin.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delpin.Mvc.Controllers
{
    public class ProductCategoriesController : Controller
    {
        private readonly IHttpService _httpService;
        private readonly ImageConverter _imageConverter;

        public ProductCategoriesController(IHttpService httpService, ImageConverter imageConverter)
        {
            _httpService = httpService;
            _imageConverter = imageConverter;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _httpService.Get<List<ProductCategoryDto>>("ProductCategories", User.GetToken());

            List<ProductCategoryViewModel> productCategoryViewModels = new List<ProductCategoryViewModel>();

            foreach (var category in response.Response)
            {
                string image = string.Empty;

                if (category.Image.Length >= 1)
                {
                    image = _imageConverter.ConvertByteArrayToBase64String(category.Image);
                }

                productCategoryViewModels.Add(new ProductCategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    Image = image
                });
            }

            return View(productCategoryViewModels);
        }

        public IActionResult Create()
        {
            return View(new CreateProductCategoryViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Image")] CreateProductCategoryViewModel productCategoryViewModel)
        {
            CreateProductCategoryDto productCategoryDto = new CreateProductCategoryDto
            {
                Name = productCategoryViewModel.Name,
                Image = await _imageConverter.ConvertImageToByteArray(productCategoryViewModel.Image)
            };

            try
            {
                var response = await _httpService.Create<CreateProductCategoryDto, ProductCategoryDto>("ProductCategories", productCategoryDto, User.GetToken());

                if (!response.Success)
                {
                    return View(productCategoryViewModel);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(productCategoryViewModel);
            }
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var response = await _httpService.Get<ProductCategoryDto>($"ProductCategories/{id}", User.GetToken());

            UpdateProductCategoryViewModel updateCategoryViewModel = new UpdateProductCategoryViewModel
            {
                Name = response.Response.Name,
            };

            if (response.Response.Image.Length > 1)
            {
                updateCategoryViewModel.ImageTemp = response.Response.Image;
            }

            return View(updateCategoryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid id, UpdateProductCategoryViewModel updateCategoryViewModel)
        {
            UpdateProductCategoryDto productCategoryDto = new UpdateProductCategoryDto
            {
                Name = updateCategoryViewModel.Name
            };

            if (updateCategoryViewModel.Image != null && updateCategoryViewModel.Image.Length > 1)
            {
                productCategoryDto.Image = await _imageConverter.ConvertImageToByteArray(updateCategoryViewModel.Image);
                updateCategoryViewModel.ImageTemp = Array.Empty<byte>();
            }

            if (updateCategoryViewModel.ImageTemp != null && updateCategoryViewModel.ImageTemp.Length > 1)
            {
                productCategoryDto.Image = updateCategoryViewModel.ImageTemp;
            }

            var response = await _httpService.Update($"ProductCategories/{id}", productCategoryDto, User.GetToken());

            if (!response.Success)
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _httpService.Delete($"ProductCategories/{id}", User.GetToken());

            if (!response.Success)
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
