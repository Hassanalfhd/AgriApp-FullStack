using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_Shared.Dtos.OrdersDtos;
using Agricultural_For_CV_Shared.Enums;
using Agricultural_For_CV_Shared.Results;

namespace Agricultural_For_CV_Shared.Interfaces
{
    public interface IOrderService
    {
        Task<Result<OrderResponseDto>> CreateOrderAsync(OrderRequestDto dto);

        Task<Result<OrderResponseDto>> GetOrderByIdAsync(int id);

        Task<Result<IEnumerable<OrderResponseDto>>> GetUserOrdersAsync(int userId, OrderStatus status);

        Task<Result<int>> CanceledOrderAsync(int id);
        Task<Result<OrderDetailDto>> CanceledOrderItemAsync(int orderItemId);


    }
}
