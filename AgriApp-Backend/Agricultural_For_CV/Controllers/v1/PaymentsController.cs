using Agricultural_For_CV_Shared.Dtos.PaymentsDtos;
using Agricultural_For_CV_Shared.Enums;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Agricultural_For_CV_API.Controllers
{
    [Route("api/v{version:ApiVersion}/payments")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new payment.
        /// </summary>
        /// <param name="dto">The payment request data.</param>
        /// <returns>The details of the created payment.</returns>
        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Result<PaymentResponseDto>.Failure(
                    "Invalid payment data."
                ));
            }

            var result = await _paymentService.CreatePaymentAsync(dto);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to create payment: {Error}", result.Error);
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a payment by its ID.
        /// </summary>
        /// <param name="id">The payment ID.</param>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var result = await _paymentService.GetPaymentByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                return NotFound(Result<PaymentResponseDto>.Failure(result.Error ?? $"Payment with ID {id} not found."));
            }

            return Ok(result);
        }

        /// <summary>
        /// Retrieves all payments associated with a specific order.
        /// </summary>
        /// <param name="orderId">The order ID.</param>
        [HttpGet("order/{orderId:int}")]
        public async Task<IActionResult> GetPaymentsByOrder(int orderId)
        {
            var result = await _paymentService.GetPaymentsByOrderIdAsync(orderId);

            if (!result.IsSuccess)
                return NotFound(Result<PaymentResponseDto>.Failure(result.Error ?? $"Payments for order ID {orderId} not found."));

            return Ok(result);
        }

        /// <summary>
        /// Updates the status of an existing payment.
        /// </summary>
        /// <param name="id">The payment ID.</param>
        /// <param name="status">The new payment status (e.g., Paid, Pending, Failed).</param>
        /// <param name="transactionId">Optional transaction ID for external payment gateways.</param>
        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status, [FromQuery] string? transactionId = null)
        {
            var validStatuses = new[] { "Paid", "Pending", "Failed" };

            if (!validStatuses.Contains(status))
            {
                return BadRequest(Result<bool>.Failure($"Invalid status value. Allowed values: {string.Join(", ", validStatuses)}"));
            }

            var result = await _paymentService.UpdatePaymentStatusAsync(id, status, transactionId);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(Result<bool>.Success(true, "Payment status updated successfully."));
        }
    }
}
