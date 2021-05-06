using AutoMapper;
using Delpin.Domain.Entities;

namespace Delpin.Application.Contracts.v1.RentalLines
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateRentalLineDto, RentalLine>();
            CreateMap<RentalLine, RentalLineDto>();
        }
    }
}