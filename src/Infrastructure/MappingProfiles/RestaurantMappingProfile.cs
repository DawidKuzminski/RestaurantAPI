using AutoMapper;
using RestaurantAPI.Core.Dto;
using RestaurantAPI.Core.DTO;
using RestaurantAPI.Core.Entity;

namespace RestaurantAPI.Infrastructure.MappingProfiles;

public class RestaurantMappingProfile : Profile
{
	public RestaurantMappingProfile()
	{
		CreateMap<RestaurantEntity, RestauantDto>()
			.ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
			.ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street))
			.ForMember(m => m.PostalCode, c => c.MapFrom(s => s.Address.PostalCode));

		CreateMap<UpdateRestaurantRequest, RestaurantEntity>();

		CreateMap<CreateRestaurantRequest, RestaurantEntity>()
			.ForMember(r => r.Address, c => c.MapFrom(a => new AddressEntity { City = a.City, Street = a.Street, PostalCode = a.PostalCode }));

		CreateMap<DishEntity, DishDto>();
		CreateMap<DishDto, DishEntity>();
		CreateMap<CreateDishRequest, DishEntity>();
	}
}
