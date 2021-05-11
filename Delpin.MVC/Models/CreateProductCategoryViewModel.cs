using Microsoft.AspNetCore.Http;

namespace Delpin.Mvc.Models
{
    public class CreateProductCategoryViewModel
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }
    }
}
