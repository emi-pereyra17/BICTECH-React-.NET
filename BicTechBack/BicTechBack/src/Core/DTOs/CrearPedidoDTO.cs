using System.ComponentModel.DataAnnotations;

namespace BicTechBack.src.Core.DTOs
{
    public class CrearPedidoDTO
    {
        [Required(ErrorMessage = "El campo 'UsuarioId' es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El 'UsuarioId' debe ser un número positivo.")]
        public int UsuarioId { get; set; }
        
        [Required(ErrorMessage = "La dirección de envío es obligatoria.")]
        [StringLength(200, ErrorMessage = "La dirección de envío no puede exceder los 200 caracteres.")]
        public string DireccionEnvio { get; set; }

        [Required(ErrorMessage = "Debe agregar al menos un producto al pedido.")]
        [MinLength(1, ErrorMessage = "Debe agregar al menos un producto al pedido.")]
        public List<CrearPedidoDetalleDTO> Productos { get; set; }
    }
}
