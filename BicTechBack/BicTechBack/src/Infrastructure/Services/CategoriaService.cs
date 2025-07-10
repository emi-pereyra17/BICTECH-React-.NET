using BicTechBack.src.Core.DTOs;
using BicTechBack.src.Core.Interfaces;
using BicTechBack.src.Core.Entities;
using AutoMapper;

namespace BicTechBack.src.Infrastructure.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repository;
        private readonly IMapper _mapper;

        public CategoriaService(ICategoriaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CategoriaDTO> CreateCategoriaAsync(CrearCategoriaDTO dto)
        {
            var categorias = await _repository.GetAllAsync();
            if (categorias.Any(c => c.Nombre.Equals(dto.Nombre, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Ya existe una categoría con ese nombre.");
            }

            var categoria = _mapper.Map<Categoria>(dto);
            var categoriaCreada = await _repository.AddAsync(categoria);
            return _mapper.Map<CategoriaDTO>(categoriaCreada);
        }

        public async Task<bool> DeleteCategoriaAsync(int id)
        {
            var eliminado = await _repository.DeleteAsync(id);
            if (!eliminado)
                throw new KeyNotFoundException("Categoría no encontrada.");
            return true;
        }

        public async Task<IEnumerable<CategoriaDTO>> GetAllCategoriasAsync()
        {
            var categorias = await _repository.GetAllAsync();
            if (categorias == null || !categorias.Any())
            {
                return Enumerable.Empty<CategoriaDTO>();
            }
            return _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);
        }

        public async Task<CategoriaDTO> GetCategoriaByIdAsync(int id)
        {
            var categoria = await _repository.GetByIdAsync(id);
            if (categoria == null)
            {
                throw new KeyNotFoundException("Categoría no encontrada.");
            }
            return _mapper.Map<CategoriaDTO>(categoria);
        }

        public async Task<CategoriaDTO> UpdateCategoriaAsync(int id, CrearCategoriaDTO dto)
        {
            var categoriaExistente = await _repository.GetByIdAsync(id);
            if (categoriaExistente == null)
            {
                throw new KeyNotFoundException("Categoría no encontrada.");
            }
            var categorias = await _repository.GetAllAsync();
            if (categorias.Any(c => c.Nombre.Equals(dto.Nombre, StringComparison.OrdinalIgnoreCase) && c.Id != id))
            {
                throw new InvalidOperationException("Ya existe una categoria con ese nombre.");
            }

            _mapper.Map(dto, categoriaExistente);

            var categoriaActualizada = await _repository.UpdateAsync(categoriaExistente);
            return _mapper.Map<CategoriaDTO>(categoriaActualizada);

        }
    }
}
