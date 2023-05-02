using Application.Dtos;
using AutoMapper;
using Domain;

namespace Application.MappingConfigurations;

public class AutoMapperInitializer : Profile
{
    public AutoMapperInitializer()
    {
        
        CreateMap<Municipality, RegisterMunicipalityDto>().ReverseMap();
        CreateMap<Supervisor, RegisterSupervisorDto>().ReverseMap();
        CreateMap<Customer, RegisterCustomerDto>().ReverseMap();
        CreateMap<Employee, RegisterEmployeeDto>().ReverseMap();
        CreateMap<Contractor, RegisterContractorDto>().ReverseMap();

        CreateMap<Cargo, RegisterCargoDto>()
            .ForMember(dest => dest.CargoItems, o => o.MapFrom(src => src.CargoItems))
            .ReverseMap();
        CreateMap<CargoItem, CargoItemDto>().ReverseMap();

        CreateMap<Customer, CustomerViewDto>()
            .ForMember(x => x.Cargoes, o => o.MapFrom(src => src.Cargoes))
            .ReverseMap();
        
        CreateMap<Cargo, CargoViewDto>()
            .ForMember(x => x.CargoItems, o => o.MapFrom(src => src.CargoItems))
            .ReverseMap();

        CreateMap<CargoItem, CargoItemViewDto>().ReverseMap();
    }
}