using Agricultural_For_CV_DAL.Entities;
using Agricultural_For_CV_Shared.Enums;

namespace Agricultural_For_CV_DAL.Interfaces
{
    public interface IOrderRepository 
    {
        Task<int> AddOrderAsync(Order order);
        Task<Order?> GetOrderByIdAsync(int id);
        Task<OrderDetail?> GetOrderItemByIdAsync(int id);
        
        Task AddOrderItemAsync(OrderDetail item);

        void CanceledOrder(Order order);

        string GetUserName(int id);
        Task CanceledOrderItemAsync(OrderDetail item);
        Task AcceptedOrderItemAsync(OrderDetail item);

        Task<List<Order>> GetCustomerOrdersAsync(int customerId, OrderStatus status);
        Task<IEnumerable<OrderDetail>> GetOrdersByFarmerIdAsync(int farmerId, ItemStatus itemStatus);

        Task ReadyForPickUpOrderItemAsync(OrderDetail item);
    }
}
