using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Settings;
using Agricultural_For_CV_Shared.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Agricultural_For_CV_BLL.Services
{
    public class ImageService : IImageService
    {
        private readonly AppSettings _settings;
        private readonly ILogger<ImageService> _logger;

        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(2,2);

        public ImageService(IOptions<AppSettings> settings, ILogger<ImageService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<Result<string>> SaveImageAsync(IFormFile file, string folder)
        {
            // 1. التحقق الأولي
            if (file == null || file.Length == 0)
                return Result<string>.Failure("Invalid file.");

            if (file.Length > 5 * 1024 * 1024)
                return Result<string>.Failure("File size exceeds the 5MB limit.");


            await _semaphore.WaitAsync();

            try
            {
                // انتظر السيمافور هنا قبل الدخول في المعالجة الثقيلة
                // await _semaphore.WaitAsync(); 

                var fileName = $"{Guid.NewGuid()}.webp";
                var uploadsFolder = Path.Combine(_settings.wwwrootPath, folder);
                var filePath = Path.Combine(uploadsFolder, fileName);

                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                // 3. استخدام ImageSharp لمعالجة الصورة
                using (var image = await Image.LoadAsync(file.OpenReadStream()))
                {
                    int maxWidth = 1200;
                    if (image.Width > maxWidth)
                    {
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new Size(maxWidth, 0),
                            Mode = ResizeMode.Max
                        }));
                    }

                    var encoder = new WebpEncoder
                    {
                        Quality = 75,
                        FileFormat = WebpFileFormatType.Lossy,
                        Method = WebpEncodingMethod.Level4
                    };

                    await image.SaveAsync(filePath, encoder);
                }

                var relativePath = Path.Combine(folder, fileName).Replace("\\", "/");
                _logger.LogInformation("Image optimized and saved: {FileName}", fileName);

                
                return Result<string>.Success(relativePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing image: {FileName}", file.FileName);
                return Result<string>.Failure("An error occurred while processing the image.");
            }
            finally
            {
                
                _semaphore.Release();

            }
        }

        public string GetFullUrl(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return string.Empty;


            return $"{_settings.BaseUrl.TrimEnd('/')}/{relativePath.TrimStart('/')}";
        }

        public Result<bool> DeleteImage(string? relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                _logger.LogWarning("Attempted to delete image with empty path.");
                return Result<bool>.Failure("Invalid image path.");
            }

            try
            {
                var fullPath = Path.Combine(_settings.wwwrootPath, relativePath.Replace("/", "\\"));
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    _logger.LogInformation("Image deleted successfully: {Path}", fullPath);
                    return Result<bool>.Success(true);
                }

                _logger.LogWarning("Image not found for deletion: {Path}", fullPath);
                return Result<bool>.Failure("Image not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting image: {Path}", relativePath);
                return Result<bool>.Failure("An error occurred while deleting the image.");
            }
        }
    }
}
