using AutoMapper;
using BicTechBack.src.Core.DTOs;
using BicTechBack.src.Core.Entities;
using BicTechBack.src.Core.Interfaces;

namespace BicTechBack.src.Infrastructure.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _repository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly IMapper _mapper;

        public PedidoService(IPedidoRepository repository, IUsuarioRepository usuarioRepository, IProductoRepository productoRepository , IMapper mapper)
        {
            _repository = repository;
            _usuarioRepository = usuarioRepository;
            _productoRepository = productoRepository;
            _mapper = mapper;
        }

        public async Task<PedidoDTO> CreatePedidoAsync(CrearPedidoDTO dto)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(dto.UsuarioId);
            if (usuario == null)
                throw new KeyNotFoundException("Usuario no encontrado.");

            if (dto.Productos == null || !dto.Productos.Any())
                throw new ArgumentException("Debe agregar al menos un producto al pedido.");

            foreach (var prod in dto.Productos)
            {
                var producto = await _productoRepository.GetByIdAsync(prod.ProductoId);
                if (producto == null)
                    throw new KeyNotFoundException($"Producto con ID {prod.ProductoId} no encontrado.");

                if (producto.Stock < prod.Cantidad)
                    throw new InvalidOperationException($"Stock insuficiente para el producto con ID {prod.ProductoId}.");

                producto.Stock -= prod.Cantidad;
                await _productoRepository.UpdateAsync(producto);
            }

            decimal total = dto.Productos.Sum(p => p.Precio * p.Cantidad);

            var pedido = new Pedido
            {
                UsuarioId = dto.UsuarioId,
                DireccionEnvio = dto.DireccionEnvio,
                FechaPedido = DateTime.UtcNow,
                Estado = EstadoPedido.Pendiente,
                Total = total,
                PedidosDetalles = dto.Productos.Select(p => new PedidoDetalle
                {
                    ProductoId = p.ProductoId,
                    Cantidad = p.Cantidad,
                    Precio = p.Precio,
                    Subtotal = p.Precio * p.Cantidad
                }).ToList()
            };

            var pedidoCreado = await _repository.AddAsync(pedido);

            return _mapper.Map<PedidoDTO>(pedidoCreado);
        }

        public async Task<PedidoDTO> AgregarProductoAlPedidoAsync(AgregarProductoPedidoDTO dto)
        {
            var pedido = await _repository.GetByIdAsync(dto.PedidoId);
            if (pedido == null)
                throw new KeyNotFoundException("Pedido no encontrado.");

            // Buscar si ya existe un detalle para ese producto
            var detalleExistente = pedido.PedidosDetalles
                .FirstOrDefault(d => d.ProductoId == dto.ProductoId);

            if (detalleExistente != null)
            {
                // Si existe, sumar la cantidad y actualizar el subtotal
                detalleExistente.Cantidad += dto.Cantidad;
                detalleExistente.Subtotal = detalleExistente.Cantidad * detalleExistente.Precio;
            }
            else
            {
                // Si no existe, crear un nuevo detalle
                var nuevoDetalle = new PedidoDetalle
                {
                    ProductoId = dto.ProductoId,
                    Cantidad = dto.Cantidad,
                    Precio = dto.Precio,
                    Subtotal = dto.Precio * dto.Cantidad
                };
                pedido.PedidosDetalles.Add(nuevoDetalle);
            }

            // Actualizar el total del pedido
            pedido.Total = pedido.PedidosDetalles.Sum(d => d.Subtotal);

            await _repository.UpdateAsync(pedido);

            return _mapper.Map<PedidoDTO>(pedido);
        }

        public async Task<bool> DeletePedidoAsync(int id)
        {
            var pedido = await _repository.DeleteAsync(id);
            if (!pedido)
                throw new KeyNotFoundException("Pedido no encontrado.");
            return true;
        }

        public async Task<IEnumerable<PedidoDTO>> GetAllPedidosAsync()
        {
            var pedidos = await _repository.GetAllAsync();
            if (pedidos == null || !pedidos.Any())
            {
                return Enumerable.Empty<PedidoDTO>();
            }
            return _mapper.Map<IEnumerable<PedidoDTO>>(pedidos);
        }


        public async Task<PedidoDTO> GetPedidoByIdAsync(int id)
        {
            var pedido = await _repository.GetByIdAsync(id);
            if (pedido == null)
                throw new KeyNotFoundException("Pedido no encontrado.");

            return _mapper.Map<PedidoDTO>(pedido);
        }


        public async Task<PedidoDTO> UpdatePedidoAsync(int id, CrearPedidoDTO dto)
        {
            var pedidoExistente = await _repository.GetByIdAsync(id);
            if (pedidoExistente == null)
                throw new KeyNotFoundException("Pedido no encontrado.");

            var usuario = await _usuarioRepository.GetByIdAsync(dto.UsuarioId);
            if (usuario == null)
                throw new KeyNotFoundException("Usuario no encontrado.");

            if (dto.Productos == null || !dto.Productos.Any())
            {
                throw new ArgumentException("Debe agregar al menos un producto al pedido.");
            }

            pedidoExistente.UsuarioId = dto.UsuarioId;
            pedidoExistente.DireccionEnvio = dto.DireccionEnvio;

            pedidoExistente.PedidosDetalles.Clear();

            foreach (var prod in dto.Productos)
            {
                pedidoExistente.PedidosDetalles.Add(new PedidoDetalle
                {
                    ProductoId = prod.ProductoId,
                    Cantidad = prod.Cantidad,
                    Precio = prod.Precio,
                    Subtotal = prod.Precio * prod.Cantidad
                });
            }

            pedidoExistente.Total = pedidoExistente.PedidosDetalles.Sum(d => d.Subtotal);

            var pedidoActualizado = await _repository.UpdateAsync(pedidoExistente);
            return _mapper.Map<PedidoDTO>(pedidoActualizado);
        }
    }
}
