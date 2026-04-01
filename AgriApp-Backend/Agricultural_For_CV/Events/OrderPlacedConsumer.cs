using Agricultural_For_CV_Shared.Dtos.Notifications;
using Agricultural_For_CV_Shared.EventsType;
using Agricultural_For_CV_Shared.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Agricultural_For_CV.Events
{
    // الـ Consumer هنا يعمل كـ "مُستقبِل" (Entry Point) في طبقة الـ Infrastructure
    public class OrderPlacedConsumer : IConsumer<OrderPlacedEvent>
    {
        private readonly INotificationService _notificationService; // تمثل الـ BLL
        private readonly ILogger<OrderPlacedConsumer> _logger;

        public OrderPlacedConsumer(INotificationService notificationService, ILogger<OrderPlacedConsumer> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
        {
            _logger.LogInformation("Processing OrderPlacedEvent for OrderId: {OrderId}", context.Message.OrderId);

            try
            {
                var eventData = context.Message;

                // 1. منطق العمل (Business Logic): 
                // نمرر الـ Event للخدمة وهي تقرر كيف تصيغ الرسالة وتجهز الـ DTO
                // هذا يضمن أن الـ Consumer لا يحتوي على Logic
                var notificationDto = new CreateNotificationDto
                {
                    UserId = eventData.UserId,
                    OrderId = eventData.OrderId,
                    Message = $"Order #{eventData.OrderId} has been placed successfully!",
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false
                };

                // 2. الوصول للبيانات (Data Access):
                // استدعاء الخدمة لحفظ الإشعار في قاعدة البيانات
                // "Under the hood": بما أننا فعلنا الـ Entity Framework Outbox/Inbox في الإعدادات،
                // فإن MassTransit ستفتح Transaction تلقائياً هنا لضمان عدم تكرار المعالجة (Idempotency)
                await _notificationService.SendNotificationAsync(notificationDto);

                _logger.LogInformation("Notification sent successfully for OrderId: {OrderId}", eventData.OrderId);
            }
            catch (Exception ex)
            {
                // الـ Senior دائماً يهتم بتسجيل الأخطاء (Logging)
                _logger.LogError(ex, "Error occurred while consuming OrderPlacedEvent for OrderId: {OrderId}", context.Message.OrderId);

                // إعادة رمي الخطأ (Rethrow) ضروري جداً لكي يفهم الـ Broker أن العملية فشلت
                // ويقوم بإعادة المحاولة (Retry Policy) أو نقلها لـ Dead Letter Queue
                throw;
            }
        }
    }
}