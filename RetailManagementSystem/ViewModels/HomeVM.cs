using RetailManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagementSystem.ViewModels
{
    public class HomeVM:ViewModelBase
    {
        private readonly ProductService _productService;
        private readonly CustomerService _customerService;

        public int totalProducts { get; set; }
        public int totalCustomers { get; set; }

        public HomeVM()
        {
            _productService = new ProductService();
            _customerService = new CustomerService();
            totalProducts = _productService.GetProductsCount();
            totalCustomers= _customerService.GetCustomersCount();
            OnPropertyChanged(nameof(totalProducts));
            OnPropertyChanged(nameof(totalCustomers));
        }

    }
}
