namespace BicTechBack.src.Core.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public RolUsuario Rol { get; set; } = RolUsuario.User;

    }
    public enum RolUsuario
    {
        User,
        Admin,
        SuperAdmin
    }
}
