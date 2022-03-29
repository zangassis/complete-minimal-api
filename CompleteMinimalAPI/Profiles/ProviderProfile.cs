using AutoMapper;
using CompleteMinimalAPI.Models;

namespace CompleteMinimalAPI.Profiles;

public class ProviderProfile : Profile
{
    public ProviderProfile()
    {
        CreateMap<ProviderDto, Provider>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.Name}"));
        CreateMap<ProviderDto, Provider>().ForMember(dest => dest.Email, opt => opt.MapFrom(src => $"{src.Email}"));
        CreateMap<ProviderDto, Provider>().ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => $"{src.PhoneNumber}"));
        CreateMap<ProviderDto, Provider>().ForMember(dest => dest.Active, opt => opt.MapFrom(src => $"{src.Active}"));
    }
}