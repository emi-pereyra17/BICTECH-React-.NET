using BicTechBack.src.Core.DTOs;
using BicTechBack.src.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BicTechBack.src.API.Controllers
{
    [ApiController]
    [Route("usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var usuarios = await _usuarioService.GetAllUsuariosAsync();
                return Ok(new { message = "Lista de usuarios", usuarios });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al consultar los usuarios", error = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
                return Ok(new { message = "Usuario encontrado", usuario });
            }
            catch(KeyNotFoundException)
            {
                return NotFound(new { message = "Usuario no encontrado"});
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = "Error al consultar el usuario", error = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] CrearUsuarioDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Faltan datos requeridos" });

            if (!System.Text.RegularExpressions.Regex.IsMatch(dto.Email, @"^\S+@\S+\.\S+$"))
                return BadRequest(new { message = "El email no tiene un formato válido" });

            try
            {
                var usuarioActualizado = await _usuarioService.UpdateUsuarioAsync(dto, id);
                return Ok(new { message = "Usuario actualizado", usuario = usuarioActualizado });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar el usuario", error = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var eliminado = await _usuarioService.DeleteUsuarioAsync(id);
                if (!eliminado)
                {
                    return NotFound(new { message = "Usuario no encontrado" });
                }
                return Ok(new { message = "Usuario eliminado"});
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar el usuario", error = ex.Message } );
            }
        }
    }
}