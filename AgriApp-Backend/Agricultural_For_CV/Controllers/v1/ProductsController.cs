using System.Security.Claims;
using Agricultural_For_CV.Helpers;
using Agricultural_For_CV_DAL.Entities;
using Agricultural_For_CV_Shared.Dtos.Products;
using Agricultural_For_CV_Shared.Dtos.UserDtos;
using Agricultural_For_CV_Shared.Enums;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Agricultural_For_CV.Controllers.v1
{
    [Route("api/v{version:ApiVersion}/Products")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }



        /// <summary>
        /// Retrieves all products available in the system.
        /// </summary>
        /// <remarks>
        /// This endpoint returns all products with their main images (if available).
        /// It constructs full image URLs dynamically based on the current request host.
        /// </remarks>
        /// <response code="200">Returns the list of products successfully.</response>
        /// <response code="404">Returned when no products were found.</response>
        [HttpGet("All")]
        [ProducesResponseType(typeof(Result<IEnumerable<ProductsResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<IEnumerable<ProductsResponseDto>>), StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> GetAllProducts()
        {
               
            var result = await _productService.GetAllProductsAsync();

            if (!result.IsSuccess)
                return NotFound(result);
            string baseUrl = "";
            if (Request != null)
                baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";


            

            return Ok(result.Data?.Select(p => new
            {
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.CreatedAt,
                p.CreatedBy,
                p.CropTypeId,
                p.QuantityName,
                p.CropName,
                p.CreatedByName,
                CreatedByImage = UrlHelper.GetFullUrl(baseUrl, p.CreatedByImage),
                p.QuantityTypeId,
                p.QuantityInStock,
                image = (p.Image!= null && p.Image.Any())
? new List<string> { UrlHelper.GetFullUrl(baseUrl, p.Image) }
: null
            }));
        }


        /// <summary>
        /// Retrieves a list of products filtered by name and price
        /// </summary>
        /// <remarks>
        /// Example request:
        /// 
        ///     GET /api/products?search=tomato&minPrice=500&maxPrice=2000
        /// 
        /// </remarks>
        /// <param name="filter">Filters to apply: search term, min and max price</param>
        /// <returns>List of products matching the filter</returns>
        /// <response code="200">Returns filtered products</response>
        /// <response code="400">If filter parameters are invalid</response>
        [HttpGet("filters")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Role(UserRole.Admin)]
        //[ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Client)]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> GetProductsFilter([FromQuery] ProductsFilterDto filter, [FromServices] IAuthorizationService authorizationService)
        {

            var IdentityClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(IdentityClaim) || !int.TryParse(IdentityClaim, out int userId))
                return Unauthorized();


            var authResult = await authorizationService.AuthorizeAsync(User, userId, "FarmerOwnerOrAdmin");
            if (!authResult.Succeeded)
                return Forbid(); // Returns HTTP 403 Forbidden


            var result = await _productService.GetProductsAsync(filter);
            return Ok(result);
        }




        /// <summary>
        /// Endpoint للزوار: جلب المنتجات بشكل Paginated مع تخزين مؤقت في المتصفح
        /// </summary>
        /// <param name="page">رقم الصفحة المطلوبة (افتراضي = 1)</param>
        /// <param name="pageSize">عدد المنتجات في كل صفحة (افتراضي = 20)</param>
        /// <returns>PagedResult يحتوي على المنتجات، رقم الصفحة، حجم الصفحة، وعدد العناصر الكلي</returns>
        [HttpGet("Page")]
        [AllowAnonymous]

        [ResponseCache(
            Duration = 60, // مدة التخزين في المتصفح 3600 ثانية = 1 ساعة
            Location = ResponseCacheLocation.Client // التخزين يكون في المتصفح فقط

        )]
        public async Task<IActionResult> GetProducts(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20
        )
        {
            var result = await _productService.GetProductsAsync(page, pageSize);
            
            string baseUrl = "";
            if (Request != null)
                baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";

            result.Data!.Items.ForEach(item =>
            {
                item.Image = UrlHelper.GetFullUrl(baseUrl, item.Image);
                item.CreatedByImage = UrlHelper.GetFullUrl(baseUrl, item.CreatedByImage);
            });
            


            return Ok(result.Data);
        }


        /// <summary>
        /// Endpoint للمسؤول: جلب المنتجات بشكل Paginated بدون أي تخزين مؤقت
        /// </summary>
        /// <param name="page">رقم الصفحة المطلوبة (افتراضي = 1)</param>
        /// <param name="pageSize">عدد المنتجات في كل صفحة (افتراضي = 20)</param>
        /// <returns>PagedResult يحتوي على المنتجات، رقم الصفحة، حجم الصفحة، وعدد العناصر الكلي</returns>
        [HttpGet("adminPage")]
        [Role(UserRole.Admin)]
        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true
        )]
        public async Task<IActionResult> GetAdminProducts(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20
        )
        {

            var result = await _productService.GetProductsAsync(page, pageSize);
            

            string baseUrl = "";
            if (Request != null)
                baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";


            result.Data!.Items.ForEach(item =>
            {
                item.Image = UrlHelper.GetFullUrl(baseUrl, item.Image);
                item.CreatedByImage = UrlHelper.GetFullUrl(baseUrl, item.CreatedByImage);
            });

            return Ok(result.Data);
        }


        /// <summary>
        /// Retrieves all products created by a specific user.
        /// </summary>
        /// <param name="page">The Number of page.</param>
        /// <param name="pageSize">The Number of Elements that you want.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <remarks>
        /// Use this endpoint to view all products uploaded by a specific user.
        /// The endpoint also attaches full image URLs for display purposes.
        /// </remarks>
        /// <response code="200">Returns the list of the user's products successfully.</response>
        /// <response code="400">Returned when the request parameters are invalid.</response>
        /// <response code="404">Returned when the user has no products.</response>
        [HttpGet("UserProducts")]
        [Role(UserRole.Farmer)]
        [ProducesResponseType(typeof(Result<PagedResult<ProductsResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<PagedResult<ProductsResponseDto>>), StatusCodes.Status400BadRequest)]
        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true
        )]
        public async Task<IActionResult> GetAllUserProducts(
            [FromServices] IAuthorizationService authorizationService,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] int userId = -1
        )
        {

            if (userId< 1 )
                return BadRequest(Result<UserResponseDto>.Failure("Invalid or missing user ID ."));


            
            var authResult = await authorizationService.AuthorizeAsync(User, userId, "FarmerOwnerOrAdmin");


            if (!authResult.Succeeded)
                return Forbid(); // Returns HTTP 403 Forbidden


            var result = await _productService.GetAllUserProductsAsync(userId, page, pageSize );

            
            if (!result.IsSuccess)
                return BadRequest(result);


            string baseUrl = "";
            if (Request != null)
                baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";


            result.Data!.Items.ForEach(item =>
            {
                item.Image = UrlHelper.GetFullUrl(baseUrl, item.Image);
                item.CreatedByImage = UrlHelper.GetFullUrl(baseUrl, item.CreatedByImage);
            });

            return Ok(result.Data);
        }


        /// <summary>
        /// Provides dynamic search suggestions for product names based on user input.
        /// </summary>
        /// <param name="query">The partial search term entered by the user (e.g., "tom" → "Tomato").</param>
        /// <remarks>
        /// This endpoint is designed to enhance the user experience by offering real-time search suggestions
        /// while the user types in the search box.  
        /// The search is case-insensitive and limited to the top 10 matching product names.
        /// </remarks>
        /// <response code="200">Returns a list of matching product names successfully.</response>
        /// <response code="400">Returned when the query string is null or empty.</response>
        /// <response code="404">Returned when no product suggestions are found.</response>
        [HttpGet("SearchSuggestions")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Result<IEnumerable<string>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<IEnumerable<string>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<IEnumerable<string>>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSearchSuggestions([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest(Result<IEnumerable<string>>.Failure("Invalid query."));

            var result = await _productService.GetProductSuggestionsAsync(query);
            
            if (!result.IsSuccess)
                return NotFound(result);
            if(result.Data == null )
                return NotFound(result);


            return Ok(result.Data);
        }


        /// <summary>
        /// Adds a new product to the system.
        /// </summary>
        /// <param name="dto">The product details to create.</param>
        /// <remarks>
        /// This endpoint allows you to add a new product, including uploading product images.  
        /// The images should be provided using a multipart/form-data request.
        /// </remarks>
        /// <response code="201">Returned when the product is created successfully.</response>
        /// <response code="400">Returned when the product creation fails due to validation or internal issues.</response>
        [HttpPost("AddNewProduct")]
        [ProducesResponseType(typeof(Result<IEnumerable<ProductsResponseDto>>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Result<IEnumerable<ProductsResponseDto>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result<IEnumerable<ProductsResponseDto>>), StatusCodes.Status400BadRequest)]
        [Role([UserRole.Admin, UserRole.Farmer])]
        public async Task<IActionResult> AddNewProducts([FromBody] ProductToCreate dto, [FromServices] IAuthorizationService authorizationService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Result<ProductsResponseDto>.Failure("invaild data."));
            }


            var authResult = await authorizationService.AuthorizeAsync(User, dto.CreatedBy, "FarmerOwnerOrAdmin");


            if (!authResult.Succeeded)
                return Forbid(); // Returns HTTP 403 Forbidden


            var result = await _productService.AddProductAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetProductById), new {id = result.Data.Id}, result);
        }


        /// <summary>
        /// Retrieves a product by its unique identifier (Id).
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>
        /// Returns the product data including images (with full URLs) if found.
        /// </returns>
        /// <response code="200">Returns the product details successfully.</response>
        /// <response code="404">Returned when the product with the specified Id does not exist.</response>
        /// <response code="400">Returned when the request is invalid.</response>

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Result<ProductResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<ProductResponseDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result<ProductResponseDto>), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            if (!result.IsSuccess || result.Data == null)
                return NotFound(Result<string>.Failure("Product not found."));


            if (!result.IsSuccess)
                return NotFound(result);
            
            


            var baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";

            if (result.Data.Images != null)
                result.Data.Images = result.Data.Images
                    .Select(img => UrlHelper.GetFullUrl(baseUrl, img))
                    .ToList();
           
            else
                result.Data.Images = null;


            if (result.Data.CreatedByImage != null) 
                    result.Data.CreatedByImage = UrlHelper.GetFullUrl(baseUrl, result.Data.CreatedByImage);
            else
                result.Data.CreatedByImage = null;



                    return Ok(result.Data);
        }



        /// <summary>
        /// Retrieves a product by its name.
        /// </summary>
        /// <param name="name">The name of the product to search for.</param>
        /// <returns>
        /// Returns the product data including images (with full URLs) if found.
        /// </returns>
        /// <response code="200">Returns the product details successfully.</response>
        /// <response code="404">Returned when no product with the specified name is found.</response>

        [HttpGet("GetByName/{name}")]
        [ProducesResponseType(typeof(Result<ProductsResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<ProductsResponseDto>), StatusCodes.Status404NotFound)]
         public async Task<IActionResult> GetByName(string name)
        {

            var result = await _productService.GetProductAsync(name);
            if (!result.IsSuccess || result.Data == null)
                return NotFound(Result<string>.Failure(result.Error ?? "Product not found."));

            if (!result.IsSuccess)
                return NotFound(Result<string>.Failure(result.Error));

            // معالجة الصور لإرجاع روابط كاملة
            var baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";

            if (result.Data.Images != null)
                result.Data.Images = result.Data.Images
                    .Select(img => UrlHelper.GetFullUrl(baseUrl, img))
                    .ToList();
            else
                result.Data.Images = null;

            return Ok(result);
        }



        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="updateProduct">The updated product data including images.</param>
        /// <returns>
        /// Returns the updated product data with full image URLs if successful.
        /// </returns>
        /// <response code="200">Product updated successfully.</response>
        /// <response code="400">Returned when validation fails or update fails.</response>

        [HttpPut("Update")]
        [ProducesResponseType(typeof(Result<ProductsResponseDto>), 200)]
        [ProducesResponseType(400)]
        [Role([UserRole.Admin, UserRole.Farmer])]
        public async Task<IActionResult> Update([FromBody] ProductToUpdate updateProduct, [FromServices] IAuthorizationService authorizationService)
        {
            if (!ModelState.IsValid)
                return BadRequest(Result<string>.Failure("Invalid product data."));

            if (updateProduct.Id < 1 || updateProduct == null)
                return BadRequest(Result<UserResponseDto>.Failure("Invalid or missing user ID ."));


            var authResult = await authorizationService.AuthorizeAsync(User, updateProduct.CreatedBy, "FarmerOwnerOrAdmin");


            if (!authResult.Succeeded)
                return Forbid(); // Returns HTTP 403 Forbidden


            var result = await _productService.UpdateProductAsync(updateProduct);

            if (!result.IsSuccess)
                return BadRequest(result);

            

            return Ok(result);
        }


        /// <summary>
        /// Deletes a product by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>
        /// Returns a boolean indicating whether the deletion was successful.
        /// </returns>
        /// <response code="200">Product deleted successfully.</response>
        /// <response code="404">Returned when the product with the specified ID does not exist.</response>

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        [ProducesResponseType(404)]
        [Role([UserRole.Admin, UserRole.Farmer])]
        public async Task<IActionResult> Delete(int id, [FromServices] IAuthorizationService authorizationService)
        {

            if (id < 1)
                return BadRequest(Result<UserResponseDto>.Failure("Invalid or missing user ID ."));


            var authResult = await authorizationService.AuthorizeAsync(User, id, "FarmerOwnerOrAdmin");


            if (!authResult.Succeeded)
                return Forbid(); // Returns HTTP 403 Forbidden



            var result = await _productService.DeleteProduct(id);



            if (!result.IsSuccess)
                return NotFound(result.Message);

         
            return NoContent();
        }





        /// <summary>
        /// Update the status of a product (e.g., Approve, Reject, Pending).
        /// Only admins can perform this action. 1- Pending 2-Approved 3- Rejected 
        /// </summary>
        /// <param name="id">Product ID.</param>
        /// <param name="status">New product status.</param>
        /// <returns>Result of the update operation.</returns>
        [HttpPut("{id:int}/status")]
        [Role(UserRole.Admin)]
        public async Task<ActionResult<Result<bool>>> UpdateStatus(int id, [FromQuery] enProductStatus status)
        {
            try
            {
                var result = await _productService.UpdateProductStatusAsync(id, status);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to update product {ProductId} status: {Message}", id, result.Message);
                    return BadRequest(result); 
                }

                _logger.LogInformation("Product {ProductId} status updated to {Status}", id, status);
                return Ok(Result<bool>.Success(true, "Product status updated successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating product {ProductId} status", id);
                return StatusCode(500, Result<bool>.Failure("An internal error occurred while updating the product status."));
            }
        }


    }
}


