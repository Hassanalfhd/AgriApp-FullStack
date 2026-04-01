using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Agricultural_For_CV_Shared.Dtos.PaymentsDtos;
using Agricultural_For_CV_Shared.Results;

namespace Agricultural_For_CV_Shared.Interfaces
{
    /// <summary>
    /// Payment Service interface to handle all payment-related operations in the system.
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Creates a new payment.
        /// </summary>
        /// <param name="dto">The payment request data.</param>
        /// <returns>A <see cref="Result{PaymentResponseDto}"/> containing the created payment details.</returns>
        Task<Result<PaymentResponseDto>> CreatePaymentAsync(PaymentRequestDto dto);

        /// <summary>
        /// Retrieves a payment by its unique identifier.
        /// </summary>
        /// <param name="id">The payment ID.</param>
        /// <returns>A <see cref="Result{PaymentResponseDto}"/> containing the payment details if found.</returns>
        Task<Result<PaymentResponseDto>> GetPaymentByIdAsync(int id);

        /// <summary>
        /// Retrieves all payments associated with a specific order.
        /// </summary>
        /// <param name="orderId">The order ID.</param>
        /// <returns>A <see cref="Result{IEnumerable{PaymentResponseDto}}"/> containing the list of payments for the order.</returns>
        Task<Result<IEnumerable<PaymentResponseDto>>> GetPaymentsByOrderIdAsync(int orderId);

        /// <summary>
        /// Updates the status of a payment.
        /// </summary>
        /// <param name="id">The payment ID.</param>
        /// <param name="newStatus">The new status for the payment (e.g., Paid, Failed, Pending).</param>
        /// <param name="transactionId">Optional transaction ID for external payment gateways.</param>
        /// <returns>A <see cref="Result{bool}"/> indicating whether the status update was successful.</returns>
        Task<Result<bool>> UpdatePaymentStatusAsync(int id, string newStatus, string? transactionId = null);
    }
}
