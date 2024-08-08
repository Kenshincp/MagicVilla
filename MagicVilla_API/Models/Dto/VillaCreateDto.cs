using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Models.Dto
{
    public class VillaCreateDto
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public double Rate { get; set; }
        public int Population { get; set; }
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
        public int SquareMeter { get; set; }

    }
}
