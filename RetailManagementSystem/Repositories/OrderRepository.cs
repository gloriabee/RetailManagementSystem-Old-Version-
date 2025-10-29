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

        public List<OrderDetailsDto> GetAllOrders()
        {
           var ordersWithCustomerInfo= _context.Orders
                .Join(
                    _context.Customers,
                    order => order.CustomerId,
                    customer => customer.Id,
                    (order, customer) => new OrderDetailsDto
                    {
                        OrderId = order.Id,
                        OrderDate = order.OrderDate,
                        Subtotal = order.TotalAmount,
                        CustomerName= customer.Username,
                        Email= customer.Email,
                        Phone= customer.Phone,
                        Address = customer.Address,
                        Country = customer.Country
                    }
                ).ToList();
            return ordersWithCustomerInfo;
        }

        public List<OrderProductDto> GetOrderProducts(int orderId)
        {
            var products = _context.OrderDetails
                .Where(od => od.OrderId == orderId)
                .Join(
                    _context.Products,
                    od => od.ProductId,
                    p => p.Id,
                    (od, p) => new OrderProductDto
                    {
                        Id = p.Id,
                        ProductName = p.ProductName,
                        Quantity = od.Quantity,
                        UnitPrice = od.UnitPrice
                    }
                ).ToList();
            return products;
        }
    }
}
