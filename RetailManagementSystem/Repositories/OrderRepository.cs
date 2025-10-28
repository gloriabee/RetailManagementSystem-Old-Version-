using RetailManagementSystem.DTOs;
using RetailManagementSystem.Interfaces;
using RetailManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagementSystem
{
    public class OrderRepository : IOrderRepository
    {
        private readonly RetailDbContext _context;
        public OrderRepository(RetailDbContext context)
        {
            _context = context;
        }

        public void Add(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void AddDetails(OrderDetail details)
        {
            _context.OrderDetails.Add(details);
            _context.SaveChanges();
        }

        public List<OrderCustomerDto> GetAllOrders()
        {
           var ordersWithCustomerInfo= _context.Orders
                .Join(
                    _context.Customers,
                    order => order.CustomerId,
                    customer => customer.Id,
                    (order, customer) => new OrderCustomerDto
                    {
                        Id = order.Id,
                        OrderDate = order.OrderDate,
                        TotalAmount = order.TotalAmount,
                        CustomerName= customer.Username,
                        Address= customer.Address
                    }
                ).ToList();
            return ordersWithCustomerInfo;
        }

       
    }
}
