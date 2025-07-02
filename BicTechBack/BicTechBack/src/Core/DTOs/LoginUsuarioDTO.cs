using System.ComponentModel.DataAnnotations;

namespace BicTechBack.src.Core.DTOs
{
    public class LoginUsuarioDTO
    {
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El password es obligatorio.")]
        public string Password { get; set; }
    }
}
