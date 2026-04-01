using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_DAL;
using Agricultural_For_CV_DAL.Entities;
using Agricultural_For_CV_DAL.Interfaces;
using Agricultural_For_CV_DAL.Repositories;
using Agricultural_For_CV_Shared.Dtos.OrdersDtos;
using Agricultural_For_CV_Shared.Enums;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Results;
using Microsoft.Extensions.Logging;

namespace Agricultural_For_CV_BLL.Services
{
    public class FarmerService : IFarmerService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository; // ضروري لجلب الأسعار والمخزون
        private readonly ILogger<OrderService> _logger;
        private readonly INotificationService _notificationService;
        private readonly AppDbContext _context;

        public FarmerService(
        IOrderRepository orderRepository,
        AppDbContext context,
        IProductRepository productRepository,
        ILogger<OrderService> logger,
        INotificationService notificationService)
        {
            _context = context;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _logger = logger;
            _notificationService = notificationService;
        }
        public async Task<Result<List<FarmerOrderDto>>> GetFarmerDashboardItemsAsync(int farmerId, ItemStatus itemStatus)
        {
            var items = await _orderRepository.GetOrdersByFarmerIdAsync(farmerId, itemStatus);

            if (items == null || !items.Any())
                return Result<List<FarmerOrderDto>>.Failure("No orders found.");

            return Result<List<FarmerOrderDto>>.Success(items.Select(MapToResponse).ToList());

        }


        public async Task<Result<int>> AcceptedOrderItemAsync(int id)
        {
            var item = await _orderRepository.GetOrderItemByIdAsync(id);


            if (item == null )
                return Result<int>.Failure("No orders found.");

            if(item.Status != ItemStatus.Pending)
                return Result<int>.Failure("Can not Accepted this order item.");

            item.Status = ItemStatus.Accepted;

            await _orderRepository.AcceptedOrderItemAsync(item);

            return Result<int>.Success(item.Id);

        }

        public async Task<Result<int>> ReadyForPickUpOrderItemAsync(int id)
        {
            var item = await _orderRepository.GetOrderItemByIdAsync(id);


            if (item == null)
                return Result<int>.Failure("No orders found.");

            if (item.Status == ItemStatus.Cancelled || item.Status == ItemStatus.Shipped)
                return Result<int>.Failure("Can not be ready for pickup of  this order item.");



            item.Status = ItemStatus.ReadyForPickup;

            await _orderRepository.ReadyForPickUpOrderItemAsync(item);

            return Result<int>.Success(item.Id);

        }


        public async Task<Result<int>> CanceledOrderItemAsync(int orderItemId)
        {
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                var item = await _orderRepository.GetOrderItemByIdAsync(orderItemId);


                if (item == null)
                {
                    _logger.LogWarning("order item not found: {id}", orderItemId);
                    return Result<int>.Failure("order not found.");
                }



                var product = await _productRepository.GetAsync(item.ProductId);
                product.QuantityInStock += item.Quantity;
                item.Status = ItemStatus.Cancelled;
                await _productRepository.UpdateAsync(product);



                //await SendOrderNotification(item.FarmerId, orderItemId, "Canceled", item.Status);


                await _orderRepository.CanceledOrderItemAsync(item);

                await transaction.CommitAsync();

                _logger.LogInformation("Order item with id = {OrderId} is canceled successfully processed.", orderItemId);
                return Result<int>.Success(item.Id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Order item with id = {id} is not canceled", orderItemId);
                return Result<int>.Failure($"Order item with id = {orderItemId} is not canceled");
            }
        }



        //Helper
        private FarmerOrderDto MapToResponse(OrderDetail item)
        {

            return new FarmerOrderDto
            {
                OrderItemId = item.Id,
                ProductName = item.Product.Name,
                Quantity = item.Quantity,
                CustomerName =  _orderRepository.GetUserName(item.Order.CustomerId), // مثال
                Status = item.Status.ToString(),
                UnitPrice = item.UnitPrice,
                Total  = item.Total,
                OrderDate = item.Order.CreatedAt
            };
        }
    }
}