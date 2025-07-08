using AutoMapper;
using BicTechBack.src.Core.DTOs;
using BicTechBack.src.Core.Interfaces;

namespace BicTechBack.src.Infrastructure.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repository;
        private readonly IMapper _mapper;

        public ProductoService(IProductoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public Task<ProductoDTO> CreateProductoAsync(CrearProductoDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProductoAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductoDTO>> GetAllProductosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProductoDTO> GetProductoByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductoDTO> UpdateProductoAsync(int id, CrearProductoDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
