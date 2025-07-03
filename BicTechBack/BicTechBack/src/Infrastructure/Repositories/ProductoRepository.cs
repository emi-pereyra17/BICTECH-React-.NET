using BicTechBack.src.Core.Entities;
using BicTechBack.src.Core.Interfaces;

namespace BicTechBack.src.Infrastructure.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        public Task<Producto> AddAsync(Producto producto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Producto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Producto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Producto> UpdateAsync(Producto producto)
        {
            throw new NotImplementedException();
        }
    }
}
