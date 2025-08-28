using AutoMapper;
using BicTechBack.src.Core.DTOs;
using BicTechBack.src.Core.Entities;
using BicTechBack.src.Core.Interfaces;
using BicTechBack.src.Core.Mappings;
using BicTechBack.src.Core.Services;
using BicTechBack.src.Infrastructure.Data;
using BicTechBack.src.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;

namespace IntegrationTests.Integration.Services
{
    public class UsuarioServiceIntegration
    {
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UsuarioProfile>();
            });
            return config.CreateMapper();
        }

        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task CreateUsuarioAsync_UsuarioValido_PersisteEnDb()
        {
            // Arrange
            using var context = GetDbContext();
            var repo = new UsuarioRepository(context);
            var mapper = GetMapper();
            var logger = new LoggerFactory().CreateLogger<UsuarioService>();
            var service = new UsuarioService(repo, mapper, logger);

            var dto = new CrearUsuarioDTO
            {
                Nombre = "Integracion",
                Email = "integ@mail.com",
                Password = "1234"
            };

            // Act
            var result = await service.CreateUsuarioAsync(dto, "User");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Integracion", result.Nombre);
            Assert.Equal("integ@mail.com", result.Email);
            Assert.Equal("User", result.Rol);

            // Verifica que realmente se guardó en la base de datos
            var usuarioEnDb = await context.Usuarios.FirstOrDefaultAsync(u => u.Email == "integ@mail.com");
            Assert.NotNull(usuarioEnDb);
            Assert.Equal("Integracion", usuarioEnDb.Nombre);
        }

        [Fact]
        public async Task CreateUsuarioAsync_EmailDuplicado_LanzaInvalidOperationException()
        {
            using var context = GetDbContext();
            var repo = new UsuarioRepository(context);
            var mapper = GetMapper();
            var logger = new LoggerFactory().CreateLogger<UsuarioService>();
            var service = new UsuarioService(repo, mapper, logger);

            // Agrega un usuario inicial
            context.Usuarios.Add(new Usuario
            {
                Nombre = "Existente",
                Email = "existente@mail.com",
                Password = "1234",
                Rol = RolUsuario.User
            });
            await context.SaveChangesAsync();

            var dto = new CrearUsuarioDTO
            {
                Nombre = "Nuevo",
                Email = "existente@mail.com",
                Password = "abcd"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateUsuarioAsync(dto, "User"));
        }
    }
}