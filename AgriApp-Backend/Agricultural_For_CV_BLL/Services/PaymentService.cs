using Agricultural_For_CV_DAL.Entities;
using Agricultural_For_CV_DAL.Interfaces;
using Agricultural_For_CV_Shared.Dtos.PaymentsDtos;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agricultural_For_CV_BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IPaymentRepository paymentRepo, IOrderRepository orderRepo, ILogger<PaymentService> logger)
        {
            _paymentRepo = paymentRepo;
            _orderRepo = orderRepo;
            _logger = logger;
        }

        public async Task<Result<PaymentResponseDto>> CreatePaymentAsync(PaymentRequestDto dto)
        {
            var order = await _orderRepo.GetOrderByIdAsync(dto.OrderId);
            if (order == null)
            {
                _logger.LogWarning("Attempted to pay for non-existent order {OrderId}", dto.OrderId);
                return Result<PaymentResponseDto>.Failure($"Order with ID {dto.OrderId} not found.");
            }

            if (dto.Amount != order.TotalPrice)
            {
                _logger.LogWarning("Payment amount {Amount} does not match order total {TotalPrice} for Order {OrderId}", dto.Amount, order.TotalPrice, dto.OrderId);
                return Result<PaymentResponseDto>.Failure("Payment amount must match the order total.");
            }

            var payment = new Payment
            {
                OrderId = dto.OrderId,
                Amount = dto.Amount,
                PaymentMethod = dto.PaymentMethod,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                var result = await _paymentRepo.AddPaymentAsync(payment);
                _logger.LogInformation("Payment {PaymentId} created for Order {OrderId}", result.Id, result.OrderId);

                return Result<PaymentResponseDto>.Success(MapToDto(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create payment for Order {OrderId}", dto.OrderId);
                return Result<PaymentResponseDto>.Failure("Failed to create payment.");
            }
        }

        public async Task<Result<PaymentResponseDto>> GetPaymentByIdAsync(int id)
        {
            try
            {
                var payment = await _paymentRepo.GetPaymentByIdAsync(id);
                if (payment == null)
                {
                    _logger.LogWarning("Payment with id {PaymentId} not found.", id);
                    return Result<PaymentResponseDto>.Failure("Payment not found.");
                }

                _logger.LogInformation("Payment with id {PaymentId} retrieved successfully.", id);
                return Result<PaymentResponseDto>.Success(MapToDto(payment));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve payment with id {PaymentId}", id);
                return Result<PaymentResponseDto>.Failure("Failed to retrieve payment.");
            }
        }

        public async Task<Result<IEnumerable<PaymentResponseDto>>> GetPaymentsByOrderIdAsync(int orderId)
        {
            try
            {
                var payments = await _paymentRepo.GetPaymentsByOrderIdAsync(orderId);
                if (payments == null || !payments.Any())
                {
                    _logger.LogInformation("No payments found for Order {OrderId}", orderId);
                    return Result<IEnumerable<PaymentResponseDto>>.Failure("No payments found for this order.");
                }

                _logger.LogInformation("Payments for Order {OrderId} retrieved successfully.", orderId);
                return Result<IEnumerable<PaymentResponseDto>>.Success(payments.Select(MapToDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve payments for Order {OrderId}", orderId);
                return Result<IEnumerable<PaymentResponseDto>>.Failure("Failed to retrieve payments.");
            }
        }

        public async Task<Result<bool>> UpdatePaymentStatusAsync(int id, string newStatus, string? transactionId = null)
        {
            try
            {
                var payment = await _paymentRepo.GetPaymentByIdAsync(id);
                if (payment == null)
                {
                    _logger.LogWarning("Attempted to update status for non-existent payment {PaymentId}", id);
                    return Result<bool>.Failure("Payment not found.");
                }

                payment.Status = newStatus;
                payment.TransactionId = transactionId;
                payment.PaymentDate = DateTime.UtcNow;

                await _paymentRepo.UpdatePaymentAsync(payment);
                _logger.LogInformation("Payment {PaymentId} status updated to {Status}", id, newStatus);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update payment status for Payment {PaymentId}", id);
                return Result<bool>.Failure("Failed to update payment status.");
            }
        }

        private static PaymentResponseDto MapToDto(Payment payment)
        {
            return new PaymentResponseDto
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                Status = payment.Status,
                TransactionId = payment.TransactionId,
                PaymentDate = payment.PaymentDate
            };
        }
    }
}
