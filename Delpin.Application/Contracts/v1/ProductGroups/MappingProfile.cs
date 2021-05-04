using AutoMapper;
using Delpin.Domain.Entities;

namespace Delpin.Application.Contracts.v1.ProductGroups
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductGroup, ProductGroupDto>();
        }
    }
}
