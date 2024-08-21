using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Repository.IRepository;

namespace MagicVilla_API.Repository
{
    public class VillaRepository :Repository<Villa>, IVillaRepository
    {
        private readonly AplicationDbContext _context;

        public VillaRepository(AplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Villa> Update(Villa entidad)
        {
            entidad.dateUpdate = DateTime.Now;
            _context.Update(entidad);
            await _context.SaveChangesAsync();
            return entidad;
        }
    }
}
