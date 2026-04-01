using Agricultural_For_CV_DAL.Entities;
using Agricultural_For_CV_DAL.Interfaces;
using Agricultural_For_CV_Shared.Dtos.Products;
using Agricultural_For_CV_Shared.Results;
using Agricultural_For_CV_Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Agricultural_For_CV_Shared.Settings;
using Microsoft.Extensions.Options;
using Agricultural_For_CV_Shared.Dtos.Notifications;
using Agricultural_For_CV_Shared.Enums;
using Agricultural_For_CV_BLL.Interfaces;
using Agricultural_For_CV_Shared.Dtos.LogsDtos;
using Agricultural_For_CV_Shared.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;



namespace Agricultural_For_CV_BLL.Services
{
 
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IImageService _imageService;
        private readonly ILogger<ProductService> _logger;
        private readonly AppSettings _settings;
        private readonly INotificationService _notificationService;
        private readonly IAuditLogService _auditLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;
         

        public ProductService(
            IProductRepository productRepository,
            IImageService imageService,
            ILogger<ProductService> logger,
            IOptions<AppSettings>setting,
            INotificationService notificationService,
            IAuditLogService auditLogService,
            IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = setting.Value ;
            _notificationService = notificationService;
            _auditLogService = auditLogService;
            _httpContextAccessor = httpContextAccessor;
        }


        #region GetAll & GetByUser & GetAllPaggntoions
        public async Task<Result<PagedResult<ProductsResponseDto>>> GetProductsAsync(int page, int pageSize)
        {

            try
            {

                _logger.LogInformation("GetProductsAsync for Page={page}.", page);
                var (items, totalCount) = await _productRepository.GetPagedProductsAsync(page, pageSize);

                var dtos = items.Select(MapToResponseDtoWithFullImage).ToList();

                _logger.LogInformation("GetAllUserProductsAsync succeeded for page={page}. pageSize={pageSize}", page, pageSize);
                return Result<PagedResult<ProductsResponseDto>>.Success(
                new PagedResult<ProductsResponseDto>
                {
                    Items = dtos,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = totalCount
                }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetProductsAsync failed for page={page}.", page);
                return Result<PagedResult<ProductsResponseDto>>.Failure("Failed to retrieve user's products.");
            }

            
        }

        public async Task<Result<PagedResult<ProductsResponseDto>>> GetAllUserProductsAsync(int userId, int page, int pageSize)
        {
            _logger.LogInformation("GetAllUserProductsAsync called for userId={UserId}.", userId);

            if (userId <= 0)
            {
                _logger.LogWarning("Invalid userId: {UserId}", userId);
                return Result<PagedResult<ProductsResponseDto>>.Failure("Invalid user id.");
            }

            try
            {

                _logger.LogInformation("GetProductsAsync for Page={page}.", page);
                var (items, totalCount) = await _productRepository.GetAllUserProductsAsync(userId,page, pageSize);

                var dtos = items.Select(MapToResponseDtoWithFullImage).ToList();

                _logger.LogInformation("GetAllUserProductsAsync succeeded for page={page}. pageSize={pageSize}", page, pageSize);
                return Result<PagedResult<ProductsResponseDto>>.Success(
                new PagedResult<ProductsResponseDto>
                {
                    Items = dtos,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = totalCount
                }
                );
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllUserProductsAsync failed for userId={UserId}.", userId);
                return Result<PagedResult<ProductsResponseDto>>.Failure("Failed to retrieve user's products.");
            }
        }

        public async Task<Result<IEnumerable<ProductsResponseDto>>> GetAllProductsAsync()
        {
            _logger.LogInformation("GetAllProductsAsync called.");

            try
            {
                var products = await _productRepository.GetAllAsyncWithMainImage(true);
                var dtos = products.Select(MapToResponseDtoWithFullImage).ToList();


                _logger.LogInformation("GetAllProductsAsync succeeded. Count={Count}", dtos.Count);
                return Result<IEnumerable<ProductsResponseDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllProductsAsync failed.");
                return Result<IEnumerable<ProductsResponseDto>>.Failure("Failed to retrieve products.");
            }
        }



       
        #endregion 

        #region Read single / by name & by Id / by suggestion

        public async Task<Result<ProductResponseDto>> GetProductAsync(string name)
        {
            _logger.LogInformation("GetProductAsync called for name='{Name}'", name);

            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("GetProductAsync received empty name.");
                return Result<ProductResponseDto>.Failure("Product name is required.");
            }

            try
            {
                var product = await _productRepository.GetAsync(name, includeRelated: true);
                if (product == null)
                {
                    _logger.LogWarning("GetProductAsync product not found: {Name}", name);
                    return Result<ProductResponseDto>.Failure("Product not found.");
                }


                // Map entity to DTO
                var dto = MapToResponseDtoWithFullImageForGetProductBy(product);
                _logger.LogInformation("GetProductAsync succeeded for name='{Name}'", name);

                return Result<ProductResponseDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetProductAsync failed for name='{Name}'", name);
                return Result<ProductResponseDto>.Failure("Failed to retrieve product.");
            }
        }

