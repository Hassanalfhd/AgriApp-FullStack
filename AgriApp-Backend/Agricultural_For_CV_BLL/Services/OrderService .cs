using Agricultural_For_CV_DAL;
using Agricultural_For_CV_DAL.Entities;
using Agricultural_For_CV_DAL.Interfaces;
using Agricultural_For_CV_Shared.Dtos.Notifications;
using Agricultural_For_CV_Shared.Dtos.OrdersDtos;
using Agricultural_For_CV_Shared.Dtos.Products;
using Agricultural_For_CV_Shared.Enums;
using Agricultural_For_CV_Shared.EventsType;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Results;
using MassTransit;
using Microsoft.Extensions.Logging;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository; // ضروري لجلب الأسعار والمخزون
    private readonly ILogger<OrderService> _logger;
    private readonly INotificationService _notificationService;
    //private readonly IPublishEndpoint _publishEndpoint;

    private readonly AppDbContext _context;
    public OrderService(
        IOrderRepository orderRepository,
        AppDbContext context,
        IProductRepository productRepository,
        ILogger<OrderService> logger,
        INotificationService notificationService)
    {
        _context = context;
        //_publishEndpoint = publishEndpoint;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _logger = logger;
        _notificationService = notificationService;
    }


    public async Task<Result<OrderResponseDto>> CreateOrderAsync(OrderRequestDto dto)
    {
        // 1. التحقق الأولي (Fail Fast)
        if (dto?.Items == null || !dto.Items.Any())
            return Result<OrderResponseDto>.Failure("Empty cart.");

        // 2. بدء معاملة قاعدة البيانات (Database Transaction)
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var order = new Order
            {
                CustomerId = dto.UserId,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                TotalPrice = 0
            };

            decimal calculatedTotal = 0;

            foreach (var item in dto.Items)
            {
                // جلب المنتج مع قفل السطر (Locking) لمنع الـ Race Condition في المخزون
                var product = await _productRepository.GetAsync(item.ProductId);

                if (product == null)
                    throw new Exception($"Product {item.ProductId} not found.");

                if (product.QuantityInStock < item.Quantity)
                    throw new Exception($"Insufficient stock for {product.Name}. Requested: {item.Quantity}, Available: {product.QuantityInStock}");

                // حساب السعر وتجهيز الصنف
                var detail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    FarmerId = product.CreatedBy,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                    Status = ItemStatus.Pending,
                    Total = item.Quantity * product.Price
                };

                product.QuantityInStock -= item.Quantity;
                calculatedTotal += (detail.Quantity * detail.UnitPrice);

                order.OrderDetails.Add(detail);


                await _productRepository.UpdateAsync(product);
            }

            order.TotalPrice = calculatedTotal;

            // 3. حفظ الطلب في قاعدة البيانات
            int savedOrderId = await _orderRepository.AddOrderAsync(order);

            var savedOrder = await _orderRepository.GetOrderByIdAsync(savedOrderId);

            
            await _context.SaveChangesAsync();

            //await _publishEndpoint.Publish(new OrderPlacedEvent
            //{
            //    OrderId = savedOrderId,
            //    UserId = savedOrder.CustomerId,
            //    IsRead = false,
            //    CreatedAt = DateTime.Now,
            //});

            // 4. تثبيت العملية
            await transaction.CommitAsync();



            _logger.LogInformation("Order {OrderId} successfully processed.", savedOrder.Id);

 
            return Result<OrderResponseDto>.Success(MapToResponse(savedOrder));
        }
        catch (Exception ex)
        {
            // التراجع عن كل شيء في حال حدوث أي خطأ (حتى لو خطأ في الشبكة)
            await transaction.RollbackAsync();
            _logger.LogCritical(ex, "Transaction failed for User {UserId}. All changes rolled back.", dto.UserId);
            return Result<OrderResponseDto>.Failure(ex.Message);
        }
    }

    public async Task<Result<OrderResponseDto>> GetOrderByIdAsync(int id)
    {
        var order = await _orderRepository.GetOrderByIdAsync(id);
        if (order == null) return Result<OrderResponseDto>.Failure("Order not found.");

        return Result<OrderResponseDto>.Success(MapToResponse(order));
    }



    public async Task<Result<OrderDetailDto>> CanceledOrderItemAsync(int orderItemId)
    {
        try
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var item = await _orderRepository.GetOrderItemByIdAsync(orderItemId    );


            if (item == null)
            {
                _logger.LogWarning("order item not found: {id}", orderItemId);
                return Result<OrderDetailDto>.Failure("order not found.");
            }



            var product = await _productRepository.GetAsync(item.ProductId);
            product.QuantityInStock += item.Quantity;
            item.Status = ItemStatus.Cancelled;
            await _productRepository.UpdateAsync(product);



            //await SendOrderNotification(item.FarmerId, orderItemId, "Canceled", item.Status);


            await _orderRepository.CanceledOrderItemAsync(item);

            await transaction.CommitAsync();

            _logger.LogInformation("Order item with id = {OrderId} is canceled successfully processed.", orderItemId);
            return Result<OrderDetailDto>.Success(MapToDetailResponse(item));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Order item with id = {id} is not canceled", orderItemId);
            return Result<OrderDetailDto>.Failure($"Order item with id = {orderItemId} is not canceled");
        }
    }



    public async Task<Result<int>> CanceledOrderAsync(int id)
    {
        try
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var order = await _orderRepository.GetOrderByIdAsync(id);

            
            if (order == null)
            {
                _logger.LogWarning("order not found: {id}", id);
                return Result<int>.Failure("order not found.");
            }



            if (order.Status != OrderStatus.Pending)
            {
                _logger.LogWarning("order can not cancelled because its status not pending: {id}", id);
                return Result<int>.Failure("order can not cancelled because its status not pending.");
            }


            order.Status = OrderStatus.Cancelled;


            foreach (var item in order.OrderDetails)
            {
                var product = await _productRepository.GetAsync(item.ProductId);
                product.QuantityInStock += item.Quantity;
                item.Status = ItemStatus.Cancelled;

                await _productRepository.UpdateAsync(product);

            }

            //await SendOrderNotification(order.CustomerId, order.Id, "Canceled", order.Status);


             _orderRepository.CanceledOrder(order);
            
            _context.SaveChanges();
            
            await transaction.CommitAsync();

            _logger.LogInformation("Order with id = {OrderId} is canceled successfully processed.", id);
            return Result<int>.Success(order.Id);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Order with id = {id} is not canceled", id);
            return Result<int>.Failure($"Order with id = {id} is not canceled");
        }
    }



    public async Task<Result<IEnumerable<OrderResponseDto>>> GetUserOrdersAsync(int userId, OrderStatus status)
    {

        var orders = await _orderRepository.GetCustomerOrdersAsync(userId, status);
     
        if (orders == null || !orders.Any())
            return Result<IEnumerable<OrderResponseDto>>.Failure("No orders found.");

        return Result<IEnumerable<OrderResponseDto>>.Success(orders.Select(MapToResponse));
    }

  

    // --- Helper Methods (Best Practice: DRY) ---

    private OrderResponseDto MapToResponse(Order order)
    {

        return new OrderResponseDto
        {
            OrderId = order.Id,
            Status = order.Status.ToString(),
            TotalAmount = order.TotalPrice,
            CreatedAt = order.CreatedAt,
            Items = order.OrderDetails.Select(MapToDetailResponse).ToList()
        };
    }


    private OrderDetailDto MapToDetailResponse(OrderDetail od)
    {
        return new OrderDetailDto
        {
            OrderItemId = od.Id,
            OrderId= od.OrderId,
            ProductId = od.ProductId,
            Quantity = od.Quantity,
            FarmerName = _orderRepository.GetUserName(od.FarmerId),
            ProductName = _productRepository.GetProductName(od.ProductId),
            FarmerId = od.FarmerId,
            Total = od.Total,
            UnitPrice = od.UnitPrice,
            Status = od.Status.ToString()
        };
    }

    private async Task SendOrderNotification(int userId, int orderId, string action, OrderStatus status)
    {
        try
        {
            await _notificationService.SendNotificationAsync(new CreateNotificationDto
            {
                UserId = userId,
                Message = $"Your order #{orderId} has been {action}. Current Status: {status}",
                OrderId = orderId
            });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Notification failed for Order {OrderId}", orderId);
        }
    }


}