using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_DAL.Entities;
using Agricultural_For_CV_DAL.Interfaces;
using Agricultural_For_CV_Shared.Dtos.OrdersDtos;
using Agricultural_For_CV_Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Agricultural_For_CV_DAL.Repositories
{
    public class OrderRepository : IOrderRepository {
        private readonly AppDbContext _context;
        public OrderRepository(AppDbContext context) => _context = context;

        public async Task<int> AddOrderAsync(Order order) {
              await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order.Id;
        } 

        public async Task AddOrderItemAsync(OrderDetail item) => await _context.OrderDetails.AddAsync(item);

        public async Task<OrderDetail?> GetOrderItemByIdAsync(int id)
        {
            return await _context.OrderDetails.AsNoTracking().Where(od=>od.Id == id).FirstOrDefaultAsync();
        }


        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            var query = _context.Orders
                .AsNoTracking()
                .Where(o => o.Id == id)
                .Include(o => o.User)
                .Include(o => o.OrderDetails);

            return await query.FirstOrDefaultAsync();
        }



        public  void CanceledOrder(Order order)
        {
             _context.Update(order);

        }

        public async Task AcceptedOrderItemAsync(OrderDetail item)
        {
            _context.OrderDetails.Update(item);
            await _context.SaveChangesAsync();
        }


        public async Task ReadyForPickUpOrderItemAsync(OrderDetail item)
        {
            _context.OrderDetails.Update(item);
            await _context.SaveChangesAsync();
        }



        public async Task CanceledOrderItemAsync(OrderDetail item)
        {
            _context.OrderDetails.Update(item);
            await _context.SaveChangesAsync();

        }

        public string GetUserName(int id) => _context.Users.Find(id).fullName ?? "unknown";
        
        public async Task<IEnumerable<OrderDetail>> GetOrdersByFarmerIdAsync(int farmerId, ItemStatus itemStatus)
        {
            return await _context.OrderDetails
            .Include(oi => oi.Order)
            .Include(oi => oi.User)
            .Include(oi => oi.Product)
            .Where(oi => oi.Product.CreatedBy== farmerId && oi.Status == itemStatus) 
            .AsNoTracking()
            .OrderByDescending(o=>o.Order.CreatedAt)
            .ToListAsync();


        }




        public async Task<List<Order>> GetCustomerOrdersAsync(int customerId, OrderStatus status) {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .Include(o=> o.User)
                .Where(o => o.CustomerId == customerId && o.Status == status)
                .OrderByDescending(o=>o.CreatedAt)
                .ToListAsync();
        }



    }
}
