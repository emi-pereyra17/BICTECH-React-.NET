using BicTechBack.src.Core.DTOs;

namespace BicTechBack.src.Core.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaDTO>> GetAllCategoriasAsync();
        Task<CategoriaDTO> GetCategoriaByIdAsync(int id);
        Task<CategoriaDTO> CreateCategoriaAsync(CrearCategoriaDTO dto);
        Task<CategoriaDTO> UpdateCategoriaAsync(int id, CrearCategoriaDTO dto);
        Task<bool> DeleteCategoriaAsync(int id);
    }
}
