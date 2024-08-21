using MagicVilla_API.Models;

namespace MagicVilla_API.Repository.IRepository
{
    public interface INumberVillaRepository :IRepository<NumberVilla>
    {
        Task<NumberVilla> Update(NumberVilla entidad);
    }
}
