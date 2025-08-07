using BicTechBack.src.Core.DTOs;
using BicTechBack.src.Core.Interfaces;
using BicTechBack.src.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                IEnumerable<PedidoDTO> pedidos;

                if (User.IsInRole("Admin"))
                {
                    pedidos = await _pedidoService.GetAllPedidosAsync();
                }
                else
                {
                    var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
                    pedidos = await _pedidoService.GetPedidosByUsuarioIdAsync(userId);
                }

                return Ok(new { message = "Lista de pedidos", pedidos });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al consultar los pedidos", error = ex.Message });
            }
        }

        [HttpGet("paginado")]
        public async Task<ActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
        [FromQuery] string? filtro = null)
        {
            try
            {
                var (pedidos, total) = await _pedidoService.GetPedidosAsync(page, pageSize, filtro);
                return Ok(new
                {
                    message = "Lista paginada de pedidos",
                    total,
                    page,
                    pageSize,
                    pedidos
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al consultar los pedidos", error = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var pedido = await _pedidoService.GetPedidoByIdAsync(id);

                if (User.IsInRole("Admin") || pedido.UsuarioId == int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value))
                {
                    return Ok(new { message = "Pedido encontrado", pedido });
                }
                else
                {
                    return Forbid();
                }
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Pedido no encontrado" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Error al consultar el pedido" });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
