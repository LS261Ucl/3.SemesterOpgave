using AutoMapper;
using Delpin.Domain.Entities;

namespace Delpin.Application.Contracts.v1.ProductItems
{
    public class MappingProfile : Profile 
    {
        public MappingProfile()
        {

            CreateMap<ProductItem, ProductItemDto>();
            CreateMap<CreateProductItemDto, ProductItem>();
            CreateMap<UpdateProductItemDto, ProductItem>();
        }
    }
}
