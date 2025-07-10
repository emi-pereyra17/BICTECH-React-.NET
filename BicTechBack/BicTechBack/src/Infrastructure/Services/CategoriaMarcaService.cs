using AutoMapper;
using BicTechBack.src.Core.DTOs;
using BicTechBack.src.Core.Entities;
using BicTechBack.src.Core.Interfaces;

namespace BicTechBack.src.Infrastructure.Services
{
    public class CategoriaMarcaService : ICategoriaMarcaService
    {
        private readonly ICategoriaMarcaRepository _repository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMarcaRepository _marcaRepository;
        private readonly IMapper _mapper;

        public CategoriaMarcaService(ICategoriaMarcaRepository categoriaMarcaRepository,ICategoriaRepository categoriaRepository, IMarcaRepository marcaRepository, IMapper mapper)
        {
            _repository = categoriaMarcaRepository;
            _categoriaRepository = categoriaRepository;
            _marcaRepository = marcaRepository;
            _mapper = mapper;
        }
        public async Task<CategoriaMarcaDTO> CreateCMAsync(CrearCategoriaMarcaDTO dto)
        {
            var cms = await _repository.GetAllAsync();
            var categoria = await _categoriaRepository.GetByIdAsync(dto.CategoriaId);
            if (categoria == null)
            {
                throw new KeyNotFoundException("Categoría no encontrada.");
            }
            var marca = await _marcaRepository.GetByIdAsync(dto.MarcaId);
            if (marca == null)
            {
                throw new KeyNotFoundException("Marca no encontrada.");
            }
            if (cms.Any(cm => cm.CategoriaId == dto.CategoriaId && cm.MarcaId == dto.MarcaId))
            {
                throw new InvalidOperationException("Ya existe una relación entre esta categoría y marca.");
            }
            
            var cm = _mapper.Map<CategoriaMarca>(dto);
            var cmCreada = await _repository.AddAsync(cm);
            return _mapper.Map<CategoriaMarcaDTO>(cmCreada);
        }

        public async Task<bool> DeleteCMAsync(int id)
        {
            var cm = await _repository.DeleteAsync(id);
            if (!cm)
                throw new KeyNotFoundException("Relación categoría-marca no encontrada.");

            return true;
        }

        public async Task<IEnumerable<CategoriaMarcaDTO>> GetAllCMAsync()
        {
            var cms = await _repository.GetAllAsync();
            if (cms == null || !cms.Any())
            {
                return Enumerable.Empty<CategoriaMarcaDTO>();
            }
            return _mapper.Map<IEnumerable<CategoriaMarcaDTO>>(cms);
        }

        public async Task<IEnumerable<CategoriaMarcaDTO>> GetCMByCategoriaIdAsync(int categoriaId)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(categoriaId);
            if (categoria == null)
            {
                throw new KeyNotFoundException("Categoría no encontrada.");
            }

            var cms = await _repository.GetByCategoriaIdAsync(categoriaId);
            if (cms == null || !cms.Any())
            {
                return Enumerable.Empty<CategoriaMarcaDTO>();
            }

            return _mapper.Map<IEnumerable<CategoriaMarcaDTO>>(cms);
        }

        public async Task<IEnumerable<CategoriaMarcaDTO>> GetCMByMarcaIdAsync(int marcaId)
        {
            var marca = await _marcaRepository.GetByIdAsync(marcaId);
            if (marca == null)
            {
                throw new KeyNotFoundException("Marca no encontrada.");
            }

            var cms = await _repository.GetByMarcaIdAsync(marcaId);
            if (cms == null || !cms.Any())
            {
                return Enumerable.Empty<CategoriaMarcaDTO>();
            }

            return _mapper.Map<IEnumerable<CategoriaMarcaDTO>>(cms);
        }
    }
}
