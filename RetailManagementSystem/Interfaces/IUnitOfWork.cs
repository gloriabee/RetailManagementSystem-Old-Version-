using RetailManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagementSystem.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IRepository<Product> Products { get; }
        IRepository<Customer> Customers { get; }
        IRepository<Order> Orders { get; }
        Task<int> CompleteAsync(CancellationToken cancellationToken);
        
    }
}
