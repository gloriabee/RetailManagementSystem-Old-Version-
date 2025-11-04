using Microsoft.EntityFrameworkCore;
using RetailManagementSystem.DTOs;
using RetailManagementSystem.Interfaces;
using RetailManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagementSystem.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(RetailDbContext context) : base(context)
        {
        }

        public async Task<List<int>> GetCustomerIdsAsync()
        {
            return await _context.Customers
                .Select(c=>c.Id)
                .ToListAsync();
        }

        public async Task<string?> GetCustomerNameByIdAsync(int customerId)
        {
            var customer= await _context.Customers
                .FindAsync(customerId);
            return customer?.Username;
            
        }

        public async Task<int> GetCustomersCountAsync()
        {
            return await _context.Customers.CountAsync();
        }

        public async Task<(List<Customer> customers, int TotalCount)>
            GetPagedCustomersAsync(int pageNumber, int pageSize, string? filter)
        {
            var customers = _context.Customers
                  .Select(c => new Customer
                  {
                      Id = c.Id,
                      Username = c.Username,
                      Email = c.Email,
                      Phone = c.Phone,
                      Address = c.Address,
                      Country = c.Country,
                  });

            if (!string.IsNullOrEmpty(filter))
            {
                customers= customers.Where(c=>
                c.Username.Contains(filter) || 
                c.Id.ToString().Contains(filter));
            }

            var totalCount = await customers.CountAsync();
            var pagedCustomers = await customers
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (pagedCustomers, totalCount);
        }

        public Task<List<TopCustomerDto>> GetTopCustomersAsync(int count)
        {
            var topCustomers= _context.Customers
                .Join(
                    _context.Orders,
                    c=> c.Id,
                    o=> o.CustomerId,
                    (c,o)=> new {c,o }
                )
                .GroupBy(x=> new {x.c.Id, x.c.Username })
                .Select(g=> new TopCustomerDto
                {
                    Name= g.Key.Username,
                    TotalOrders= g.Count(),
                    TotalSpent= g.Sum(x=> x.o.TotalAmount)
                })
                .OrderByDescending(x=> x.TotalOrders)
                .Take(count)
                .ToListAsync();
            return topCustomers;
        }
    }
}
