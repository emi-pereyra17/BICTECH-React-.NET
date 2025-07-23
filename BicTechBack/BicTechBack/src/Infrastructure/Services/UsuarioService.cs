using AutoMapper;
using BicTechBack.src.Core.DTOs;
using BicTechBack.src.Core.Entities;
using BicTechBack.src.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BicTechBack.src.Infrastructure.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;
        private readonly IMapper _mapper;

        public UsuarioService(IUsuarioRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<UsuarioDTO> CreateUsuarioAsync(CrearUsuarioDTO dto, string rol)
        {
            var usuarios = await _repository.GetAllAsync();
            if (usuarios.Any(u => u.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Ya existe un usuario con ese email.");
            }
            if (string.IsNullOrWhiteSpace(rol))
            {
                throw new ArgumentException("El rol no puede ser nulo o vacío.", nameof(rol));
            }
            var usuario = _mapper.Map<Usuario>(dto);
            if (!Enum.TryParse<RolUsuario>(rol, true, out var rolUsuario))
            {
                throw new ArgumentException("El rol especificado no es válido.", nameof(rol));
            }

            usuario.Rol = rolUsuario;

            var usuarioCreadoId = await _repository.CreateAsync(usuario);
            usuario.Id = usuarioCreadoId;
            return _mapper.Map<UsuarioDTO>(usuario);
        }

        public async Task<bool> DeleteUsuarioAsync(int id)
        {
            var usuario = await _repository.GetByIdAsync(id);
            if (usuario == null)
            {
                throw new KeyNotFoundException("Usuario no encontrado.");
            }
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<UsuarioDTO>> GetAllUsuariosAsync()
        {
            var usuarios = await _repository.GetAllAsync();
            if (usuarios == null || !usuarios.Any())
            {
                return Enumerable.Empty<UsuarioDTO>();
            }
            return _mapper.Map<IEnumerable<UsuarioDTO>>(usuarios);
        }

        public async Task<UsuarioDTO> GetUsuarioByIdAsync(int id)
        {
            var usuario = await _repository.GetByIdAsync(id);
            if (usuario == null)
            {
                throw new KeyNotFoundException("Usuario no encontrado.");
            }
            return _mapper.Map<UsuarioDTO>(usuario);
        }

        public async Task<UsuarioDTO> UpdateUsuarioAsync(CrearUsuarioDTO dto, int id)
        {
            var usuarioExistente = await _repository.GetByIdAsync(id);
            if (usuarioExistente == null)
                throw new KeyNotFoundException("Usuario no encontrado.");

            var usuarios = await _repository.GetAllAsync();
            if (usuarios.Any(u => u.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase) && u.Id != id))
                throw new InvalidOperationException("Ya existe un usuario con ese email.");


            usuarioExistente.Nombre = dto.Nombre;
            usuarioExistente.Email = dto.Email;
            usuarioExistente.Password = dto.Password;

            var usuarioActualizado = await _repository.UpdateAsync(usuarioExistente);
            return _mapper.Map<UsuarioDTO>(usuarioActualizado);
        }
    }
}
