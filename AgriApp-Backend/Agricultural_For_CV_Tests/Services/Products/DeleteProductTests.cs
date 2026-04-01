using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Agricultural_For_CV_BLL.Services;
using Agricultural_For_CV_DAL.Entities;
using Agricultural_For_CV_DAL.Interfaces;
using Agricultural_For_CV_Shared.Results;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Settings;
using Microsoft.Extensions.Options;
using Agricultural_For_CV_BLL.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Agricultural_For_CV.Tests.Services.Products
{
    public class DeleteProductTests
    {
        private readonly Mock<IImageService> _mockImageService;
        private readonly Mock<INotificationService> _mockNotificationService;
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly Mock<ILogger<ProductService>> _mockLogger;
        private readonly Mock<IOptions<AppSettings>> _mockSetting;
        private readonly Mock<IAuditLogService> _auditLogService;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly ProductService _service;
        public DeleteProductTests()
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

        // 1️⃣ Invalid product ID
        [Fact]
        public async Task DeleteProduct_ShouldFail_WhenIdIsInvalid()
        {
            var result = await _service.DeleteProduct(0);
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid product id.", result.Error);
        }

        // 2️⃣ Product not found
        [Fact]
        public async Task DeleteProduct_ShouldFail_WhenProductNotFound()
        {
            _mockRepo.Setup(r => r.GetAsync(It.IsAny<int>(), true)).ReturnsAsync((Product)null);

            var result = await _service.DeleteProduct(1);

            Assert.False(result.IsSuccess);
            Assert.Equal("Product not found.", result.Error);
        }

        // 3️⃣ Happy Path: Delete product without images
        [Fact]
        public async Task DeleteProduct_ShouldSucceed_WhenNoImages()
        {

            var existing = new Product
            {
                Id = 1,
                Name = "OldName",
                Description = "OldDesc",
                QuantityInStock = 5,
                CropTypeId = 1,
                QuantityTypeId = 1,
                IsDeleted = true, 
                UpdatedAt = DateTime.UtcNow,
                Price = 3,
            };

            _mockRepo.Setup(r => r.GetAsync(1, false)).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);

            var result = await _service.DeleteProduct(1);

            Assert.True(result.IsSuccess);
            Assert.Equal("Product deleted.", result.Message);
            _mockRepo.Verify(r => r.UpdateAsync(existing), Times.Once);
        }

        

        // 6️ Repository throws exception
        [Fact]
        public async Task DeleteProduct_ShouldFail_WhenRepositoryThrows()
        {
            
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

            _mockRepo.Setup(r => r.GetAsync(1, false)).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.UpdateAsync(existing)).ThrowsAsync(new Exception("DB Error"));

            var result = await _service.DeleteProduct(1);

            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to delete product.", result.Error);
        }

    }
}
