using RetailManagementSystem.DTOs;
using RetailManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagementSystem.Interfaces
{
    public interface IOrderRepository
    {
        void Add(Order order);
        void AddDetails(OrderDetail details);
        List<OrderDetailsDto> GetAllOrders();

    }
}
