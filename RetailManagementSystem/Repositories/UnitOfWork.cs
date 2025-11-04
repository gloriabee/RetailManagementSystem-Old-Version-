using RetailManagementSystem.Interfaces;
using RetailManagementSystem.Models;
using RetailManagementSystem.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagementSystem.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RetailDbContext _context;
        public UnitOfWork(RetailDbContext context)
        {
            _context = context;
            Customers= new CustomerRepository(_context);
            Products= new ProductRepository(_context);
            Orders= new OrderRepository(_context);
        }
        public ICustomerRepository Customers { get; private set; }

        public IProductRepository Products { get; private set; }

        public IOrderRepository Orders { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
           _context.Dispose();
        }
    }
}
