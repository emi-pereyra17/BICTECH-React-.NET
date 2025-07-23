using AutoMapper;
using BicTechBack.src.Core.DTOs;
using BicTechBack.src.Core.Entities;
using BicTechBack.src.Core.Interfaces;
using BicTechBack.src.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BicTechBack.src.Infrastructure.Services
{
    public class CarritoService : ICarritoService
    {
        private readonly ICarritoRepository _repository;
        private readonly IProductoRepository _productoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public CarritoService(ICarritoRepository repository, IMapper mapper, IUsuarioRepository usuarioRepository, IProductoRepository productoRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _usuarioRepository = usuarioRepository;
            _productoRepository = productoRepository;
        }
        public async Task<CarritoDTO> AddProductoToCarritoAsync(int usuarioId, int productoId, int cantidad)
        {
            var carrito = await _repository.GetByUsuarioIdAsync(usuarioId);
            if (carrito == null)
            {
                throw new InvalidOperationException("El carrito del usuario especificado no existe.");
            }
            var detalle = carrito.CarritosDetalles?.FirstOrDefault(cd => cd.ProductoId == productoId);
            if (detalle != null)
            {
                throw new InvalidOperationException("El producto ya está en el carrito.");
            }
            if (cantidad <= 0)
            {
                throw new InvalidOperationException("La cantidad debe ser mayor que cero.");
            }

            // Validar stock disponible
            var producto = await _productoRepository.GetByIdAsync(productoId);
            if (producto == null)
                throw new InvalidOperationException("El producto no existe.");
            if (producto.Stock < cantidad)
                throw new InvalidOperationException("No hay suficiente stock disponible.");


            var carritoCreado = await _repository.AddProductoAsync(usuarioId, productoId, cantidad);
            if (carritoCreado == null)
            {
                throw new InvalidOperationException("No se pudo agregar el producto al carrito.");
            }
            var carritoDto = _mapper.Map<CarritoDTO>(carritoCreado);
            return carritoDto;
        }

        public async Task<CarritoDTO> ClearCarritoAsync(int usuarioId)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null)
            {
                throw new InvalidOperationException("El usuario especificado no existe.");
            }
            var carrito = await _repository.ClearAsync(usuarioId);
            if (carrito == null)
            {
                throw new InvalidOperationException("No se pudo limpiar el carrito del usuario.");
            }
            var carritoDto = _mapper.Map<CarritoDTO>(carrito);
            return carritoDto;
        }

        public async Task<CarritoDTO> DeleteProductoFromCarritoAsync(int usuarioId, int productoId)
        {
            var carrito = await _repository.GetByUsuarioIdAsync(usuarioId);
            if (carrito == null)
            {
                throw new InvalidOperationException("El carrito del usuario especificado no existe.");
            }
            var detalle = carrito.CarritosDetalles?.FirstOrDefault(cd => cd.ProductoId == productoId);
            if (detalle == null)
            {
                throw new InvalidOperationException("El producto especificado no está en el carrito.");
            }
            var carritoActualizado = await _repository.DeleteAsync(usuarioId, productoId);
            if (carritoActualizado == null)
            {
                throw new InvalidOperationException("No se pudo eliminar el producto del carrito.");
            }
            var carritoDto = _mapper.Map<CarritoDTO>(carritoActualizado);
            return carritoDto;
        }

        public async Task<IEnumerable<CarritoDTO>> GetAllCarritosAsync()
        {
            var carritos = await _repository.GetAllAsync();
            if (carritos == null || !carritos.Any())
            {
                return Enumerable.Empty<CarritoDTO>();
            }
            return _mapper.Map<IEnumerable<CarritoDTO>>(carritos);
        }

        public async Task<CarritoDTO> GetCarritoByUsuarioIdAsync(int usuarioId)
        {
            var carrito = await _repository.GetByUsuarioIdAsync(usuarioId);
            if (carrito == null)
            {
                throw new InvalidOperationException("El carrito del usuario especificado no existe.");
            }
            var carritoDto = _mapper.Map<CarritoDTO>(carrito);
            return carritoDto;
        }

        public async Task<CarritoDTO> UpdateAmountProductoAsync(int usuarioId, int productoId, int cantidad)
        {
            var carrito = await _repository.GetByUsuarioIdAsync(usuarioId);
            if (carrito == null)
            {
                throw new InvalidOperationException("El carrito del usuario especificado no existe.");
            }
            var detalle = carrito.CarritosDetalles?.FirstOrDefault(cd => cd.ProductoId == productoId);
            if (detalle == null)
            {
                throw new InvalidOperationException("El producto especificado no está en el carrito.");
            }
            if (cantidad <= 0)
            {
                throw new InvalidOperationException("La cantidad debe ser mayor que cero.");
            }

            // Validar stock disponible
            var producto = await _productoRepository.GetByIdAsync(productoId);
            if (producto == null)
                throw new InvalidOperationException("El producto no existe.");
            if (producto.Stock < cantidad)
                throw new InvalidOperationException("No hay suficiente stock disponible.");


            var carritoActualizado = await _repository.UpdateAsync(usuarioId, productoId, cantidad);
            if (carritoActualizado == null)
            {
                throw new InvalidOperationException("No se pudo actualizar la cantidad del producto en el carrito.");
            }
            var carritoDto = _mapper.Map<CarritoDTO>(carritoActualizado);
            return carritoDto;
        }
    }
}
