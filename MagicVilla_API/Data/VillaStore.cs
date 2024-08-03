using MagicVilla_API.Models.Dto;

namespace MagicVilla_API.Data
{
    public static class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto>
        {
            new VillaDto { Id = 1, Name = "Con vista a la pileta", Population = 5, SquareMeter = 230 },
            new VillaDto { Id = 2, Name = "Con vista a la playa", Population = 6, SquareMeter = 290 },
            new VillaDto { Id = 3, Name = "Con vista a la sierra", Population = 8, SquareMeter = 380 },
            new VillaDto { Id = 4, Name = "Con vista a la pradera", Population = 3, SquareMeter = 120 }
        };
    }
}
