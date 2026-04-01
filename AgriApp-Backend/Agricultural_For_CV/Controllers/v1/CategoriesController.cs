using Agricultural_For_CV.Helpers;
using Agricultural_For_CV_Shared.Dtos.CategoriesDtos;
using Agricultural_For_CV_Shared.Enums;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Agricultural_For_CV.Controllers.v1
{
    [Route("api/v{version:ApiVersion}/Categories")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Client)]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            

        }



        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns>List of categories with full image URLs.</returns>
        /// <response code="200">Returns the list of categories successfully.</response>
        /// <response code="400">Returned when an error occurs while fetching categories.</response>
          

        [HttpGet("Get-All", Name = "Get-All")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Result<IEnumerable<CategoryResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<IEnumerable<CategoryResponseDto>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllCategories()
        {

            var result = await _categoryService.GetAllAsync();

            string baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result.Data?.Select(c=> new
            {
                c.Id,
                c.Name,
                ImageFile = string.IsNullOrEmpty(c.ImageFile) ? null : UrlHelper.GetFullUrl(baseUrl, c.ImageFile)

            }));
        }



        /// <summary>
        /// Retrieves a category by its ID.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <returns>Category data including full image URL.</returns>
        /// <response code="200">Category retrieved successfully.</response>
        /// <response code="404">Returned when the category does not exist.</response>
        [HttpGet("{categoryId:int}")]
        //[Role([UserRole.Admin])]
        [ProducesResponseType(typeof(Result<CategoryResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<CategoryResponseDto>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult>GetCategoryById(int categoryId)
        {

            var result = await _categoryService.GetById(categoryId);
            string baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";

            if (!result.IsSuccess)
                return NotFound(result);

            if (result.Data?.ImageFile != null)
                result.Data.ImageFile = string.IsNullOrEmpty(result.Data.ImageFile) ? null : UrlHelper.GetFullUrl(baseUrl, result.Data.ImageFile);

            return Ok(result.Data);
        }



        /// <summary>
        /// Adds a new category.
        /// </summary>
        /// <param name="dto">Category data including name and image.</param>
        /// <returns>The newly created category data.</returns>
        /// <response code="201">Category created successfully.</response>
        /// <response code="400">Returned when the category could not be created.</response>

        [HttpPost("AddNewCategory", Name = "AddNewCategory")]
        [Role([UserRole.Admin])]
        [ProducesResponseType(typeof(Result<CategoryResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Result<CategoryResponseDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddNewCategory(CreateCategoryDto dto)
        {
            var result = await _categoryService.AddAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(result);


            return CreatedAtAction(nameof(GetCategoryById), new { categoryId = result.Data.Id}, result);
        }


        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="dto">The updated category data.</param>
        /// <returns>The updated category data.</returns>
        /// <response code="200">Category updated successfully.</response>
        /// <response code="400">Returned when the update fails.</response>

        [HttpPut("UpdateCategory", Name = "UpdateCategory")]
        [ProducesResponseType(typeof(Result<CategoryResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<CategoryResponseDto>), StatusCodes.Status400BadRequest)]
        [Role([UserRole.Admin])]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto dto)
        {
            var result = await _categoryService.UpdateAsync(dto);
            string baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";

            if (result.Data?.ImageFile != null)
                result.Data.ImageFile = string.IsNullOrEmpty(result.Data.ImageFile) ? null : UrlHelper.GetFullUrl(baseUrl, result.Data.ImageFile);

            return Ok(result);
        }


        /// <summary>
        /// Deletes a category by its ID.
        /// </summary>
        /// <param name="categoryId">The ID of the category to delete.</param>
        /// <returns>Boolean indicating success of deletion.</returns>
        /// <response code="200">Category deleted successfully.</response>
        /// <response code="400">Returned when deletion fails.</response>
        [HttpDelete("Delete/{categoryId:int}")]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status400BadRequest)]
        [Role([UserRole.Admin])]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {

            var result = await _categoryService.DeleteAsync(categoryId);


            if(!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);

        }
        

    }
}
