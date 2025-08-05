using BicTechBack.src.Core.DTOs;
using BicTechBack.src.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BicTechBack.src.API.Controllers
{
    [ApiController]
    [Route("carritos")]
    public class CarritoController : ControllerBase
    {
        private readonly ICarritoService _carritoService;

        public CarritoController(ICarritoService carritoService)
        {
            _carritoService = carritoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var carritos = await _carritoService.GetAllCarritosAsync();
            return Ok(carritos);
        }

        [HttpGet("{usuarioId:int}")]
        public async Task<IActionResult> GetByUsuarioId(int usuarioId)
        {
            var carrito = await _carritoService.GetCarritoByUsuarioIdAsync(usuarioId);
            if (carrito == null || carrito.Id == 0)
            {
                return Ok(new { usuarioId, productos = new List<object>() });
            }
            return Ok(carrito);
        }

        [HttpPut("{usuarioId:int}/productos/{productoId:int}")]
        public async Task<IActionResult> UpdateAmountProductoInCarrito(int usuarioId, int productoId, [FromQuery] int cantidad)
        {
            if (cantidad <= 0)
                return BadRequest(new { message = "La cantidad debe ser mayor que cero." });

            try
            {
                var carritoActualizado = await _carritoService.UpdateAmountProductoAsync(usuarioId, productoId, cantidad);
                return Ok(new { message = "Cantidad actualizada", carrito = carritoActualizado });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{usuarioId:int}/productos/{productoId:int}/add")]
        public async Task<IActionResult> AddProductoToCarrito(int usuarioId, int productoId, [FromQuery] int cantidad)
        {
            if (cantidad <= 0)
                return BadRequest(new { message = "La cantidad debe ser mayor que cero." });

            try
            {
                var carritoCreado = await _carritoService.AddProductoToCarritoAsync(usuarioId, productoId, cantidad);
                return Ok(new { message = "Producto agregado al carrito", carrito = carritoCreado });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{usuarioId:int}/productos/{productoId:int}")]
        public async Task<IActionResult> DeleteProductoFromCarrito(int usuarioId, int productoId)
        {
            try
            {
                var eliminado = await _carritoService.DeleteProductoFromCarritoAsync(usuarioId, productoId);
                return Ok(new { message = "Producto quitado del carrito", carrito = eliminado });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{usuarioId:int}")]
        public async Task<IActionResult> ClearCarrito(int usuarioId)
        {
            try
            {
                var carritoLimpio = await _carritoService.ClearCarritoAsync(usuarioId);
                return Ok(new { message = "Carrito vaciado", carrito = carritoLimpio });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
