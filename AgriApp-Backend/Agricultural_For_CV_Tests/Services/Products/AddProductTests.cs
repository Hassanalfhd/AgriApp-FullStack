using System.Threading.Tasks;
using Agricultural_For_CV_BLL.Interfaces;
using Agricultural_For_CV_BLL.Services;
using Agricultural_For_CV_DAL.Entities;
using Agricultural_For_CV_DAL.Interfaces;
using Agricultural_For_CV_Shared.Dtos.Products;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Agricultural_For_CV_Tests.Services.Products
{
    public class AddProductTests
    {

        private readonly Mock<IImageService> _mockImageService;
        private readonly Mock<INotificationService> _mockNotificationService;
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly Mock<ILogger<ProductService>> _mockLogger;
        private readonly Mock<IOptions<AppSettings>> _mockSetting;
        private readonly ProductService _productService;
        private readonly Mock<IAuditLogService> _auditLogService;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        public AddProductTests()
        {
            _mockImageService = new Mock<IImageService>();
            _mockLogger = new Mock<ILogger<ProductService>>();
            _mockRepo = new Mock<IProductRepository>();
            _mockSetting = new Mock<IOptions<AppSettings>>();
            _mockNotificationService = new Mock<INotificationService>();
            _auditLogService = new Mock<IAuditLogService>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();

            _productService = new ProductService(
                  _mockRepo.Object,              
                _mockImageService.Object,        
                _mockLogger.Object,              
                _mockSetting.Object,             
                _mockNotificationService.Object, 
                _auditLogService.Object,         
                _httpContextAccessor.Object      
            );
        }



        // 1️ Null DTO
        [Fact]
        public async Task AddProduct_ShouldReturnFailure_WhenDtoIsNull()
        {
            var result = await _productService.AddProductAsync(null);

            
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid product data.", result.Error);

        }



        //2  Empty Name, null, space


        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public async Task AddProduct_ShouldReturnFailure_WhenNameIsEmpty(string name)
        {

            //Arrange
            var dto = new ProductToCreate
            {
                Name = name,
                QuantityInStock = 1,
                CropTypeId = 1,
                QuantityTypeId = 1,
                Price = 1,
                CreatedBy = 1
            };

            //Act
            var result = await _productService.AddProductAsync(dto);


            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Product name is required.", result.Error);

        }


        // 3️ Happy Path --> success code
        [Fact]
        public async Task AddProduct_ShouldReturnSuccess_WhenDtoIsValid()
        {
            var dto = new ProductToCreate
            {
                Name = "Tomato",
                Description = "Fresh tomato",
                QuantityInStock = 10,
                CropTypeId = 1,
                QuantityTypeId = 1,
                Price = 5,
                CreatedBy = 1
            };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            var result = await _productService.AddProductAsync(dto);
            

            
            Assert.True(result.IsSuccess);

            Assert.Equal("Product created successfully.", result.Message);
            Assert.Equal("Tomato", result.Data!.Name);

            _mockRepo.Verify(r=> r.AddAsync(It.IsAny<Product>()), Times.Once);

        }



        // 4️ Exception من Repository
        [Fact]
        public async Task AddProduct_ShouldReturnFailure_WhenRepositoryThrowsException()
        {
            var dto = new ProductToCreate
            {
                Name = "Tomato",
                QuantityInStock = 10,
                CropTypeId = 1,
                QuantityTypeId = 1,
                Price = 5,
                CreatedBy = 1
            };

            // DataBase Error 
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Product>())).ThrowsAsync(new Exception("DB Error"));

            var result = await _productService.AddProductAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to add product.", result.Error);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
        }


        // 5️ Trim Name
        [Fact]
        public async Task AddProduct_ShouldTrimName_WhenNameHasSpaces()
        {
            var dto = new ProductToCreate
            {
                Name = "  Tomato  ",
                QuantityInStock = 10,
                CropTypeId = 1,
                QuantityTypeId = 1,
                Price = 5,
                CreatedBy = 1
            };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            var result = await _productService.AddProductAsync(dto);

            Assert.True(result.IsSuccess);
            Assert.Equal("Tomato", result.Data?.Name); // الاسم بعد Trim
        }

        // 6️ Null Description
        [Fact]
        public async Task AddProduct_ShouldHandleNullDescription()
        {
            var dto = new ProductToCreate
            {
                Name = "Tomato",
                Description = null,
                QuantityInStock = 10,
                CropTypeId = 1,
                QuantityTypeId = 1,
                Price = 5,
                CreatedBy = 1
            };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            var result = await _productService.AddProductAsync(dto);

            Assert.True(result.IsSuccess);
            Assert.Null(result.Data.Description);
        }
    }
}
