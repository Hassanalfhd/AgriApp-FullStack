using Moq;
using Microsoft.AspNetCore.Mvc;
using Agricultural_For_CV.Controllers.v1;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Results;
using Agricultural_For_CV_Shared.Dtos.Products;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Agricultural_For_CV.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _mockService;
        private readonly ProductsController _controller;
        private readonly Mock<ILogger<ProductsController>> _mocklogger;

        public ProductsControllerTests()
        {
            _mockService = new Mock<IProductService>();
            _mocklogger = new Mock<ILogger<ProductsController>>();
            _controller = new ProductsController(_mockService.Object, _mocklogger.Object);
        }


        // 1️⃣ GetAllProducts - Success
        [Fact]
        public async Task GetAllProducts_ReturnsOk_WhenProductsExist()
        {
            //Arrange
            var products = new List<ProductsResponseDto>
            {
                new ProductsResponseDto { Id = 1, Name = "Tomato", Price = 100, CreatedAt = DateTime.UtcNow, CreatedBy = 1, CropTypeId = 1, QuantityInStock = 23, Description ="NONO", QuantityName = "kilo", QuantityTypeId = 1, },
                new ProductsResponseDto { Id = 2, Name = "Potato", Price = 100, CreatedAt = DateTime.UtcNow, CreatedBy = 1, CropTypeId = 1, QuantityInStock = 23, Description ="NONO", QuantityName = "kilo", QuantityTypeId = 1, },
                new ProductsResponseDto { Id = 3, Name = "Apple", Price = 100, CreatedAt = DateTime.UtcNow, CreatedBy = 1, CropTypeId = 1, QuantityInStock = 23, Description ="NONO", QuantityName = "kilo", QuantityTypeId = 1, },
            };

            _mockService.Setup(s => s.GetAllProductsAsync())
                .ReturnsAsync(Result<IEnumerable<ProductsResponseDto>>.Success(products));


            //Act
            var result = await _controller.GetAllProducts();

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnedProducts = Assert.IsAssignableFrom<IEnumerable<ProductsResponseDto>>(okResult);

        }

        // 2️ GetAllProducts - NotFound
        [Fact]
        public async Task GetAllProducts_ReturnsNotFound_WhenNoProducts()
        {
            _mockService.Setup(s => s.GetAllProductsAsync())
                .ReturnsAsync(Result<IEnumerable<ProductsResponseDto>>.Failure("No products found."));

            var result = await _controller.GetAllProducts();

            Assert.IsType<NotFoundObjectResult>(result);
        }


        // 3️ GetAllUserProducts - Success
        [Fact]
        public async Task GetAllUserProducts_ReturnsOk_WhenProductsExist()
        {
            // 1. Arrange (إعداد البيانات)
            int userId = 1;
            int page = 1;
            int pageSize = 20;

            var userProducts = new List<ProductsResponseDto>
            {
                new ProductsResponseDto { Id = 1, Name = "Tomato", Image = "img.jpg", CreatedByImage = "user.jpg" }
            };

            var pagedUserProducts = new PagedResult<ProductsResponseDto>
            {
                Items = userProducts,
                TotalCount = 1,
                Page = page,
                PageSize = pageSize
            };

            // إعداد Mock الصلاحيات ليعطي Success
            var mockAuthService = new Mock<IAuthorizationService>();
            mockAuthService.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), userId, "FarmerOwnerOrAdmin"))
                .ReturnsAsync(AuthorizationResult.Success());

            // إعداد Mock الخدمة
            _mockService
                .Setup(s => s.GetAllUserProductsAsync(userId, page, pageSize))
                .ReturnsAsync(Result<PagedResult<ProductsResponseDto>>.Success(pagedUserProducts));

            // 2. Act (التنفيذ)
            // لاحظ تمرير الباراميترات بنفس ترتيب الدالة (AuthService أولاً)
            var result = await _controller.GetAllUserProducts(mockAuthService.Object, page, pageSize, userId);

            // 3. Assert (التحقق)
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedData = Assert.IsType<PagedResult<ProductsResponseDto>>(okResult.Value);

            Assert.Single(returnedData.Items);
            Assert.Equal("Tomato", returnedData.Items[0].Name);
        }

        // GetAllUserProducts - userId < 1
        [Fact]
        public async Task GetAllUserProducts_ReturnsBadRequest_WhenUserIdIsInvalid()
        {
            // Act
            var result = await _controller.GetAllUserProducts(new Mock<IAuthorizationService>().Object, userId: 0);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        // 4️ AddNewProducts - Success
        [Fact]
        public async Task AddNewProducts_ReturnsCreated_WhenProductAdded()
        {
            // 1. Arrange (إعداد البيانات والـ Mocks)
            var dto = new ProductToCreate { Name = "Tomato", QuantityInStock = 10, CropTypeId = 1, QuantityTypeId = 1, Price = 5, CreatedBy = 1 };
            var responseDto = new ProductsResponseDto { Id = 1, Name = "Tomato" };

            // إعداد الـ Mock للصلاحيات ليعطي "نجاح"
            var mockAuthService = new Mock<IAuthorizationService>();
            mockAuthService.Setup(a => a.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                dto.CreatedBy, // نمرر نفس المعرف الموجود في الـ DTO
                "FarmerOwnerOrAdmin"))
                .ReturnsAsync(AuthorizationResult.Success());

            // إعداد الـ Mock للخدمة لترجع نجاح الإضافة
            _mockService.Setup(s => s.AddProductAsync(dto))
                .ReturnsAsync(Result<ProductsResponseDto>.Success(responseDto));

            // 2. Act (التنفيذ)
            // نمرر الـ dto والـ mockAuthService.Object
            var result = await _controller.AddNewProducts(dto, mockAuthService.Object);

            // 3. Assert (التحقق)
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetProductById), createdResult.ActionName);
            Assert.Equal(1, ((Result<ProductsResponseDto>)createdResult.Value).Data.Id);
        }


        // 5️ AddNewProducts - BadRequest (Invalid ModelState)
        [Fact]
        public async Task AddNewProducts_ReturnsBadRequest_WhenModelInvalid()
        {
            // 1. Arrange
            // إضافة خطأ يدوياً لمحاكاة فشل الـ Validation
            _controller.ModelState.AddModelError("Name", "Required");

            var dto = new ProductToCreate();

            // إنشاء Mock صوري فقط لإرضاء باراميترات الدالة
            var mockAuthService = new Mock<IAuthorizationService>();

            // 2. Act
            // يجب تمرير الـ mockAuthService حتى لو لم نصل إليه في الكود
            var result = await _controller.AddNewProducts(dto, mockAuthService.Object);

            // 3. Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            // إضافي: التأكد من أن الرسالة هي التي توقعناها في الـ Controller
            var response = badRequestResult.Value as Result<ProductsResponseDto>;
            Assert.Equal("invaild data.", response!.Message);
        }
        // 6️ GetProductById - NotFound
        [Fact]
        public async Task GetProductById_ReturnsNotFound_WhenProductNotExist()
        {
            _mockService.Setup(s => s.GetProductByIdAsync(1))
                .ReturnsAsync(Result < ProductResponseDto >.Failure("Product not found."));


            var result = await _controller.GetProductById(1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // 7️ Delete - Success
        [Fact]
        public async Task Delete_ReturnsNoContent_WhenUserIsAuthorizedAndProductDeleted()
        {
            // 1. اعداد البيانات (Arrange)
            int productId = 1;

            // عمل Mock لخدمة الصلاحيات
            var mockAuthService = new Mock<IAuthorizationService>();

            // جعل عملية التحقق تنجح (Succeeded = true)
            mockAuthService.Setup(a => a.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                "FarmerOwnerOrAdmin"))
                .ReturnsAsync(AuthorizationResult.Success());

            // عمل Mock لخدمة المنتجات لتُرجع نجاح
            _mockService.Setup(s => s.DeleteProduct(productId))
                .ReturnsAsync(Result<bool>.Success(true));

            // 2. التنفيذ (Act)
            // نمرر الـ mockAuthService.Object للدالة لأنها تستخدم [FromServices]
            var result = await _controller.Delete(productId, mockAuthService.Object);

            // 3. التحقق (Assert)
            Assert.IsType<NoContentResult>(result); // لاحظ التغيير من Ok إلى NoContent
        }
        // 8️ Delete - NotFound
        [Fact]
        public async Task Delete_ReturnsNotFound_WhenProductNotExist()
        {
            // 1. إعداد الـ Mock للصلاحيات (يجب أن تنجح لكي نصل لمرحلة الحذف)
            var mockAuthService = new Mock<IAuthorizationService>();
            mockAuthService.Setup(a => a.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                "FarmerOwnerOrAdmin"))
                .ReturnsAsync(AuthorizationResult.Success());

            // 2. إعداد الـ Mock للخدمة لترجع فشل (المنتج غير موجود)
            _mockService.Setup(s => s.DeleteProduct(1))
                .ReturnsAsync(Result<bool>.Failure("Product not found."));

            // 3. التنفيذ - تمرير المعرف وخدمة الصلاحيات
            var result = await _controller.Delete(1, mockAuthService.Object);

            // 4. التحقق
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Product not found.", notFoundResult.Value);
        }
    }
}
