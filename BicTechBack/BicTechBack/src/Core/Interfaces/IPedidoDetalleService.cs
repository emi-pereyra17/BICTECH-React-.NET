using BicTechBack.src.Core.DTOs;

namespace BicTechBack.src.Core.Interfaces
{
    public interface IPedidoDetalleService
    {
        Task<IEnumerable<PedidoDetalleDTO>> GetAllPDAsync();
        Task<PedidoDetalleDTO> GetPDByIdAsync(int id);
        Task<PedidoDetalleDTO> AddPDAsync(CrearPedidoDetalleDTO dto);
        Task<PedidoDetalleDTO> UpdatePDAsync(int id, CrearPedidoDetalleDTO dto);
        Task<bool> DeletePDAsync(int id);
    }
}
