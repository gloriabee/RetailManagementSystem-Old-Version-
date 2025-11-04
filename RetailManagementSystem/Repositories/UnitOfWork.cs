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
    public class UnitOfWork:IUnitOfWork
    {
        private readonly RetailDbContext _dbContext;
        public UnitOfWork(RetailDbContext dbContext)
        {
            _dbContext = dbContext;
            Products= new Repository<Product>(_dbContext);
            Customers= new Repository<Customer>(_dbContext);
            Orders= new Repository<Order>(_dbContext);
        }

        public IRepository<Product> Products { get; }
        public IRepository<Customer> Customers { get; }
        public IRepository<Order> Orders { get; }

        public async Task<int> CompleteAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
