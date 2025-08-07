using BicTechBack.src.Core.DTOs;
using BicTechBack.src.Core.Interfaces;
using BicTechBack.src.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BicTechBack.src.API.Controllers
{
    [ApiController]
    [Route("marcas")]
    public class MarcaController : ControllerBase
    {
        private readonly IMarcaService _marcaService;

        public MarcaController(IMarcaService marcaService)
        {
            _marcaService = marcaService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var marcas = await _marcaService.GetAllMarcasAsync();
                return Ok(new { message = "Lista de Marcas", marcas });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al consultar las Marcas", error = ex.Message });
            }
        }

        [HttpGet("paginado")]
        public async Task<ActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
        [FromQuery] string? filtro = null)
        {
            try
            {
                var (marcas, total) = await _marcaService.GetMarcasAsync(page, pageSize, filtro);
                return Ok(new
                {
                    message = "Lista paginada de marcas",
                    total,
                    page,
                    pageSize,
                    marcas
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al consultar las marcas", error = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var marca = await _marcaService.GetMarcaByIdAsync(id);
                return Ok(new { message = "Marca encontrada", marca });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Marca no encontrada" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al consultar la Marca", error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([FromBody] CrearMarcaDTO dto)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(dto.Nombre))
                return BadRequest(new { message = "Faltan datos requeridos" });

            try
            {
                var marcaCreada = await _marcaService.CreateMarcaAsync(dto);
                return StatusCode(201, new { message = "Marca creada", marca = marcaCreada });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear la Marca", error = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(int id, [FromBody] CrearMarcaDTO dto)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(dto.Nombre))
                return BadRequest(new { message = "Faltan datos requeridos" });

            try
            {
                var marcaActualizada = await _marcaService.UpdateMarcaAsync(id, dto);
                return Ok(new { message = "Marca actualizada", marca = marcaActualizada });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Marca no encontrada" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar la Marca", error = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var eliminado = await _marcaService.DeleteMarcaAsync(id);
                if (!eliminado)
                {
                    return NotFound(new { message = "Marca no encontrada" });
                }
                return Ok(new { message = "Marca eliminada" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Marca no encontrada" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar la Marca", error = ex.Message });
            }
        }
    }
}
