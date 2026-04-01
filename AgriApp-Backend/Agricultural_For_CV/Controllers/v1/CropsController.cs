using Agricultural_For_CV.Helpers;
using Agricultural_For_CV_Shared.Dtos.CropsDtos;
using Agricultural_For_CV_Shared.Enums;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agricultural_For_CV.Controllers.v1
{
    [Route("api/v{version:ApiVersion}/crops")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Client)]
    public class CropsController : ControllerBase
    {

        private readonly ICropService _cropService;
        private readonly ILoggingService _logger;
        public CropsController(ICropService cropService, ILoggingService logger)
        {
            _cropService = cropService;
            _logger = logger;
        }




        /// <summary>
        /// Retrieves all crops from the database.
        /// Returns a list of crops with detailed information including category and owner.
        /// </summary>
        /// <returns>
        /// Returns <see cref="OkObjectResult"/> containing a list of <see cref="CropResponseDto"/>.
        /// If no crops are found, returns an empty list.
        /// </returns>
        /// <remarks>
        /// Example request:
        /// GET /api/crops
        /// Response:
        /// [
        ///   {
        ///     "Id": 1,
        ///     "Name": "Tomato",
        ///     "CategoryId": 1,
        ///     "CategoryName": "Vegetables",
        ///     "Username": "FarmerAli",
        ///     "OwnerId": 5,
        ///     "ImagePath": "/images/crops/tomato.jpg",
        ///     "CreatedAt": "2025-10-13T12:00:00"
        ///   },
        ///   ...
        /// ]
        /// </remarks>
        [HttpGet("All")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Result<IEnumerable<CropResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<IEnumerable<CropResponseDto>>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllCrops()
        {
            var result = await _cropService.GetAllAsync();

            string baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";

            if (!result.IsSuccess)
                return BadRequest(result);


            return Ok(result.Data?.Select(c => new
            {
                c.Id,
                c.Name,
                c.OwnerId,
                c.CategoryId,
                c.CreatedAt,
                ImagePath = string.IsNullOrEmpty(c.ImagePath) ? null : UrlHelper.GetFullUrl(baseUrl, c.ImagePath),
                c?.Username,
                c?.CategoryName
            }));
        }


        /// <summary>
        /// Retrieves all crops for a specific user based on their OwnerId.
        /// </summary>
        /// <param name="ownerId">
        /// The ID of the user whose crops are being retrieved.
        /// </param>
        /// <returns>
        /// Returns <see cref="OkObjectResult"/> containing a list of <see cref="CropResponseDto"/> belonging to the specified user.
        /// Returns an empty list if the user has no crops.
        /// </returns>
        /// <remarks>
        /// Example request:
        /// GET /api/crops/user/5
        /// Response:
        /// [
        ///   {
        ///     "Id": 1,
        ///     "Name": "Tomato",
        ///     "CategoryId": 1,
        ///     "CategoryName": "Vegetables",
        ///     "Username": "FarmerAli",
        ///     "OwnerId": 5,
        ///     "ImagePath": "/images/crops/tomato.jpg",
        ///     "CreatedAt": "2025-10-13T12:00:00"
        ///   },
        ///   ...
        /// ]
        /// </remarks>
        [HttpGet("ownerId")]
        [ProducesResponseType(typeof(Result<IEnumerable<CropResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<IEnumerable<CropResponseDto>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result<IEnumerable<CropResponseDto>>), StatusCodes.Status400BadRequest)]
        [Role([UserRole.Admin, UserRole.Farmer, UserRole.Customer])]
        public async Task<IActionResult> GetAllCropsByUserId(int ownerId)
        {
            var result = await _cropService.GetCropsByUserIdAsync(ownerId);
            string baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";

            if (!result.IsSuccess)
                return NotFound(result);




            return Ok(result.Data?.Select(c => new
            {
                c.Id,
                c.Name,
                c.OwnerId,
                c.CategoryId,
                c.CreatedAt,
                ImagePath = string.IsNullOrEmpty(c.ImagePath) ? null : UrlHelper.GetFullUrl(baseUrl, c.ImagePath),
                c?.Username,
                c?.CategoryName
            }));
        }


        /// <summary>
        /// Retrieves crops by their category ID.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <returns>
        /// Returns <see cref="OkObjectResult"/> containing a list of <see cref="CropResponseDto"/> in the specified category.
        /// Returns an empty list if no crops exist in this category.
        /// </returns>
        /// <remarks>
        /// Example request:
        /// GET /api/crops/category/1
        /// Response:
        /// [
        ///   {
        ///     "Id": 1,
        ///     "Name": "Tomato",
        ///     "CategoryId": 1,
        ///     "CategoryName": "Vegetables",
        ///     "Username": "FarmerAli",
        ///     "OwnerId": 5,
        ///     "ImagePath": "/images/crops/tomato.jpg",
        ///     "CreatedAt": "2025-10-13T12:00:00"
        ///   }
        /// ]
        /// </remarks>
        [HttpGet("categoryId")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Result<IEnumerable<CropResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<IEnumerable<CropResponseDto>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result<IEnumerable<CropResponseDto>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllCropsByCategoryId(int categoryId)
        {
            var result = await _cropService.GetCropsByCategoryIdAsync(categoryId);
            string baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";

            if (!result.IsSuccess)
                return NotFound(result);




            return Ok(result.Data?.Select(c => new
            {
                c.Id,
                c.Name,
                c.OwnerId,
                c.CategoryId,
                c.CreatedAt,
                ImagePath = string.IsNullOrEmpty(c.ImagePath) ? null : UrlHelper.GetFullUrl(baseUrl, c.ImagePath),
                c?.Username,
                c?.CategoryName
            }));
        }


        /// <summary>
        /// Retrieves crops by their name (partial or full match).
        /// </summary>
        /// <param name="name">The name of the crop to search for.</param>
        /// <returns>
        /// Returns <see cref="OkObjectResult"/> containing a list of <see cref="CropResponseDto"/> matching the name.
        /// Returns an empty list if no crops match.
        /// </returns>
        /// <remarks>
        /// Example request:
        /// GET /api/crops/search?name=Tomato
        /// Response:
        /// [
        ///   {
        ///     "Id": 1,
        ///     "Name": "Tomato",
        ///     "CategoryId": 1,
        ///     "CategoryName": "Vegetables",
        ///     "Username": "FarmerAli",
        ///     "OwnerId": 5,
        ///     "ImagePath": "/images/crops/tomato.jpg",
        ///     "CreatedAt": "2025-10-13T12:00:00"
        ///   }
        /// ]
        /// </remarks>
        [HttpGet("searchByName")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Result<IEnumerable<CropResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<IEnumerable<CropResponseDto>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result<IEnumerable<CropResponseDto>>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllCropsByName(string name)
        {
            var result = await _cropService.SearchCropsByNameAsync(name);

            string  baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";
            if (!result.IsSuccess)
                return NotFound(result);


            return Ok(result.Data?.Select(c => new
            {
                c.Id,
                c.Name,
                c.OwnerId,
                c.CategoryId,
                c.CreatedAt,
                ImagePath = string.IsNullOrEmpty(c.ImagePath) ? null : UrlHelper.GetFullUrl(baseUrl, c.ImagePath),
                c?.Username,
                c?.CategoryName
            }));
        }


        /// <summary>
        /// Retrieves a crop by its unique identifier.
        /// </summary>
        /// <param name
        /// ="cropId">The unique ID of the crop.</param>
        /// <returns>
        /// Returns <see cref="OkObjectResult"/> containing the <see cref="CropResponseDto"/> if found.
        /// Returns <see cref="NotFoundResult"/> if the crop does not exist.
        /// </returns>
        /// <remarks>
        /// Example request:
        /// GET /api/crops/1
        /// Response:
        /// {
        ///   "Id": 1,
        ///   "Name": "Tomato",
        ///   "CategoryId": 1,
        ///   "CategoryName": "Vegetables",
        ///   "Username": "FarmerAli",
        ///   "OwnerId": 5,
        ///   "ImagePath": "/images/crops/tomato.jpg",
        ///   "CreatedAt": "2025-10-13T12:00:00"
        /// }
        /// </remarks>
        [HttpGet("{cropId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Result<CropResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<CropResponseDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<CropResponseDto>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult>GetCropById(int cropId)
        {

            var result = await _cropService.GetByIdAsync(cropId);
            
            string  baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";
            if (!result.IsSuccess)
                return NotFound(result);

            if (result.Data?.ImagePath != null)
                result.Data.ImagePath = string.IsNullOrEmpty(result.Data.ImagePath) ? null : UrlHelper.GetFullUrl(baseUrl, result.Data.ImagePath);


            return Ok(result.Data);
        }

        /// <summary>
        /// Creates a new crop and stores it in the database.
        /// Returns a 201 Created response with the location of the newly created crop.
        /// </summary>
        /// <param name="dto">
        /// The data transfer object containing the crop details:
        /// <list type="bullet">
        /// <item><description>Name: The name of the crop.</description></item>
        /// <item><description>CategoryId: The ID of the crop category.</description></item>
        /// <item><description>OwnerId: The ID of the user who owns the crop.</description></item>
        /// <item><description>ImagePath: Optional image file for the crop.</description></item>
        /// </list>
        /// </param>
        /// <returns>
        /// Returns <see cref="CreatedAtActionResult"/> with the newly created crop data and a link to <see cref="GetCropById(int)"/>.
        /// Returns <see cref="BadRequestObjectResult"/> if creation fails.
        /// </returns>
        /// <remarks>
        /// Example request:
        /// POST /api/crops
        /// FormData:
        /// - Name: "Tomato"
        /// - CategoryId: 1
        /// - OwnerId: 5
        /// - ImagePath: file (optional)
        /// </remarks>
        [HttpPost("AddNewCrop")]
        [ProducesResponseType(typeof(Result<CropResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Result<CropResponseDto>), StatusCodes.Status400BadRequest)]
        [Role([UserRole.Admin])]
        public async Task<IActionResult> AddNewCrop(CreateCropDto dto)
        {
            var result = await _cropService.CreateAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(result);

        
            return CreatedAtAction(nameof(GetCropById),new { cropId = result.Data!.Id }, dto);
        }






        /// <summary>
        /// Updates an existing crop in the database.
        /// Returns a 200 OK response with the updated crop data.
        /// </summary>
        /// <param name="dto">
        /// The data transfer object containing the updated crop details:
        /// <list type="bullet">
        /// <item><description>Id: The unique identifier of the crop to be updated.</description></item>
        /// <item><description>Name: The updated name of the crop.</description></item>
        /// <item><description>CategoryId: The updated category ID of the crop.</description></item>
        /// <item><description>OwnerId: The ID of the user who owns the crop.</description></item>
        /// <item><description>ImagePath: Optional new image file for the crop.</description></item>
        /// </list>
        /// </param>
        /// <returns>
        /// Returns <see cref="OkObjectResult"/> with the updated crop data if the update succeeds.
        /// Returns <see cref="NotFoundResult"/> if the crop with the specified Id does not exist.
        /// Returns <see cref="BadRequestObjectResult"/> if the update fails due to validation or other issues.
        /// </returns>
        /// <remarks>
        /// Example request:
        /// PUT /api/crops
        /// FormData:
        /// - Id: 10
        /// - Name: "Updated Tomato"
        /// - CategoryId: 1
        /// - OwnerId: 5
        /// - ImagePath: file (optional)
        /// </remarks>
        /// 
        [HttpPut("UpdateCrop")]
        [ProducesResponseType(typeof(Result<CropResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<CropResponseDto>), StatusCodes.Status400BadRequest)]
        [Role([UserRole.Admin])]
        public async Task<IActionResult> UpdateCrop(UpdateCropDto dto)
        {

            try
            {
                var result = await _cropService.UpdateAsync(dto);
            
                if (!result.IsSuccess)
                return BadRequest(result);

                return Ok(result);
            }catch(Exception ex)
            {
                await _logger.LogErrorAsync("Failed: An Error Occured", ex);
                return BadRequest(ex.Message.ToString());
            }






        }


        /// <summary>
        /// Deletes a crop from the database by its unique identifier.
        /// </summary>
        /// <param name="cropId">The unique ID of the crop to be deleted.</param>
        /// <returns>
        /// Returns <see cref="NoContentResult"/> (204) if deletion succeeds.
        /// Returns <see cref="NotFoundResult"/> (404) if the crop does not exist.
        /// Returns <see cref="BadRequestObjectResult"/> (400) if deletion fails due to constraints or other errors.
        /// </returns>
        /// <remarks>
        /// Example request:
        /// DELETE /api/crops/1
        /// </remarks>
        [HttpDelete("{cropId}")]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status404NotFound)]
        [Role([UserRole.Admin])]
        public async Task<IActionResult>DeleteCrop(int cropId)
        {
            var result = await _cropService.DeleteAsync(cropId);
        
            if(!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }


    }
}