        public async Task<Result<ProductResponseDto>> GetProductByIdAsync(int id)
        {
            _logger.LogInformation("GetProductByIdAsync called for id={Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("GetProductByIdAsync received invalid id={Id}", id);
                return Result<ProductResponseDto>.Failure("Invalid product id.");
            }

            try
            {
                var product = await _productRepository.GetAsync(id, includeRelated: true);
                if (product == null)
                {
                    _logger.LogWarning("GetProductByIdAsync product not found: {Id}", id);
                    return Result<ProductResponseDto>.Failure("Product not found.");
                }

                var dto = MapToResponseDtoWithFullImageForGetProductBy(product);
                _logger.LogInformation("GetProductByIdAsync succeeded for id={Id}", id);

                return Result<ProductResponseDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetProductByIdAsync failed for id={Id}", id);
                return Result<ProductResponseDto>.Failure("Failed to retrieve product.");
            }
        }

        public async Task<List<ProductsResponseDto>> GetProductsAsync(ProductsFilterDto filter)
        {
            var query = _productRepository
                .GetQueryable()
                .Where(p => !p.IsDeleted);



            if (filter.MinPrice.HasValue)
                query = query.Where(p => p.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);

            if (!string.IsNullOrWhiteSpace(filter.Search))
                query = query.Where(p =>
                    p.Name.Contains(filter.Search) ||
                    p.Description.Contains(filter.Search)
                );


            return await query
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new ProductsResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                })
                .ToListAsync();
        }

        public async Task<Result<IEnumerable<string>>> GetProductSuggestionsAsync(string query)
        {
            try
            {
                var suggestions = await _productRepository.GetSuggestionsAsync(query);
                return Result<IEnumerable<string>>.Success(suggestions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting search suggestions.");
                return Result<IEnumerable<string>>.Failure("Failed to get suggestions.");
            }
        }


        #endregion

        #region Create 

        public async Task<Result<ProductsResponseDto>> AddProductAsync(ProductToCreate createProductDto)
        {
            
            _logger.LogInformation("AddProductAsync called for Name={Name}.", createProductDto?.Name);

            if (createProductDto == null)
                return Result<ProductsResponseDto>.Failure("Invalid product data.");
            

            if (string.IsNullOrWhiteSpace(createProductDto.Name))
                return Result<ProductsResponseDto>.Failure("Product name is required.");


            try
            {
                var product = new Product
                {
                    Name = createProductDto.Name.Trim(),
                    Description = createProductDto.Description?.Trim(),
                    QuantityInStock = createProductDto.QuantityInStock,
                    CropTypeId = createProductDto.CropTypeId,
                    QuantityTypeId = createProductDto.QuantityTypeId,
                    Price = createProductDto.Price,
                    // will get the id from token the right way
                    CreatedBy = createProductDto.CreatedBy,
                    CreatedAt = DateTime.UtcNow
                };

                await _productRepository.AddAsync(product);
                _logger.LogInformation("AddProductAsync succeeded. ProductId={ProductId}", product.Id);


                // 2️⃣ إنشاء إشعار بعد نجاح الإضافة
                await _notificationService.SendNotificationAsync(new CreateNotificationDto
                {
                    UserId = product.CreatedBy, // أو أي مستخدم تريد إشعاره
                    Message = $"تم إضافة المنتج {product.Name} بنجاح",
                    ProductId = product.Id
                });



                var IP = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown";
                var userAgent = _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString() ?? "unknown";

                //string userType = GetUserType.getUserType((UserRole)product.User?.UserType);


                // 🔹 تسجيل الـ Audit Log
                await _auditLogService.LogAsync(new AuditLogDto
                {
                    action = "create",
                    ResourceType = "product",
                    ResourceId = product.Id,
                    Before = null,                // لا يوجد حالة قبل الإضافة
                    After = new
                    {
                        product.Id,
                        product.Name,
                        product.Description,
                        product.Price,
                        product.QuantityInStock,
                        product.CropTypeId,
                        product.QuantityTypeId,
                        product.CreatedAt
                    },
                    ActorId = product.CreatedBy,             // ActorId هو int
                    ActorType = "admin",
                    Metadata = new { reason = "Product created via API", IP, userAgent }
                });


                return Result<ProductsResponseDto>.Success(MapToResponseDtoWithFullImage(product), "Product created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddProductAsync failed.");
                return Result<ProductsResponseDto>.Failure("Failed to add product.");
            }
        }

        #endregion

        #region Update & Delete

        public async Task<Result<ProductsResponseDto>> UpdateProductAsync(ProductToUpdate updateProduct)
        {
            _logger.LogInformation("UpdateProductAsync called for id={Id}.", updateProduct?.Id);

            if (updateProduct == null)
                return Result<ProductsResponseDto>.Failure("Invalid product data.");

            try
            {
                var existing = await _productRepository.GetAsync(updateProduct.Id, includeRelated: true);
                if (existing == null)
                    return Result<ProductsResponseDto>.Failure("Product not found.");


                // product before updated
                var before = new
                {
                    existing.Name,
                    existing.Description,
                    existing.Price,
                    existing.QuantityInStock,
                    existing.CropTypeId,
                    existing.QuantityTypeId,
                    existing.UpdatedAt
                };


                existing.Name = updateProduct.Name?.Trim() ?? existing.Name;
                existing.Description = updateProduct.Description?.Trim() ?? existing.Description;
                existing.QuantityInStock = updateProduct.QuantityInStock;
                existing.CropTypeId = updateProduct.CropTypeId;
                existing.QuantityTypeId = updateProduct.QuantityTypeId;
                existing.Price = updateProduct.Price;
                existing.UpdatedAt = DateTime.UtcNow;
                


                await _productRepository.UpdateAsync(existing);

                _logger.LogInformation("UpdateProductAsync succeeded for id={Id}.", existing.Id);

                var response = MapToResponseDtoWithFullImage(existing);


                // 🔹 لقطة after (الحالة بعد التعديل)
                var after = new
                {
                    existing.Name,
                    existing.Description,
                    existing.Price,
                    existing.QuantityInStock,
                    existing.CropTypeId,
                    existing.QuantityTypeId,
                    existing.UpdatedAt
                };


                await _notificationService.SendNotificationAsync(new CreateNotificationDto
                {
                    UserId = updateProduct.CreatedBy, // أو أي مستخدم تريد إشعاره
                    Message = $"تم تعديل المنتج {existing.Name} بنجاح.",
                    ProductId = existing.Id
                });

                var IP = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown";
                var userAgent = _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString() ?? "unknown";
                string userType = GetUserType.getUserType((UserRole)existing.User?.UserType);


                await _auditLogService.LogAsync( new AuditLogDto {
                    action =  "update",
                    ResourceType= "product",
                    ResourceId= existing.Id,
                    Before= before,
                    After= after,
                    ActorId= existing.CreatedBy,
                    ActorType = userType,
                    Metadata= new { reason = "Product updated via API", IP, userAgent}
               } );


                return Result<ProductsResponseDto>.Success(response, "Product updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateProductAsync failed for id={Id}.", updateProduct?.Id);
                return Result<ProductsResponseDto>.Failure("Failed to update product.");
            }
        }


        /// Soft delete
        public async Task<Result<bool>> DeleteProduct(int id)
        {
            _logger.LogInformation("DeleteProductAsync called for Id={Id}.", id);

            if (id <= 0)
            {
                return Result<bool>.Failure("Invalid product id.");

            }


            try
            {
                var existing = await _productRepository.GetAsync(id, includeRelated: true);
                if (existing == null)
                    return Result<bool>.Failure("Product not found.");


                // 🔹 لقطة قبل الحذف
                // 🔹 snapshot before soft  deleted
                var before = new
                {
                    existing.Id,
                    existing.Name,
                    existing.Description,
                    existing.Price,
                    existing.QuantityInStock,
                    existing.CropTypeId,
                    existing.QuantityTypeId,
                    existing.IsDeleted,
                    existing.UpdatedAt
                };


                // 🔹 تحديث الحقل IsDeleted بدلاً من الحذف الفعلي
                existing.IsDeleted = true;
                existing.UpdatedAt = DateTime.UtcNow;

                // 🔹 حفظ التغييرات
                await _productRepository.UpdateAsync(existing);

                _logger.LogInformation("Product {Id} marked as deleted successfully.", id);

                await _notificationService.SendNotificationAsync(new CreateNotificationDto
                {
                    UserId = id,
                    Message = $"تم حذف المنتج {existing.Name} بنجاح (Soft Delete).",
                    ProductId = existing.Id
                });

                // 🔹 استخراج IP و User-Agent
                var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown";
                var userAgent = _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString() ?? "unknown";
                string userType = GetUserType.getUserType((UserRole)existing.User?.UserType) ?? "unknown";

                var metadataJson = JsonSerializer.Serialize(new { reason = "Product soft deleted via API", ip, userAgent });
            
                // 🔹 snapshot after soft deleted
                var after = new
                {
                    existing.Id,
                    existing.Name,
                    existing.Description,
                    existing.Price,
                    existing.QuantityInStock,
                    existing.CropTypeId,
                    existing.QuantityTypeId,
                    existing.IsDeleted,
                    existing.UpdatedAt
                };



                // 🔹 تسجيل AuditLog
                await _auditLogService.LogAsync( new AuditLogDto {
                    action=  "soft_delete",
                    ResourceType= "product",
                    ResourceId= existing.Id,
                    Before= before,
                    After= after,
                    ActorId= existing.CreatedBy ,
                    ActorType= userType,
                    Metadata= metadataJson
                });



                
                return Result<bool>.Success(true, "Product deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete product {Id}", id);
                return Result<bool>.Failure("Failed to delete product.");
            }
        }
        
        
        #endregion

        #region Helpers

        private ProductsResponseDto MapToResponseDtoWithFullImage(Product product)
        {
            if (product == null) return null!;


            return new ProductsResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CropTypeId = product.CropTypeId,
                CropName = product.Crops?.Name,
                QuantityName = product.QuantityTypes?.TypeName,
                Price = product.Price,
                QuantityInStock = product.QuantityInStock,
                QuantityTypeId = product.QuantityTypeId,
                CreatedBy = product.CreatedBy,
                CreatedAt = product.CreatedAt,
                CreatedByName = product.User?.Username,
                CreatedByImage = product.User?.ImageFile,
                Image = product.ProductsImages != null ? product.ProductsImages.OrderBy(img=>img.ImageOrder).Select(img=>img.ImagePath).FirstOrDefault() : null
            };
        }



        private ProductResponseDto MapToResponseDtoWithFullImageForGetProductBy(Product product)
        {
            if (product == null) return null!;

            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                CropTypeId = product.CropTypeId,
                CreatedAt = product.CreatedAt,
                CreatedBy = product.CreatedBy,
                CropName = product.Crops?.Name,
                QuantityName = product.QuantityTypes?.TypeName,
                Price = product.Price,
                QuantityInStock = product.QuantityInStock,
                QuantityTypeId = product.QuantityTypeId,
                Description = product.Description,
                CreatedByName = product.User?.Username,
                CreatedByImage = product.User?.ImageFile,
                Images = product.ProductsImages != null ? product.ProductsImages.OrderByDescending(img=>img.ImageOrder).Select(img=>img.ImagePath).ToList(): null
            };
        }

        #endregion


        #region Change the status to product --> aprove or reject

        public async Task<Result<bool>> UpdateProductStatusAsync(int id, enProductStatus productStatus)
        {
            // التحقق من صحة الـId
            if (id <= 0)
            {
                _logger.LogWarning("Attempted to update product status with invalid Id: {ProductId}", id);
                return Result<bool>.Failure("Invalid Product Id.");
            }

            // جلب المنتج من المستودع
            var product = await _productRepository.GetAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product not found when trying to update status. ProductId: {ProductId}", id);
                return Result<bool>.Failure("Product not found.");
            }

            try
            {
                // تحديث الحالة
                var oldStatus = product.Status;
                product.Status = productStatus;

                // حفظ التغييرات
                await _productRepository.UpdateAsync(product);

                // تسجيل الحدث
                _logger.LogInformation("Product status updated. ProductId: {ProductId}, OldStatus: {OldStatus}, NewStatus: {NewStatus}",
                                       id, oldStatus, productStatus);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ
                _logger.LogError(ex, "Error occurred while updating product status. ProductId: {ProductId}", id);
                return Result<bool>.Failure("An error occurred while updating product status.");
            }
        }



        #endregion



    }
}
