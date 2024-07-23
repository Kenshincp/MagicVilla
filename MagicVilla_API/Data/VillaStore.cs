using MagicVilla_API.Models.Dto;

namespace MagicVilla_API.Data
{
    public static class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto>
        {
            new VillaDto { Id = 1, Name = "Con vista a la pileta" },
            new VillaDto { Id = 2, Name = "Con vista a la playa" },
            new VillaDto { Id = 3, Name = "Con vista a la sierra" },
            new VillaDto { Id = 4, Name = "Con vista a la pradera" }
        };
    }
}
