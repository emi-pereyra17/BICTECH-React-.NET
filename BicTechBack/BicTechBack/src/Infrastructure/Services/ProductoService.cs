using AutoMapper;
using BicTechBack.src.Core.DTOs;
using BicTechBack.src.Core.Entities;
using BicTechBack.src.Core.Interfaces;

namespace BicTechBack.src.Infrastructure.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMarcaRepository _marcaRepository;
        private readonly IMapper _mapper;

        public ProductoService(IProductoRepository repository, IMarcaRepository marcaRepository, ICategoriaRepository categoriaRepository, IMapper mapper)
        {
            _repository = repository;
            _categoriaRepository = categoriaRepository;
            _marcaRepository = marcaRepository;
            _mapper = mapper;
        }
        public async Task<ProductoDTO> CreateProductoAsync(CrearProductoDTO dto)
        {
            var productos = await _repository.GetAllAsync();
            if (productos.Any(p => p.Nombre.Equals(dto.Nombre, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Ya existe un producto con ese nombre.");
            }

            if (!await _categoriaRepository.ExistsAsync(dto.CategoriaId))
            {
                throw new InvalidOperationException("La categoría especificada no existe.");
            }

            if (!await _marcaRepository.ExistsAsync(dto.MarcaId))
            {
                throw new InvalidOperationException("La marca especificada no existe.");
            }

            var producto = _mapper.Map<Producto>(dto);
            var productoCreado = await _repository.AddAsync(producto);
            return _mapper.Map<ProductoDTO>(productoCreado);
        }

        public async Task<bool> DeleteProductoAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ProductoDTO>> GetAllProductosAsync()
        {
            var productos = await _repository.GetAllAsync();
            if (productos == null || !productos.Any())
            {
                return Enumerable.Empty<ProductoDTO>();
            }
            return _mapper.Map<IEnumerable<ProductoDTO>>(productos);
        }

        public async Task<ProductoDTO> GetProductoByIdAsync(int id)
        {
            var producto = await _repository.GetByIdAsync(id);
            return producto == null ? throw new KeyNotFoundException("Producto no encontrado.") : _mapper.Map<ProductoDTO>(producto);
        }

        public async Task<ProductoDTO> UpdateProductoAsync(int id, CrearProductoDTO dto)
        {
            var productoExistente = await _repository.GetByIdAsync(id);
            if (productoExistente == null)
                throw new KeyNotFoundException("Producto no encontrado.");

            var productos = await _repository.GetAllAsync();
            if (productos.Any(p => p.Nombre.Equals(dto.Nombre, StringComparison.OrdinalIgnoreCase) && p.Id != id))
            {
                throw new InvalidOperationException("Ya existe un producto con ese nombre.");
            }

            if (!await _categoriaRepository.ExistsAsync(dto.CategoriaId))
                throw new InvalidOperationException("La categoría especificada no existe.");

            if (!await _marcaRepository.ExistsAsync(dto.MarcaId))
                throw new InvalidOperationException("La marca especificada no existe.");

             _mapper.Map(dto, productoExistente);

            var productoActualizado = await _repository.UpdateAsync(productoExistente);
            return _mapper.Map<ProductoDTO>(productoActualizado);
        }
    }
}
