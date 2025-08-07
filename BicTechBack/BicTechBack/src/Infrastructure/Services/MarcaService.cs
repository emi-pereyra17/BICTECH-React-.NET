using AutoMapper;
using BicTechBack.src.Core.DTOs;
using BicTechBack.src.Core.Entities;
using BicTechBack.src.Core.Interfaces;

namespace BicTechBack.src.Infrastructure.Services
{
    public class MarcaService : IMarcaService
    {
        private readonly IMarcaRepository _repository;
        private readonly IMapper _mapper;

        public MarcaService(IMarcaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<MarcaDTO> CreateMarcaAsync(CrearMarcaDTO dto)
        {
            var marcas = await _repository.GetAllAsync();
            if (marcas.Any(m => m.Nombre.Equals(dto.Nombre, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Ya existe una marca con ese nombre.");
            }
            var marca = _mapper.Map<Marca>(dto);
            var marcaCreada = await _repository.AddAsync(marca);
            return _mapper.Map<MarcaDTO>(marcaCreada);
        }

        public async Task<bool> DeleteMarcaAsync(int id)
        {
            var eliminado = await _repository.DeleteAsync(id);
            if (!eliminado)
                throw new KeyNotFoundException("Marca no encontrada.");

            return true;
        }

        public async Task<IEnumerable<MarcaDTO>> GetAllMarcasAsync()
        {
            var marcas = await _repository.GetAllAsync();
            if (marcas == null || !marcas.Any())
            {
                return Enumerable.Empty<MarcaDTO>();
            }
            return _mapper.Map<IEnumerable<MarcaDTO>>(marcas);
        }

        public async Task<MarcaDTO> GetMarcaByIdAsync(int id)
        {
            var marca = await _repository.GetByIdAsync(id);
            if (marca == null)
            {
                throw new KeyNotFoundException("Marca no encontrada.");
            }
            return _mapper.Map<MarcaDTO>(marca);
        }

        public async Task<(IEnumerable<MarcaDTO> Marcas, int Total)> GetMarcasAsync(int page, int pageSize, string? filtro)
        {
            var marcas = await _repository.GetAllAsync();

            var total = marcas.Count();

            var marcasPaginados = marcas
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var marcasDto = _mapper.Map<IEnumerable<MarcaDTO>>(marcasPaginados);

            return (marcasDto, total);
        }

        public async Task<MarcaDTO> UpdateMarcaAsync(int id, CrearMarcaDTO dto)
        {
            var marcaExistente = await _repository.GetByIdAsync(id);
            var marcas = await _repository.GetAllAsync();

            if (marcaExistente == null)
            {
                throw new KeyNotFoundException("Marca no encontrada.");
            }
            
            if (marcas.Any(m => m.Nombre.Equals(dto.Nombre, StringComparison.OrdinalIgnoreCase) && m.Id != id))
            {
                throw new InvalidOperationException("Ya existe una marca con ese nombre.");
            }

            _mapper.Map(dto, marcaExistente);

            var marcaActualizada = await _repository.UpdateAsync(marcaExistente);
            return _mapper.Map<MarcaDTO>(marcaActualizada);
        }
    }
}
