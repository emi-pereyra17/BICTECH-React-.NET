namespace BicTechBack.src.Core.DTOs
{
    public class PedidoDetalleDTO
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; } 
        public decimal Subtotal { get; set; }
    }
}
