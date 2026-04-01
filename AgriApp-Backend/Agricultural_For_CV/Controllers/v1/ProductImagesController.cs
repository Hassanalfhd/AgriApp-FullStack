using Agricultural_For_CV.Helpers;
using Agricultural_For_CV_BLL.Interfaces;
using Agricultural_For_CV_Shared.Dtos.ProductsImages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agricultural_For_CV_API.Controllers
{
    [ApiController]
    [Route("api/v{version:ApiVersion}/products/images")]
    [ApiVersion("1.0")]
    [Authorize]
    public class ProductImagesController : ControllerBase
    {
        private readonly IProductImageService _productImageService;

        public ProductImagesController(IProductImageService imageService)
        {
            _productImageService = imageService;
        }

        /// <summary>
        /// Retrieves all images for a specific product.
        /// </summary>
        /// <param name="productId">The ID of the product.</param>
        /// <returns>A list of images associated with the product.</returns>
        /// <response code="200">Images retrieved successfully.</response>
        /// <response code="404">No images found for the specified product.</response>
        [HttpGet("{productId:int}")]
        public async Task<IActionResult> GetImages(int productId)
        {
            var result = await _productImageService.GetImagesAsync(productId);

            string baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";
            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result.Data?.Select(c => new
            {
                c.Id,
                ImagePath = string.IsNullOrEmpty(c.ImagePath) ? null : UrlHelper.GetFullUrl(baseUrl, c.ImagePath),
                c.ImageOrder                
            }));

        }



        /// <summary>
        /// Uploads a single image for a product.
        /// </summary>
        /// <param name="productId">The product ID.</param>
        /// <param name="dto.file">The image file to upload.</param>
        /// <param imageOrder="dto.ImageOrder">Optional: the display order of the image.</param>
        /// <returns>The uploaded image details.</returns>
        /// <response code="200">Image uploaded successfully.</response>
        /// <response code="400">Failed to upload image due to invalid data.</response>
        [HttpPost("upload-single")]
        public async Task<IActionResult> UploadSingle(int productId,  UploadImageDto dto)
        {
            if (dto.File == null)
                return BadRequest(new { message = "No image file provided." });


            var result = await _productImageService.AddImageAsync(productId, dto.File, dto.ImageOrder);

            string baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";
            if (result.Data?.ImagePath != null)
                result.Data.ImagePath = string.IsNullOrEmpty(result.Data.ImagePath) ? null : UrlHelper.GetFullUrl(baseUrl, result.Data.ImagePath);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Uploads multiple images for a product at once.
        /// </summary>
        /// <param name="productId">The product ID.</param>
        /// <param name="files">The array of image files to upload.</param>
        /// <returns>The details of all uploaded images.</returns>
        /// <response code="200">Images uploaded successfully.</response>
        /// <response code="400">Failed to upload images due to invalid data.</response>
        [HttpPost("upload-multiple")]
        public async Task<IActionResult> UploadMultiple(int productId,  IFormFile[] files)
        {
            if (files == null || files.Length == 0)
                return BadRequest(new { message = "No image files provided." });

            var result = await _productImageService.AddMultipleImagesAsync(productId, files);

            string baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";
            if (!result.IsSuccess)
                return BadRequest(result);


            return Ok(result.Data?.Select(c => new
            {
                c.Id,
                ImagePath = string.IsNullOrEmpty(c.ImagePath) ? null : UrlHelper.GetFullUrl(baseUrl, c.ImagePath),
                c.ImageOrder
            }));
        }

        /// <summary>
        /// Deletes a specific image of a product.
        /// </summary>
        /// <param name="imageId">The ID of the image to delete.</param>
        /// <returns>Success or failure result of the deletion.</returns>
        /// <response code="200">Image deleted successfully.</response>
        /// <response code="404">Image not found.</response>
        [HttpDelete("Delete/{imageId}")]
        public async Task<IActionResult> Delete(int imageId)
        {
            var result = await _productImageService.DeleteImageAsync(imageId);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}
