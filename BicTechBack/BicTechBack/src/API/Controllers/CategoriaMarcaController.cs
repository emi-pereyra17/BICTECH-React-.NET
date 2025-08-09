using BicTechBack.src.Core.DTOs;
using BicTechBack.src.Core.Interfaces;
using BicTechBack.src.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BicTechBack.src.API.Controllers
{
    [ApiController]
    [Route("categoriaMarca")]
    public class CategoriaMarcaController : ControllerBase
    {
        private readonly ICategoriaMarcaService _categoriaMarcaService;
        private readonly ILogger<CategoriaMarcaController> _logger; 

        public CategoriaMarcaController(ICategoriaMarcaService categoriaMarcaService, ILogger<CategoriaMarcaController> logger)
        {
            _categoriaMarcaService = categoriaMarcaService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetAll()
        {
            _logger.LogInformation("Obteniendo todas las relaciones categoría-marca.");
            try
            {
                var relaciones = await _categoriaMarcaService.GetAllCMAsync();
                _logger.LogInformation("Relaciones obtenidas correctamente. Total: {Total}", relaciones.Count());
                return Ok(new { message = "Lista de relaciones", relaciones });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar las relaciones.");
                return StatusCode(500, new { message = "Error al consultar las relaciones", error = ex.Message });
            }
        }

        [HttpGet("paginado")]
        public async Task<ActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
        [FromQuery] string? filtro = null)
        {
            _logger.LogInformation("Obteniendo relaciones paginadas. Página: {Page}, Tamaño: {PageSize}, Filtro: {Filtro}", page, pageSize, filtro);
            try
            {
                var (cms, total) = await _categoriaMarcaService.GetCMAsync(page, pageSize, filtro);
                _logger.LogInformation("Relaciones paginadas obtenidas correctamente. Total: {Total}", total);
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
                _logger.LogError(ex, "Error al consultar las cms paginadas.");
                return StatusCode(500, new { message = "Error al consultar las cms", error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([FromBody] CrearCategoriaMarcaDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Datos requeridos faltantes al crear relación categoría-marca.");
                return BadRequest(new { message = "Faltan datos requeridos" });
            }

            _logger.LogInformation("Intentando crear relación categoría-marca. CategoriaId: {CategoriaId}, MarcaId: {MarcaId}", dto.CategoriaId, dto.MarcaId);
            try
            {
                var relacionCreada = await _categoriaMarcaService.CreateCMAsync(dto);
                _logger.LogInformation("Relación creada correctamente. Id: {Id}", relacionCreada.Id);
                return StatusCode(201, new { message = "Relacion creada", relacion = relacionCreada });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Operación inválida al crear relación: {Error}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Entidad no encontrada al crear relación: {Error}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la relación.");
                return StatusCode(500, new { message = "Error al crear la relacion", error = ex.Message });
            }
        }

        [HttpGet("categoria/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult> ObtenerMarcasPorCategoria(int id)
        {
            _logger.LogInformation("Obteniendo marcas para la categoría. CategoriaId: {CategoriaId}", id);
            try
            {
                var marcas = await _categoriaMarcaService.GetMarcasPorCategoriaAsync(id);
                if (marcas == null || !marcas.Any())
                {
                    _logger.LogWarning("Categoría no encontrada o sin marcas. CategoriaId: {CategoriaId}", id);
                    return NotFound(new { message = "Categoria no encontrada" });
                }

                _logger.LogInformation("Marcas obtenidas para la categoría. CategoriaId: {CategoriaId}", id);
                return Ok(new { message = $"Marcas de la categoria {id}", marcas });
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Categoría no encontrada al buscar marcas. CategoriaId: {CategoriaId}", id);
                return NotFound(new { message = "Categoria no encontrada" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las marcas por categoría. CategoriaId: {CategoriaId}", id);
                return StatusCode(500, new { message = "Error al obtener las marcas por categoria", error = ex.Message });
            }
        }

        [HttpGet("marca/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult> ObtenerCategoriasPorMarca(int id)
        {
            _logger.LogInformation("Obteniendo categorías para la marca. MarcaId: {MarcaId}", id);
            try
            {
                var categorias = await _categoriaMarcaService.GetCategoriasPorMarcaAsync(id);
                if (categorias == null || !categorias.Any())
                {
                    _logger.LogWarning("Marca no encontrada o sin categorías. MarcaId: {MarcaId}", id);
                    return NotFound(new { message = "Marca no encontrada" });
                }

                _logger.LogInformation("Categorías obtenidas para la marca. MarcaId: {MarcaId}", id);
                return Ok(new { message = $"Categorias de la marca {id}", categorias });
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Marca no encontrada al buscar categorías. MarcaId: {MarcaId}", id);
                return NotFound(new { message = "Marca no encontrada" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar las categorías por marca. MarcaId: {MarcaId}", id);
                return StatusCode(500, new { message = "Error al consultar las categorias por marca", error = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            _logger.LogInformation("Intentando eliminar relación categoría-marca. Id: {Id}", id);
            try
            {
                var eliminado = await _categoriaMarcaService.DeleteCMAsync(id);
                if (!eliminado)
                {
                    _logger.LogWarning("Relación no encontrada al intentar eliminar. Id: {Id}", id);
                    return NotFound(new { message = "Relacion no encontrada" });
                }
                _logger.LogInformation("Relación eliminada correctamente. Id: {Id}", id);
                return Ok(new { message = "Relacion eliminada" });
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Relación no encontrada al intentar eliminar. Id: {Id}", id);
                return NotFound(new { message = "Relacion no encontrada" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la relación. Id: {Id}", id);
                return StatusCode(500, new { message = "Error al eliminar la relacion", error = ex.Message });
            }
        }
    }
}
