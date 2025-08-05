using BicTechBack.src.Core.DTOs;
using BicTechBack.src.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BicTechBack.src.API.Controllers
{
    [ApiController]
    [Route("categorias")]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var categorias = await _categoriaService.GetAllCategoriasAsync();
                return Ok(new { message = "Lista de categorias", categorias });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al consultar las categorias", error = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var categoria = await _categoriaService.GetCategoriaByIdAsync(id);
                return Ok(new { message = "Categoria encontrada", categoria });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Categoria no encontrada" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al consultar la categoria", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CrearCategoriaDTO dto)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(dto.Nombre))
                return BadRequest(new { message = "Faltan datos requeridos" });

            try
            {
                var categoriaCreada = await _categoriaService.CreateCategoriaAsync(dto);
                return StatusCode(201, new { message = "Categoria creada", categoria = categoriaCreada });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear la categoria", error = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] CrearCategoriaDTO dto)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(dto.Nombre))
                return BadRequest(new { message = "Faltan datos requeridos" });

            try
            {
                var categoriaActualizada = await _categoriaService.UpdateCategoriaAsync(id, dto);
                return Ok(new { message = "Categoria actualizada", categoria = categoriaActualizada });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Categoria no encontrada" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar la categoria", error = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var eliminado = await _categoriaService.DeleteCategoriaAsync(id);
                if (!eliminado)
                {
                    return NotFound(new { message = "Categoria no encontrada" });
                }
                return Ok(new { message = "Categoria eliminada" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Categoria no encontrada" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar la categoria", error = ex.Message });
            }
        }
    }
}