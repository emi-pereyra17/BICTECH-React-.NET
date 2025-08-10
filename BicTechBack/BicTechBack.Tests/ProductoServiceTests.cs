using AutoMapper;
using BicTechBack.src.Core.DTOs;
using BicTechBack.src.Core.Entities;
using BicTechBack.src.Core.Interfaces;
using BicTechBack.src.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BicTechBack.Tests
{
    public class ProductoServiceTests
    {
        [Fact]
        public async Task CreateProductoAsync_ProductoValido_CreaProductoYRetornaDTO()
        {
            var mockRepo = new Mock<IProductoRepository>();
            var mockCategoriaRepo = new Mock<ICategoriaRepository>();
            var mockMarcaRepo = new Mock<IMarcaRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<ProductoService>>();

            var dto = new CrearProductoDTO
            {
                Nombre = "Producto Test",
                Precio = 100,
                Descripcion = "Descripción Test",
                CategoriaId = 1,
                MarcaId = 1,
                Stock = 10,
                ImagenUrl = "http://example.com/imagen.jpg"
            };

            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Producto>());
            mockCategoriaRepo.Setup(r => r.ExistsAsync(dto.CategoriaId)).ReturnsAsync(true);
            mockMarcaRepo.Setup(r => r.ExistsAsync(dto.MarcaId)).ReturnsAsync(true);

            var producto = new Producto { Id = 1, Nombre = dto.Nombre };
            mockMapper.Setup(m => m.Map<Producto>(dto)).Returns(producto);
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Producto>())).ReturnsAsync(producto);
            mockMapper.Setup(m => m.Map<ProductoDTO>(producto)).Returns(new ProductoDTO { Id = 1, Nombre = dto.Nombre });

            var service = new ProductoService(
                mockRepo.Object,
                mockMarcaRepo.Object,
                mockCategoriaRepo.Object,
                mockMapper.Object,
                mockLogger.Object
            );

            // Act
            var result = await service.CreateProductoAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.Nombre, result.Nombre);
            mockRepo.Verify(r => r.AddAsync(It.IsAny<Producto>()), Times.Once);

        }

        [Fact]
        public async Task CreateProductoAsync_NombreDuplicado_LanzaInvalidOperationException()
        {
            var mockRepo = new Mock<IProductoRepository>();
            var mockCategoriaRepo = new Mock<ICategoriaRepository>();
            var mockMarcaRepo = new Mock<IMarcaRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<ProductoService>>();

            var dto = new CrearProductoDTO
            {
                Nombre = "Producto Test",
                Precio = 100,
                Descripcion = "Descripción Test",
                CategoriaId = 1,
                MarcaId = 1,
                Stock = 10,
                ImagenUrl = "http://example.com/imagen.jpg"
            };

            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Producto>
            {
                new Producto { Id = 1, Nombre = dto.Nombre }
            });

            var service = new ProductoService(
                mockRepo.Object,
                mockMarcaRepo.Object,
                mockCategoriaRepo.Object,
                mockMapper.Object,
                mockLogger.Object
            );

            await Assert.ThrowsAsync<InvalidOperationException>(async() => 
            {
                await service.CreateProductoAsync(dto);
            });

        }

        [Fact]
        public async Task CreateProductoAsync_CategoriaNoExiste_LanzaInvalidOperationException()
        {
            var mockRepo = new Mock<IProductoRepository>();
            var mockCategoriaRepo = new Mock<ICategoriaRepository>();
            var mockMarcaRepo = new Mock<IMarcaRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<ProductoService>>();

            var dto = new CrearProductoDTO
            {
                Nombre = "Producto Test",
                Precio = 100,
                Descripcion = "Descripción Test",
                CategoriaId = 1,
                MarcaId = 1,
                Stock = 10,
                ImagenUrl = "http://example.com/imagen.jpg"
            };

            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Producto>());
            mockCategoriaRepo.Setup(r => r.ExistsAsync(dto.CategoriaId)).ReturnsAsync(false);

            var service = new ProductoService(
                mockRepo.Object,
                mockMarcaRepo.Object,
                mockCategoriaRepo.Object,
                mockMapper.Object,
                mockLogger.Object
            );

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await service.CreateProductoAsync(dto);
            });
        }

        [Fact]
        public async Task CreateProductoAsync_MarcaNoExiste_LanzaInvalidOperationException()
        {
            // Arrange
            var mockRepo = new Mock<IProductoRepository>();
            var mockCategoriaRepo = new Mock<ICategoriaRepository>();
            var mockMarcaRepo = new Mock<IMarcaRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<ProductoService>>();

            var dto = new CrearProductoDTO
            {
                Nombre = "Producto Test",
                Precio = 100,
                Descripcion = "Descripción Test",
                CategoriaId = 1,
                MarcaId = 99,
                Stock = 10,
                ImagenUrl = "http://example.com/imagen.jpg"
            };

            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Producto>());
            mockCategoriaRepo.Setup(r => r.ExistsAsync(dto.CategoriaId)).ReturnsAsync(true);
            mockMarcaRepo.Setup(r => r.ExistsAsync(dto.MarcaId)).ReturnsAsync(false);

            var service = new ProductoService(
                mockRepo.Object,
                mockMarcaRepo.Object,
                mockCategoriaRepo.Object,
                mockMapper.Object,
                mockLogger.Object
            );

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateProductoAsync(dto));
        }
    }
}
