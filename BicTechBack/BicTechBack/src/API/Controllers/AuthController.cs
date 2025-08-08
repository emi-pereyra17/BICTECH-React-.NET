using BicTechBack.src.Core.DTOs;
using BicTechBack.src.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BicTechBack.src.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] RegisterUsuarioDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Faltan datos requeridos" });

            try
            {
                var id = await _authService.RegisterUserAsync(dto);
                return StatusCode(201, new { message = "Usuario registrado", id });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al registrar el usuario", error = ex.Message });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginUsuarioDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Faltan datos requeridos" });

            try
            {
                var result = await _authService.LoginUserAsync(dto);
                return Ok(new { token = result.Token, user = new { id = result.UsuarioId, nombre = result.Nombre, email = result.Email } });
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al iniciar sesión", error = ex.Message });
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            await _authService.LogoutAsync(userId);
            return Ok(new { message = "Sesión cerrada correctamente" });
        }

        [HttpPut("password/{id:int}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> UpdatePassword(int id, [FromBody] string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return BadRequest(new { message = "Contraseña inválida" });

            var isAdmin = User.IsInRole("Admin");
            var userIdClaim = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            if (!isAdmin && id != userIdClaim)
            {
                return Forbid();
            }

            try
            {
                var actualizado = await _authService.UpdateUserPasswordAsync(id, password);
                if (!actualizado)
                    return NotFound(new { message = "Usuario no encontrado" });

                return Ok(new { message = "Contraseña actualizada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar la contraseña", error = ex.Message });
            }
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<ActionResult> Refresh([FromBody] RefreshRequestDTO dto)
        {
            try
            {
                var result = await _authService.RefreshTokenAsync(dto.Token, dto.RefreshToken);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}

