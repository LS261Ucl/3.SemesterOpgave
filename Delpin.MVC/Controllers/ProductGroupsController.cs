using Delpin.Mvc.Extensions;
using Delpin.Mvc.Models.ProductGroups;
using Delpin.Mvc.Services;
using Delpin.MVC.Dto.v1.ProductGroups;
using Delpin.MVC.Services;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Index(string categoryName)
        {
            var response = await _httpService.Get<List<ProductGroupDto>>($"ProductGroups?productCategory={categoryName}",
                User.GetToken());

            List<ProductGroupViewModel> productGroupViewModels = new List<ProductGroupViewModel>();

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
                    Products = productGroup.Products,
                    ProductCategory = productGroup.ProductCategory
                });
            }

            return View(productGroupViewModels);
        }
    }
}
