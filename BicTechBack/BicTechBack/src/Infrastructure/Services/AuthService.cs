using AutoMapper;
using BicTechBack.src.Core.DTOs;
using BicTechBack.src.Core.Entities;
using BicTechBack.src.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BicTechBack.src.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<Usuario> _passwordHasher = new();
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository repository, IMapper mapper, IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _repository = repository;
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<LoginResultDTO> LoginUserAsync(LoginUsuarioDTO dto)
        {
            var usuario = await _repository.GetByEmailAsync(dto.Email);
            if (usuario == null)
            {
                throw new InvalidOperationException("Credenciales inválidas.");
            }

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, dto.Password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                throw new InvalidOperationException("Credenciales inválidas.");
            }

            var token = GenerateJwtToken(usuario);

            return new LoginResultDTO
            {
                Token = token,
                UsuarioId = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email
            };
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim("nombre", usuario.Nombre),
                new Claim("rol", usuario.Rol.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<int> RegisterUserAsync(RegisterUsuarioDTO dto)
        {
            var usuarioExistente = await _repository.GetByEmailAsync(dto.Email);
            if (usuarioExistente != null)
            {
                throw new InvalidOperationException("El usuario ya existe con ese email.");
            }

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Rol = RolUsuario.User
            };

            usuario.Password = _passwordHasher.HashPassword(usuario, dto.Password);

            var usuarioCreado = await _usuarioRepository.CreateAsync(usuario);
            return usuarioCreado;
        }

        public async Task<bool> UpdateUserPasswordAsync(int id, string newPassword)
        {
            var usuario = await _repository.GetByIdAsync(id);
            if (usuario == null)
                throw new InvalidOperationException("Usuario no encontrado.");

            var hashedPassword = _passwordHasher.HashPassword(usuario, newPassword);
            return await _repository.UpdatePasswordAsync(id, hashedPassword);
        }
    }
}
