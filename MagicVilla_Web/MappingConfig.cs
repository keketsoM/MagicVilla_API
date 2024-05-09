using AutoMapper;
using MagicVilla_Web.Model;
using MagicVilla_Web.Model.Dto;

namespace MagicVilla_Web
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
         
            CreateMap<VillaDto, VillaDtoCreate>().ReverseMap();
            CreateMap<VillaDto, VillaDtoUpdate>().ReverseMap();

            CreateMap<VillaNumberDto, VillaNumberDtoCreate>().ReverseMap();
            CreateMap<VillaNumberDto, VillaNumberDtoUpdate>().ReverseMap();
        }
    }
}
