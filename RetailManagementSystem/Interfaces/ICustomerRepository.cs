using RetailManagementSystem.DTOs;
using RetailManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagementSystem.Interfaces
{
    public interface ICustomerRepository:IGenericRepository<Customer> 
    {
        Task<int> GetCustomersCountAsync();
        Task<List<int>> GetCustomerIdsAsync();
        Task<string?> GetCustomerNameByIdAsync(int customerId);
        Task<List<TopCustomerDto>> GetTopCustomersAsync(int count);
        Task<(List<Customer> customers,int TotalCount)> 
            GetPagedCustomersAsync
            (int pageNumber, int pageSize, string? filter);
    }
}
