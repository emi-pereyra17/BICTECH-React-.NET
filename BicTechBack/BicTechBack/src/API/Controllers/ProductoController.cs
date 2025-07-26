using BicTechBack.src.Core.DTOs;
using BicTechBack.src.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BicTechBack.src.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> GetAll()
        {
            var productos = await _productoService.GetAllProductosAsync();
            return Ok(productos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductoDTO>> GetById(int id)
        {
            try
            {
                var producto = await _productoService.GetProductoByIdAsync(id);
                return Ok(producto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            
        }

        [HttpPost]
        public async Task<ActionResult<ProductoDTO>> Create([FromBody] CrearProductoDTO dto)
        {
            try
            {
                var productoCreado = await _productoService.CreateProductoAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = productoCreado.Id }, productoCreado);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }       
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductoDTO>> Update(int id, [FromBody] CrearProductoDTO dto)
        {
            try
            {
                var productoActualizado = await _productoService.UpdateProductoAsync(id, dto);
                if (productoActualizado == null)
                {
                    return NotFound();
                }
                return Ok(productoActualizado);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
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
                    return NotFound();
                }
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            
        }

    }
}
