using AutoMapper;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;

namespace MagicVilla_API
{
    public class MappingConfig: Profile
    {
        public MappingConfig()
        {
            //Caso 1 especificando cada escenario del mapeo
            CreateMap<Villa, VillaDto>();
            CreateMap<VillaDto, Villa>();

            //Caso 2 en unsa sola linea podemos identificar ambos casos.
            CreateMap<Villa, VillaCreateDto>().ReverseMap();
            CreateMap<Villa, VillaUpdateDto>().ReverseMap();
        }
    }
}
