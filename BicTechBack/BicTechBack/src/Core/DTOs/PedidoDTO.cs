namespace BicTechBack.src.Core.DTOs
{
    public class PedidoDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaPedido { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
        public string DireccionEnvio { get; set; }
        public List<PedidoDetalleDTO> Productos { get; set; }
    }
}
