using System.ComponentModel.DataAnnotations;

namespace BicTechBack.src.Core.DTOs
{
    public class CrearCarritoDTO
    {
        [Required(ErrorMessage = "El campo 'UsuarioId' es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo 'UsuarioId' debe ser un número entero positivo.")]
        public int UsuarioId { get; set; }
    }
}
