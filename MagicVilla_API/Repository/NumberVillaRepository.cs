using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Repository.IRepository;

namespace MagicVilla_API.Repository
{
    public class NumberVillaRepository : Repository<NumberVilla>, INumberVillaRepository
    {
        private readonly AplicationDbContext _context;

        public NumberVillaRepository(AplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<NumberVilla> Update(NumberVilla entidad)
        {
            entidad.dateUpdate = DateTime.Now;
            _context.Update(entidad);
            await _context.SaveChangesAsync();
            return entidad;
        }
    }
}
