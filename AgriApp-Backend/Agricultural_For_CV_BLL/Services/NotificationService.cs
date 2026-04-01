using Agricultural_For_CV_DAL.Entities;
using Agricultural_For_CV_DAL.Interfaces;
using Agricultural_For_CV_Shared.Dtos.Notifications;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Results;
using Microsoft.Extensions.Logging;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repo;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(INotificationRepository repo, ILogger<NotificationService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    /// <summary>
    /// Send Notification To Database For User.
    /// </summary>
    public async Task<Result<NotificationDto>> SendNotificationAsync(CreateNotificationDto dto)
    {
        if (dto == null)
            return Result<NotificationDto>.Failure("Invalid notification data.");

        if (string.IsNullOrWhiteSpace(dto.Message))
            return Result<NotificationDto>.Failure("Notification message is required.");

        try
        {
            var notification = new Notification
            {
                UserId = dto.UserId,
                Message = dto.Message.Trim(),
                ProductId = dto.ProductId,
                OrderId = dto.OrderId,
                CreatedAt = dto.CreatedAt,
                IsRead = dto.IsRead
            };


            var added = await _repo.AddAsync(notification);
            _logger.LogInformation("Notification created successfully for UserId={UserId}", dto.UserId);

            return Result<NotificationDto>.Success(MapToDto(added), "Notification sent successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send notification for UserId={UserId}", dto.UserId);
            return Result<NotificationDto>.Failure("Failed to send notification.");
        }
    }

    /// <summary>
    /// Get All Notifications Of User.
    /// </summary>
    public async Task<Result<List<NotificationDto>>> GetUserNotificationsAsync(int userId)
    {
        if (userId <= 0)
            return Result<List<NotificationDto>>.Failure("Invalid user ID.");

        try
        {
            var list = await _repo.GetUserNotificationsAsync(userId);
            if (list == null || !list.Any())
                return Result<List<NotificationDto>>.Failure("No notifications found.");

            var mapped = list.Select(MapToDto).ToList();
            return Result<List<NotificationDto>>.Success(mapped);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching notifications for user {UserId}", userId);
            return Result<List<NotificationDto>>.Failure("Failed to fetch notifications.");
        }
    }

    /// <summary>
    /// Marked as Read.
    /// </summary>
    public async Task<Result<bool>> MarkAsReadAsync(int notificationId)
    {
        if (notificationId <= 0)
            return Result<bool>.Failure("Invalid notification ID.");

        try
        {
            var notification = await _repo.GetByIdAsync(notificationId);
            if (notification == null)
                return Result<bool>.Failure("Notification not found.");

            if (notification.IsRead)
                return Result<bool>.Success(true, "Notification already marked as read.");

            notification.IsRead = true;
            await _repo.UpdateAsync(notification);

            _logger.LogInformation("Notification {NotificationId} marked as read.", notificationId);
            return Result<bool>.Success(true, "Notification marked as read.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to mark notification {NotificationId} as read.", notificationId);
            return Result<bool>.Failure("Failed to update notification status.");
        }
    }


    /// <summary>
    /// Mapping To DTO.
    /// </summary>
    private static NotificationDto MapToDto(Notification n)
    {
        return new NotificationDto
        {
            Id = n.Id,
            UserId = n.UserId,
            Message = n.Message,
            IsRead = n.IsRead,
            CreatedAt = n.CreatedAt,
            ProductId = n.ProductId,
            OrderId = n.OrderId
        };
    }
}
