namespace BicTechBack.src.Core.DTOs
{
    public class LoginResultDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; } 
        public int UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
    }
}
