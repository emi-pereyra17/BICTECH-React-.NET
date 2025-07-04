using BicTechBack.src.Core.Entities;
using BicTechBack.src.Core.Interfaces;
using BicTechBack.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BicTechBack.src.Infrastructure.Repositories
{
    public class MarcaRepository : IMarcaRepository
    {
        private readonly AppDbContext _context;

        public MarcaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Marca> AddAsync(Marca marca)
        {
             _context.Marcas.Add(marca);
             await _context.SaveChangesAsync();
             return marca;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var marca = await _context.Marcas.FindAsync(id);
            if (marca == null)
                return false;

            _context.Marcas.Remove(marca);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Marca>> GetAllAsync()
        {
            return await _context.Marcas
                .ToListAsync();
        }

        public async Task<Marca?> GetByIdAsync(int id)
        {
            return await _context.Marcas
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Marca> UpdateAsync(Marca marca)
        {
            _context.Marcas.Update(marca);
            await _context.SaveChangesAsync();
            return marca;
        }
    }
}
