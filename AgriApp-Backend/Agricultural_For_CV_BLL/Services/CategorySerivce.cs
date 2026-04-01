using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agricultural_For_CV_DAL.Entities;
using Agricultural_For_CV_DAL.Interfaces;
using Agricultural_For_CV_Shared.Dtos.CategoriesDtos;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Results;
using Agricultural_For_CV_Shared.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Agricultural_For_CV_BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IImageService _imageService;
        private readonly AppSettings _appSettings;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IImageService imageService,
            IOptions<AppSettings> setting,
            ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _imageService = imageService;
            _appSettings = setting.Value;
            _logger = logger;
        }

        // ✅ Get all categories
        public async Task<Result<IEnumerable<CategoryResponseDto>>> GetAllAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();

                var result = categories.Select(c => new CategoryResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImageFile = c.ImagePath
                });

                _logger.LogInformation("Retrieved {Count} categories successfully.", result.Count());
                return Result<IEnumerable<CategoryResponseDto>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all categories.");
                return Result<IEnumerable<CategoryResponseDto>>.Failure("An error occurred while retrieving categories.");
            }
        }

        // ✅ Get by id
        public async Task<Result<CategoryResponseDto>> GetById(int id)
        {
            try
            {
                if (id <= 0)
                    return Result<CategoryResponseDto>.Failure("Invalid category ID.");

                var category = await _categoryRepository.GetByIdAsync(id);

                if (category == null)
                {
                    _logger.LogWarning("Category with ID {Id} not found.", id);
                    return Result<CategoryResponseDto>.Failure("Category not found.");
                }

                _logger.LogInformation("Category {Id} retrieved successfully.", id);

                return Result<CategoryResponseDto>.Success(new CategoryResponseDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    ImageFile = category.ImagePath
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category with ID {Id}.", id);
                return Result<CategoryResponseDto>.Failure("An error occurred while retrieving category.");
            }
        }

        // ✅ Add category
        public async Task<Result<CategoryResponseDto>> AddAsync(CreateCategoryDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
            {
                _logger.LogWarning("Category creation failed due to invalid data.");
                return Result<CategoryResponseDto>.Failure("Category name is required.");
            }

            var category = new Category { Name = dto.Name };

            try
            {
                // 🔸 Handle image upload
                var result = await _imageService.SaveImageAsync(dto.ImageFile, _appSettings.ImagePaths.Categories);
                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to save image: {Error}", result.Message);
                }
                category.ImagePath = result.Data;

                
                await _categoryRepository.AddAsync(category);
                _logger.LogInformation("Category '{Name}' added successfully with ID {Id}.", category.Name, category.Id);

                return Result<CategoryResponseDto>.Success(new CategoryResponseDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    ImageFile = category.ImagePath
                });
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(category.ImagePath))
                    _imageService.DeleteImage(category.ImagePath);

                _logger.LogError(ex, "Error adding category '{Name}'.", category.Name);
                return Result<CategoryResponseDto>.Failure("An error occurred while adding category.");
            }
        }

        // ✅ Update category
        public async Task<Result<CategoryResponseDto>> UpdateAsync(UpdateCategoryDto dto)
        {
            if (dto == null || dto.Id <= 0)
            {
                _logger.LogWarning("Invalid category update data.");
                return Result<CategoryResponseDto>.Failure("Invalid category data.");
            }

            var category = await _categoryRepository.GetByIdAsync(dto.Id);
            if (category == null)
            {
                _logger.LogWarning("Category with ID {Id} not found for update.", dto.Id);
                return Result<CategoryResponseDto>.Failure("Category not found.");
            }

            try
            {
                category.Name = dto.Name ?? category.Name;

                // 🔸 Handle image upload
                var result = await _imageService.SaveImageAsync(dto.ImageFile, _appSettings.ImagePaths.Categories);
                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to save image: {Error}", result.Message);
                }
                category.ImagePath = result.Data;

                
                await _categoryRepository.UpdateAsync(category);
                _logger.LogInformation("Category {Id} updated successfully.", dto.Id);

                return Result<CategoryResponseDto>.Success(new CategoryResponseDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    ImageFile = category.ImagePath
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category with ID {Id}.", dto.Id);
                return Result<CategoryResponseDto>.Failure("An error occurred while updating category.");
            }
        }

        // ✅ Delete category
        public async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid category ID for delete operation.");
                return Result<bool>.Failure("Invalid category ID.");
            }

            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                _logger.LogWarning("Category with ID {Id} not found for deletion.", id);
                return Result<bool>.Failure("Category not found.");
            }

            try
            {
                await _categoryRepository.DeleteAsync(id);
                _logger.LogInformation("Category with ID {Id} deleted successfully.", id);

                if (!string.IsNullOrEmpty(category.ImagePath))
                    _imageService.DeleteImage(category.ImagePath);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category with ID {Id}.", id);
                return Result<bool>.Failure("An error occurred while deleting category.");
            }
        }
    }
}
