using AutoMapper;
using Delpin.Domain.Entities;

namespace Delpin.Application.Contracts.v1.Rentals
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateRentalDto, Rental>();
            CreateMap<UpdateRentalDto, Rental>();
            CreateMap<Rental, RentalDto>();
        }
    }
}