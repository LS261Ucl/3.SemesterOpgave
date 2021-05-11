using Delpin.Mvc.Extensions;
using Delpin.Mvc.Models;
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

        public ProductCategoriesController(IHttpService httpService)
        {
            _httpService = httpService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _httpService.Get<List<ProductCategoryDto>>("ProductCategories", User.GetToken());

            List<ProductCategoryViewModel> productCategoryViewModels = new List<ProductCategoryViewModel>();

            foreach (var category in response.Response)
            {
                string image = string.Empty;

                if (category.Image == null && category.Image?.Length <= 0)
                {
                    image = Convert.ToBase64String(category.Image);
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
        public async Task<IActionResult> Create(CreateProductCategoryViewModel productCategoryViewModel)
        {
            CreateProductCategoryDto productCategoryDto = new CreateProductCategoryDto
            {
                Name = productCategoryViewModel.Name,
                // fix
                Image = null
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

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _httpService.Delete($"ProductCategories/{id}", User.GetToken());

            if (!response.Success)
            {

            }

            return RedirectToAction(nameof(Index));
        }
    }
}
