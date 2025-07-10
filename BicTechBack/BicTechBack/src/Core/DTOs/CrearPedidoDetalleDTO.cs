using System.ComponentModel.DataAnnotations;

namespace BicTechBack.src.Core.DTOs
{
    public class CrearPedidoDetalleDTO
    {
        [Required(ErrorMessage = "El campo 'ProductoId' es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El 'ProductoId' debe ser un número positivo.")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero.")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal Precio { get; set; }
    }
}

