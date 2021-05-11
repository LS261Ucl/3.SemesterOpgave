using Delpin.Mvc.Extensions;
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

        public ProductGroupsController(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<IActionResult> Index(string categoryName)
        {
            var response = await _httpService.Get<List<ProductGroupDto>>($"ProductGroups?productCategory={categoryName}",
                User.GetToken());

            return View();
        }
    }
}
