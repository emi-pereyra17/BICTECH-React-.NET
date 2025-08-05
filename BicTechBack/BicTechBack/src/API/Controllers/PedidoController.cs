using BicTechBack.src.Core.DTOs;
using BicTechBack.src.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BicTechBack.src.API.Controllers
{
    [ApiController]
    [Route("pedidos")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var pedidos = await _pedidoService.GetAllPedidosAsync();
                return Ok(new { message = "Lista de pedidos", pedidos });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al consultar los pedidos", error = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var pedido = await _pedidoService.GetPedidoByIdAsync(id);
                return Ok(new { message = "Pedido encontrado", pedido });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Pedido no encontrado" });
            }catch (Exception)
            {
                return StatusCode(500, new { message = "Error al consultar el pedido" });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CrearPedidoDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Faltan datos requeridos" });

            try
            {
                var pedidoCreado = await _pedidoService.CreatePedidoAsync(dto);
                return StatusCode(201, new { message = "Pedido creado correctamente", pedido = pedidoCreado });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear el pedido", error = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] CrearPedidoDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Faltan datos requeridos" });
            try
            {
                var pedidoActualizado = await _pedidoService.UpdatePedidoAsync(id, dto);
                return Ok(new { message = "Pedido actualizado", pedido = pedidoActualizado });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Pedido no encontrado" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar el pedido", error = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var resultado = await _pedidoService.DeletePedidoAsync(id);
                if (resultado)
                    return Ok(new { message = "Pedido eliminado" });
                else
                    return NotFound(new { message = "Pedido no encontrado" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar el pedido", error = ex.Message });
            }
        }
    }
}
