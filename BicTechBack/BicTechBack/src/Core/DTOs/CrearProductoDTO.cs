using System.ComponentModel.DataAnnotations;

namespace BicTechBack.src.Core.DTOs
{
    public class CrearProductoDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero")]
        public decimal Precio { get; set; }
        [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "La categoría es obligatoria")]
        public int CategoriaId { get; set; }
        [Required(ErrorMessage = "La marca es obligatoria")]
        public int MarcaId { get; set; }
        [Required(ErrorMessage = "El stock es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }
        [Required(ErrorMessage = "La URL de la imagen es obligatoria")]
        [Url(ErrorMessage = "La URL de la imagen no es válida")]
        public string ImagenUrl { get; set; }
    }
}
