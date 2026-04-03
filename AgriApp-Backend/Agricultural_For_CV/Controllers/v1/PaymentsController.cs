using Agricultural_For_CV_Shared.Dtos.PaymentsDtos;
using Agricultural_For_CV_Shared.Enums;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Agricultural_For_CV.Controllers.v1
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
        /// <response code="200">Payment created successfully.</response>
        /// <response code="400">Invalid payment data or creation failed.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Result<PaymentResponseDto>), 200)]
        [ProducesResponseType(typeof(Result<PaymentResponseDto>), 400)]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Result<PaymentResponseDto>.Failure("Invalid payment data."));
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
        /// <param name="id">The unique payment ID.</param>
        /// <returns>The payment details if found.</returns>
        /// <response code="200">Payment found and returned successfully.</response>
        /// <response code="404">Payment with the specified ID not found.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Result<PaymentResponseDto>), 200)]
        [ProducesResponseType(typeof(Result<PaymentResponseDto>), 404)]
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
        /// <param name="orderId">The order ID to filter payments.</param>
        /// <returns>A list of payments related to the specified order.</returns>
        /// <response code="200">Payments retrieved successfully.</response>
        /// <response code="404">No payments found for the specified order.</response>
        [HttpGet("order/{orderId:int}")]
        [ProducesResponseType(typeof(IEnumerable<Result<PaymentResponseDto>>), 200)]
        [ProducesResponseType(404)]
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
        /// <param name="id">The unique payment ID.</param>
        /// <param name="status">The new payment status. Allowed values: Paid, Pending, Failed.</param>
        /// <param name="transactionId">Optional transaction ID for external payment gateways.</param>
        /// <response code="200">Payment status updated successfully.</response>
        /// <response code="400">Invalid status value provided.</response>
        /// <response code="404">Payment with the specified ID not found.</response>
        [HttpPut("{id:int}/status")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        [ProducesResponseType(typeof(Result<bool>), 400)]
        [ProducesResponseType(typeof(Result<bool>), 404)]
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