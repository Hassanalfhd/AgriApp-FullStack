using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Agricultural_For_CV_DAL.Interfaces;
using Agricultural_For_CV_Shared.Interfaces;

public class ImageCleanupService : BackgroundService
{
    private readonly ILogger<ImageCleanupService> _logger;
    private readonly IProductRepository _productRepo;
    private readonly IImageService _imageService;
    private readonly TimeSpan _interval = TimeSpan.FromDays(1); // كل يوم

    public ImageCleanupService(
        ILogger<ImageCleanupService> logger,
        IProductRepository productRepo,
        IImageService imageService)
    {
        _logger = logger;
        _productRepo = productRepo;
        _imageService = imageService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await CleanDeletedProductImagesAsync();
            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task CleanDeletedProductImagesAsync()
    {
        _logger.LogInformation("🧹 Starting image cleanup task...");

        var deletedProducts = await _productRepo.GetDeletedProductsWithImagesAsync();
        foreach (var product in deletedProducts)
        {
            foreach (var img in product.ProductsImages)
            {
                try
                {
                    _imageService.DeleteImage(img.ImagePath);
                    _logger.LogInformation("Deleted image: {Path}", img.ImagePath);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to delete image: {Path}", img.ImagePath);
                }
            }
        }

        _logger.LogInformation("✅ Image cleanup completed.");
    }
}
