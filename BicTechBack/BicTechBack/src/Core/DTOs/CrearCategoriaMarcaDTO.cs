using System.ComponentModel.DataAnnotations;

namespace BicTechBack.src.Core.DTOs
{
    public class CrearCategoriaMarcaDTO
    {
        [Required(ErrorMessage = "El ID de categoria es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de categoria debe ser un número positivo")]
        public int CategoriaId { get; set; }
        [Required(ErrorMessage = "El ID de marca es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de marca debe ser un número positivo")]
        public int MarcaId { get; set; }
    }
}
