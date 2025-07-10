namespace BicTechBack.src.Core.DTOs
{
    public class AgregarProductoPedidoDTO
    {
        public int PedidoId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}
