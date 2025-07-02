using System.ComponentModel.DataAnnotations;

namespace BicTechBack.src.Core.DTOs
{
    public class CrearCarritoDetalleDTO
    {
        [Required(ErrorMessage = "El campo CarritoId es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El CarritoId debe ser un número positivo.")]
        public int CarritoId { get; set; }
        [Required(ErrorMessage = "El campo ProductoId es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ProductoId debe ser un número positivo.")]
        public int ProductoId { get; set; }

    }
}
