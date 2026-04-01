using System.Collections.Generic;
using System.Threading.Tasks;
using Agricultural_For_CV_Shared.Dtos.Notifications;
using Agricultural_For_CV_Shared.Results;

namespace Agricultural_For_CV_Shared.Interfaces
{
    /// <summary>
    /// Defines operations for managing user notifications, including sending, retrieving, 
    /// and marking notifications as read.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Sends a new notification to a user.
        /// </summary>
        /// <param name="dto">The notification data (title, message, and receiver ID).</param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing the created <see cref="NotificationDto"/> 
        /// or an error message if sending fails.
        /// </returns>
        Task<Result<NotificationDto>> SendNotificationAsync(CreateNotificationDto dto);

        /// <summary>
        /// Retrieves all notifications for a specific user.
        /// </summary>
        /// <param name="userId">The user ID to fetch notifications for.</param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing a list of <see cref="NotificationDto"/> 
        /// or an error message if none are found.
        /// </returns>
        Task<Result<List<NotificationDto>>> GetUserNotificationsAsync(int userId);

        /// <summary>
        /// Marks a specific notification as read.
        /// </summary>
        /// <param name="notificationId">The ID of the notification to mark as read.</param>
        /// <returns>
        /// A <see cref="Result{T}"/> indicating whether the operation succeeded.
        /// </returns>
        Task<Result<bool>> MarkAsReadAsync(int notificationId);
    }
}
