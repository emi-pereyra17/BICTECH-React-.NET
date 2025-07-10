using BicTechBack.src.Core.DTOs;

namespace BicTechBack.src.Core.Interfaces
{
    public interface IPedidoService
    {
        Task<IEnumerable<PedidoDTO>> GetAllPedidosAsync();
        Task<PedidoDTO> GetPedidoByIdAsync(int id);
        Task<PedidoDTO> CreatePedidoAsync(CrearPedidoDTO dto);
        Task<PedidoDTO> UpdatePedidoAsync(int id, CrearPedidoDTO dto);
        Task<PedidoDTO> AgregarProductoAlPedidoAsync(AgregarProductoPedidoDTO dto);
        Task<bool> DeletePedidoAsync(int id);
    }
}
