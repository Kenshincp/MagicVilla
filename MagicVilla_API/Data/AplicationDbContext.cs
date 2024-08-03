using MagicVilla_API.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Data
{
    public class AplicationDbContext: DbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) :base(options)
        {
            
        }
        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                { 
                    Id = 1,
                    Name = "El mar y tu",
                    Description = "Un lugar junto al más que nos permite disfrutar",
                    Rate = 100,
                    Population = 5,
                    SquareMeter = 230,
                    ImageUrl = "",
                    Amenity = "",
                    dateCreate = DateTime.Now,
                    dateUpdate = DateTime.Now,
                },
                new Villa()
                {
                    Id = 2,
                    Name = "Momentos serranos",
                    Description = "Un lugar en la sierra, para generar los mejores recuerdos",
                    Rate = 150,
                    Population = 6,
                    SquareMeter = 210,
                    ImageUrl = "",
                    Amenity = "",
                    dateCreate = DateTime.Now,
                    dateUpdate = DateTime.Now,
                }
            );
        }
    }
}
