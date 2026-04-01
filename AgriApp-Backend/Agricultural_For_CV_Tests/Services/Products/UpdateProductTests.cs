using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Agricultural_For_CV_BLL.Services;
using Agricultural_For_CV_DAL.Entities;
using Agricultural_For_CV_DAL.Interfaces;
using Agricultural_For_CV_Shared.Dtos.Products;
using Agricultural_For_CV_Shared.Results;
using Microsoft.Extensions.Logging;
using Agricultural_For_CV_Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Agricultural_For_CV_Shared.Settings;
using Microsoft.Extensions.Options;
using Agricultural_For_CV_BLL.Interfaces;

namespace Agricultural_For_CV.Tests.Services.Products
{
    public class UpdateProductTests
    {
        private readonly Mock<IImageService> _mockImageService;
        private readonly Mock<INotificationService> _mockNotificationService;
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly Mock<ILogger<ProductService>> _mockLogger;
        private readonly Mock<IOptions<AppSettings>> _mockSetting;
        private readonly ProductService _service;
        private readonly Mock<IAuditLogService> _auditLogService;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        public UpdateProductTests()
        {
            _mockImageService = new Mock<IImageService>();
            _mockLogger = new Mock<ILogger<ProductService>>();
            _mockSetting = new Mock<IOptions<AppSettings>>();
            _mockRepo = new Mock<IProductRepository>();
            _mockNotificationService = new Mock<INotificationService>();
            _auditLogService = new Mock<IAuditLogService>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();

            _service = new ProductService(
                _mockRepo.Object,                  // IProductRepository
                _mockImageService.Object,          // IImageService
                _mockLogger.Object,                // ILogger<ProductService>
                _mockSetting.Object,               // ISettingService
                _mockNotificationService.Object,   // INotificationService
                _auditLogService.Object,           // IAuditLogService
                _httpContextAccessor.Object        // IHttpContextAccessor
            );
        }


        // 1️ DTO null
        [Fact]
        public async Task UpdateProduct_ShouldReturnFailure_WhenDtoIsNull()
        {
            var result = await _service.UpdateProductAsync(null);
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid product data.", result.Error);
        }

        // 2️ Product Not Found!
        [Fact]
        public async Task UpdateProduct_ShouldReturnFailure_WhenProductNotFound()
        {
            var dto = new ProductToUpdate { Id = 1 };
            _mockRepo.Setup(r => r.GetAsync(dto.Id, true)).ReturnsAsync((Product)null);

            var result = await _service.UpdateProductAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal("Product not found.", result.Error);
        }


        // 3️ Happy Path Without Images
        [Fact]
        public async Task UpdateProduct_ShouldReturnSuccess_WhenValidWithoutImages()
        {
            var dto = new ProductToUpdate
            {
                Id = 1,
                Name = "Tomato",
                Description = "Fresh",
                QuantityInStock = 10,
                CropTypeId = 1,
                QuantityTypeId = 1,
                Price = 5
            };

            var existing = new Product
            {
                Id = 1,
                Name = "OldName",
                Description = "OldDesc",
                QuantityInStock = 5,
                CropTypeId = 1,
                QuantityTypeId = 1,
                Price = 3,
                ProductsImages = new List<ProductsImages>()
            };

            _mockRepo.Setup(r => r.GetAsync(dto.Id, true)).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            var result = await _service.UpdateProductAsync(dto);

            Assert.True(result.IsSuccess);
            Assert.Equal("Tomato", result.Data?.Name);
            Assert.Equal("Fresh", result.Data?.Description);
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Once);
        }




        // 5️ Exception Repository
        [Fact]
        public async Task UpdateProduct_ShouldReturnFailure_WhenRepositoryThrowsException()
        {
            var dto = new ProductToUpdate { Id = 1, Name = "Tomato" };
            _mockRepo.Setup(r => r.GetAsync(dto.Id, true)).ThrowsAsync(new Exception("DB Error"));

            var result = await _service.UpdateProductAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to update product.", result.Error);
        }

        // 6️ Trim Name/Description
        [Fact]
        public async Task UpdateProduct_ShouldTrimNameAndDescription()
        {
            var dto = new ProductToUpdate
            {
                Id = 1,
                Name = "  Tomato  ",
                Description = "  Fresh  "
            };

            var existing = new Product
            {
                Id = 1,
                Name = "OldName",
                Description = "OldDesc",
                ProductsImages = new List<ProductsImages>()
            };

            _mockRepo.Setup(r => r.GetAsync(dto.Id, true)).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            var result = await _service.UpdateProductAsync(dto);

            Assert.True(result.IsSuccess);
            Assert.Equal("Tomato", result.Data.Name);
            Assert.Equal("Fresh", result.Data.Description);
        }
    }
}
