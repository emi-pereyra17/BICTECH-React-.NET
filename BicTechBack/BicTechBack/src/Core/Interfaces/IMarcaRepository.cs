using BicTechBack.src.Core.Entities;

namespace BicTechBack.src.Core.Interfaces
{
    public interface IMarcaRepository
    {
        Task<IEnumerable<Marca>> GetAllAsync();
        Task<Marca?> GetByIdAsync(int id);
        Task<Marca> AddAsync(Marca marca);
        Task<Marca> UpdateAsync(Marca marca);
        Task<bool> DeleteAsync(int id);
    }
}
