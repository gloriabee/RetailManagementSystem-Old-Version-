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
      ICustomerRepository Customers { get; }
      IProductRepository Products { get; }
      IOrderRepository Orders { get; }
      int Complete();
      
    }
}
