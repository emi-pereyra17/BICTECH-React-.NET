using AutoMapper;
using BicTechBack.src.Core.DTOs;
using BicTechBack.src.Core.Interfaces;

namespace BicTechBack.src.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repository;
        private readonly IMapper _mapper;
        public Task<LoginResultDTO> LoginUserAsync(LoginUsuarioDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<int> RegisterUserAsync(RegisterUsuarioDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserPasswordAsync(int id, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}
