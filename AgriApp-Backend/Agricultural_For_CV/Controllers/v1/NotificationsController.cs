using Agricultural_For_CV_BLL.Services;
using Agricultural_For_CV_Shared.Dtos.Notifications;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
    /// جلب جميع الإشعارات الخاصة بمستخدم معين.
    /// </summary>
    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetUserNotifications(int userId)
    {
        var result = await _service.GetUserNotificationsAsync(userId);

        if (!result.IsSuccess)
            return NotFound(new { message = result.Message });

        return Ok(result.Data);
    }

    /// <summary>
    /// إرسال إشعار جديد.
    /// </summary>
    [HttpPost("send")]
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
    /// تعليم الإشعار كمقروء.
    /// </summary>
    [HttpPost("{id}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var result = await _service.MarkAsReadAsync(id);

        if (!result.IsSuccess)
            return NotFound(new { message = result.Message });

        return Ok(new { message = result.Message });
    }
}
