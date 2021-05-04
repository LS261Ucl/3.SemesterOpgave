using AutoMapper;
using Delpin.Domain.Entities;

namespace Delpin.Application.Contracts.v1.ProductCategories
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductCategory, ProductCategoryDto>();
            CreateMap<CreateProductCategoryDto, ProductCategory>();
            CreateMap<UpdateProductCategoryDto, ProductCategory>();
        }
    }
}