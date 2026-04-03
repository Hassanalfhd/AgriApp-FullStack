using Agricultural_For_CV_BLL.Services;
using Agricultural_For_CV_Shared.Dtos.Notifications;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;




namespace Agricultural_For_CV.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/notifications")]
    [ApiVersion("1.0")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationsController(INotificationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all notifications for a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of notifications for the user.</returns>
        /// <response code="200">Returns the list of notifications.</response>
        /// <response code="404">If no notifications are found for the user.</response>
        [HttpGet("{userId:int}")]
        [ProducesResponseType(typeof(IEnumerable<NotificationDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserNotifications(int userId)
        {
            var result = await _service.GetUserNotificationsAsync(userId);

            if (!result.IsSuccess)
                return NotFound(new { message = result.Message });

            return Ok(result.Data);
        }

        /// <summary>
        /// Sends a new notification to a user.
        /// </summary>
        /// <param name="dto">The notification data to be sent.</param>
        /// <returns>The created notification details.</returns>
        /// <response code="200">Returns the created notification.</response>
        /// <response code="400">If the request data is invalid or sending fails.</response>
        [HttpPost("send")]
        [ProducesResponseType(typeof(NotificationDto), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SendNotification([FromBody] CreateNotificationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.SendNotificationAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(result.Data);
        }

        /// <summary>
        /// Marks a notification as read.
        /// </summary>
        /// <param name="id">The unique identifier of the notification.</param>
        /// <returns>A message indicating the operation result.</returns>
        /// <response code="200">If the notification was successfully marked as read.</response>
        /// <response code="404">If the notification is not found.</response>
        [HttpPost("{id}/read")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var result = await _service.MarkAsReadAsync(id);

            if (!result.IsSuccess)
                return NotFound(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}