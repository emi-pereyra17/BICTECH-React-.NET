using System.ComponentModel.DataAnnotations;

namespace BicTechBack.src.Core.DTOs
{
    public class CrearMarcaDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres")]
        public string Nombre { get; set; }
    }
}
