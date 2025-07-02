namespace BicTechBack.src.Core.Entities
{
    public class Pedido
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public DateTime FechaPedido { get; set; }
        public decimal Total { get; set; }
        public EstadoPedido Estado { get; set; }
        public string DireccionEnvio { get; set; }

    }

    public enum EstadoPedido
    {
        Pendiente,
        Enviado,
        Entregado,
        Cancelado
    }
}
