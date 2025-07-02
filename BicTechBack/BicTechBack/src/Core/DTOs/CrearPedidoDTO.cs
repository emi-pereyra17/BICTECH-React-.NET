using System.ComponentModel.DataAnnotations;

namespace BicTechBack.src.Core.DTOs
{
    public class CrearPedidoDTO
    {
        [Required(ErrorMessage = "El campo 'UsuarioId' es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El 'UsuarioId' debe ser un número positivo.")]
        public int UsuarioId { get; set; }
        [Required(ErrorMessage = "El monto total es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto total debe ser un número positivo mayor que cero.")]
        public decimal Total { get; set; }
        [Required(ErrorMessage = "La dirección de envío es obligatoria.")]
        [StringLength(200, ErrorMessage = "La dirección de envío no puede exceder los 200 caracteres.")]
        public string DireccionEnvio { get; set; }

    }
}
