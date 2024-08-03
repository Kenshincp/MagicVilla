using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla_API.Models
{
    public class Villa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public double Rate { get; set; }
        public int Population { get; set; }
        public int SquareMeter { get; set; }
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
        public DateTime dateCreate { get; set; }
        public DateTime dateUpdate { get; set; }

    }
}
