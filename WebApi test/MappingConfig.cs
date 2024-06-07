using AutoMapper;
using WebApi_test.Model;
using WebApi_test.Model.Dto;

namespace WebApi_test
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDto>().ReverseMap();
            CreateMap<Villa, VillaDtoCreate>().ReverseMap();
            CreateMap<Villa, VillaDtoUpdate>().ReverseMap();

            CreateMap<VillaNumber, VillaNumberDto>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberDtoCreate>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberDtoUpdate>().ReverseMap();
            CreateMap<ApplicationUser, UserDto>().ReverseMap();
        }
    }
}
