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
        private OrderRepository(RetailDbContext context)
        {
            _context = context;
        }

        public static OrderRepository Create()
        {
            var context= new RetailDbContext();
            return new OrderRepository(context);
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

        public List<Order> GetOrderByCreatedBy(string createdBy)
        {
           var orders= _context.Orders
                .Where(o => o.CreatedBy == createdBy)
                .ToList();

            return orders;
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

        public List<OrderDetailsDto> GetPagedOrders(int pageNumber, int pageSize, string? filter, out int totalCount)
        {
            var orders = _context.Orders
                 .Join(_context.Customers,
                 o => o.CustomerId,
                 c => c.Id,
                 (o, c) => new OrderDetailsDto
                 {
                     OrderId = o.Id,
                     OrderDate = o.OrderDate,
                     Subtotal = o.TotalAmount,
                     CustomerName = c.Username,
                     Email = c.Email,
                     Phone = c.Phone,
                     Address = c.Address,
                     Country = c.Country
                 });

            if (!string.IsNullOrEmpty(filter))
            {
                orders= orders.Where(o => o.CustomerName.Contains(filter) || o.OrderId.ToString().Contains(filter));
            }

              totalCount= orders.Count();

            var pagedOrders= orders.
                OrderBy(o=>o.OrderId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return pagedOrders;


        }
    }
}
