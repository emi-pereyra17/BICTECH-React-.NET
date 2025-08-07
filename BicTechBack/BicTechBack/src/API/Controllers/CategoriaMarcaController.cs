using BicTechBack.src.Core.DTOs;
using BicTechBack.src.Core.Interfaces;
using BicTechBack.src.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BicTechBack.src.API.Controllers
{
    [ApiController]
    [Route("categoriaMarca")]
    public class CategoriaMarcaController : ControllerBase
    {
        private readonly ICategoriaMarcaService _categoriaMarcaService;

        public CategoriaMarcaController(ICategoriaMarcaService categoriaMarcaService)
        {
            _categoriaMarcaService = categoriaMarcaService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var relaciones = await _categoriaMarcaService.GetAllCMAsync();
                return Ok(new { message = "Lista de relaciones", relaciones });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al consultar las relaciones", error = ex.Message });
            }
        }

        [HttpGet("paginado")]
        public async Task<ActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
        [FromQuery] string? filtro = null)
        {
            try
            {
                var (cms, total) = await _categoriaMarcaService.GetCMAsync(page, pageSize, filtro);
                return Ok(new
                {
                    message = "Lista paginada de cms",
                    total,
                    page,
                    pageSize,
                    cms
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al consultar las cms", error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([FromBody] CrearCategoriaMarcaDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Faltan datos requeridos" });

            try
            {
                var relacionCreada = await _categoriaMarcaService.CreateCMAsync(dto);
                return StatusCode(201, new { message = "Relacion creada", relacion = relacionCreada });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear la relacion", error = ex.Message });
            }
        }

        [HttpGet("categoria/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult> ObtenerMarcasPorCategoria(int id)
        {
            try
            {
                var marcas = await _categoriaMarcaService.GetMarcasPorCategoriaAsync(id);
                if (marcas == null || !marcas.Any())
                    return NotFound(new { message = "Categoria no encontrada" });

                return Ok(new { message = $"Marcas de la categoria {id}", marcas });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Categoria no encontrada" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener las marcas por categoria", error = ex.Message });
            }
        }

        [HttpGet("marca/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult> ObtenerCategoriasPorMarca(int id)
        {
            try
            {
                var categorias = await _categoriaMarcaService.GetCategoriasPorMarcaAsync(id);
                if (categorias == null || !categorias.Any())
                    return NotFound(new { message = "Marca no encontrada" });

                return Ok(new { message = $"Categorias de la marca {id}", categorias });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Marca no encontrada" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al consultar las categorias por marca", error = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var eliminado = await _categoriaMarcaService.DeleteCMAsync(id);
                if (!eliminado)
                {
                    return NotFound(new { message = "Relacion no encontrada" });
                }
                return Ok(new { message = "Relacion eliminada" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Relacion no encontrada" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar la relacion", error = ex.Message });
            }
        }
    }
}
