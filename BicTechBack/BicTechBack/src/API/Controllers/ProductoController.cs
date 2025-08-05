using BicTechBack.src.Core.DTOs;
using BicTechBack.src.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BicTechBack.src.API.Controllers
{
    [ApiController]
    [Route("productos")]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var productos = await _productoService.GetAllProductosAsync();
                return Ok(new { message = "Lista de productos", productos });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al consultar los productos", error = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var producto = await _productoService.GetProductoByIdAsync(id);
                return Ok(new { message = "Producto encontrado", producto });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Producto no encontrado" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al consultar el producto", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CrearProductoDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Faltan datos requeridos" });

            try
            {
                var productoCreado = await _productoService.CreateProductoAsync(dto);
                return StatusCode(201, new { message = "Producto creado", producto = productoCreado });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear el producto", error = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] CrearProductoDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Faltan datos requeridos" });

            try
            {
                var productoActualizado = await _productoService.UpdateProductoAsync(id, dto);
                return Ok(new { message = "Producto actualizado", producto = productoActualizado });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Producto no encontrado" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar el producto", error = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var eliminado = await _productoService.DeleteProductoAsync(id);
                if (!eliminado)
                {
                    return NotFound(new { message = "Producto no encontrado" });
                }
                return Ok(new { message = "Producto eliminado" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Producto no encontrado" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar el producto", error = ex.Message });
            }
        }
    }
}