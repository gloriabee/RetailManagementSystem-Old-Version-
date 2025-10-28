using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetailManagementSystem.DTOs;
using RetailManagementSystem.Models;

namespace RetailManagementSystem.Services
{
    public class CustomerService
    {
       private readonly RetailDbContext _context;

        public CustomerService()
        {
            _context = new RetailDbContext();
        }

        public List<Customer> GetPagedCustomers(string filter, int pageNumber, int pageSize)
        {
           

            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(c => c.Username.ToLower().Contains(filter.ToLower()) ||
                                         c.Id.ToString().Contains(filter));
            }

             return query
                .OrderBy(c => c.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c=> new Customer
                {
                    Id = c.Id,
                    Username = c.Username,
                    Email = c.Email,
                    Phone = c.Phone,
                    Address = c.Address,
                    Country = c.Country
                }).ToList();
       

            
        }

        public int GetTotalCount(string filter)
        {
        

            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {

                query = query.Where(c => c.Username.ToLower().Contains(filter.ToLower()) ||
                                         c.Id.ToString().Contains(filter));

            }

            return query.Count();
        }




        // Add a new customer
        public void Add(Customer customer)
        {
   
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        // Update user
        public void Update(Customer customer)
        {    var customerToUpdate = _context.Customers.Find(customer.Id);
            if (customerToUpdate != null)
            {
               customerToUpdate.Username= customer.Username;
                customerToUpdate.Email= customer.Email;
                customerToUpdate.Phone= customer.Phone;
                customerToUpdate.Address= customer.Address;
                customerToUpdate.Country= customer.Country;
                customerToUpdate.UpdatedAt= DateTime.UtcNow;
                _context.SaveChanges();
            }
           
        }

        // Delete Customer
        public void Delete(int customerId)
        {
            var customer = _context.Customers.Find(customerId);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }

        }

        // Get total customer count
        public int GetCustomersCount()
        {
            return _context.Customers.Count();
        }

        // Get Customer IDs category
        public List<int> GetCustomerIds()
        {
            return _context.Customers.Select(c => c.Id).ToList();
        }


        // Get Customer by ID
        public string GetCustomerNameById(int customerId)
        {
            var customer = _context.Customers.Find(customerId);
            return customer?.Username ?? String.Empty;
        }

        public List<TopCustomerDto> GetTopCustomers(int count)
        {
            var topCustomers = _context.Customers
        .Join(
            _context.Orders,
            c => c.Id,              
            o => o.CustomerId,      
            (c, o) => new { c, o }  
        )
        .GroupBy(x => new { x.c.Id, x.c.Username })
        .Select(g => new TopCustomerDto
        {
            
            Name = g.Key.Username,
            TotalOrders = g.Count(),
            TotalSpent = g.Sum(x => x.o.TotalAmount),
            
        })
        .OrderByDescending(x => x.TotalOrders)
        .Take(count)
        .ToList();

         return topCustomers;
        }
    }
}
