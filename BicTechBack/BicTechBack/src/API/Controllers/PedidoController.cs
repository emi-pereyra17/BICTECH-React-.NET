using BicTechBack.src.Core.DTOs;
using BicTechBack.src.Core.Interfaces;
using BicTechBack.src.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; 
using System.Data;

namespace BicTechBack.src.API.Controllers
{
    [ApiController]
    [Route("pedidos")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;
        private readonly ILogger<PedidoController> _logger; 

        public PedidoController(IPedidoService pedidoService, ILogger<PedidoController> logger)
        {
            _pedidoService = pedidoService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> GetAll()
        {
            _logger.LogInformation("Obteniendo lista de pedidos para el usuario actual.");
            try
            {
                IEnumerable<PedidoDTO> pedidos;

                if (User.IsInRole("Admin"))
                {
                    pedidos = await _pedidoService.GetAllPedidosAsync();
                    _logger.LogInformation("Pedidos obtenidos para Admin. Total: {Total}", pedidos.Count());
                }
                else
                {
                    var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
                    pedidos = await _pedidoService.GetPedidosByUsuarioIdAsync(userId);
                    _logger.LogInformation("Pedidos obtenidos para usuario {UserId}. Total: {Total}", userId, pedidos.Count());
                }

                return Ok(new { message = "Lista de pedidos", pedidos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar los pedidos.");
                return StatusCode(500, new { message = "Error al consultar los pedidos", error = ex.Message });
            }
        }

        [HttpGet("paginado")]
        public async Task<ActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
        [FromQuery] string? filtro = null)
        {
            _logger.LogInformation("Obteniendo pedidos paginados. Página: {Page}, Tamaño: {PageSize}, Filtro: {Filtro}", page, pageSize, filtro);
            try
            {
                var (pedidos, total) = await _pedidoService.GetPedidosAsync(page, pageSize, filtro);
                _logger.LogInformation("Pedidos paginados obtenidos correctamente. Total: {Total}", total);
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
                _logger.LogError(ex, "Error al consultar los pedidos paginados.");
                return StatusCode(500, new { message = "Error al consultar los pedidos", error = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> GetById(int id)
        {
            _logger.LogInformation("Buscando pedido por Id: {Id}", id);
            try
            {
                var pedido = await _pedidoService.GetPedidoByIdAsync(id);

                if (User.IsInRole("Admin") || pedido.UsuarioId == int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value))
                {
                    _logger.LogInformation("Pedido encontrado. Id: {Id}", id);
                    return Ok(new { message = "Pedido encontrado", pedido });
                }
                else
                {
                    _logger.LogWarning("Usuario no autorizado para acceder al pedido. Id: {Id}", id);
                    return Forbid();
                }
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Pedido no encontrado. Id: {Id}", id);
                return NotFound(new { message = "Pedido no encontrado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar el pedido. Id: {Id}", id);
                return StatusCode(500, new { message = "Error al consultar el pedido", error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> Create([FromBody] CrearPedidoDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Datos requeridos faltantes al crear pedido.");
                return BadRequest(new { message = "Faltan datos requeridos" });
            }

            _logger.LogInformation("Intentando crear pedido para usuario {UsuarioId}", dto.UsuarioId);
            try
            {
                var pedidoCreado = await _pedidoService.CreatePedidoAsync(dto);
                _logger.LogInformation("Pedido creado correctamente. Id: {Id}, UsuarioId: {UsuarioId}", pedidoCreado.Id, pedidoCreado.UsuarioId);
                return StatusCode(201, new { message = "Pedido creado correctamente", pedido = pedidoCreado });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el pedido para usuario {UsuarioId}", dto.UsuarioId);
                return StatusCode(500, new { message = "Error al crear el pedido", error = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(int id, [FromBody] CrearPedidoDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Datos requeridos faltantes al actualizar pedido. Id: {Id}", id);
                return BadRequest(new { message = "Faltan datos requeridos" });
            }

            _logger.LogInformation("Intentando actualizar pedido. Id: {Id}", id);
            try
            {
                var pedidoActualizado = await _pedidoService.UpdatePedidoAsync(id, dto);
                _logger.LogInformation("Pedido actualizado correctamente. Id: {Id}", id);
                return Ok(new { message = "Pedido actualizado", pedido = pedidoActualizado });
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Pedido no encontrado al intentar actualizar. Id: {Id}", id);
                return NotFound(new { message = "Pedido no encontrado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el pedido. Id: {Id}", id);
                return StatusCode(500, new { message = "Error al actualizar el pedido", error = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            _logger.LogInformation("Intentando eliminar pedido. Id: {Id}", id);
            try
            {
                var resultado = await _pedidoService.DeletePedidoAsync(id);
                if (resultado)
                {
                    _logger.LogInformation("Pedido eliminado correctamente. Id: {Id}", id);
                    return Ok(new { message = "Pedido eliminado" });
                }
                else
                {
                    _logger.LogWarning("Pedido no encontrado al intentar eliminar. Id: {Id}", id);
                    return NotFound(new { message = "Pedido no encontrado" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el pedido. Id: {Id}", id);
                return StatusCode(500, new { message = "Error al eliminar el pedido", error = ex.Message });
            }
        }
    }
}
