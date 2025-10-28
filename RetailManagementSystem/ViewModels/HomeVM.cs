using RetailManagementSystem.DTOs;
using RetailManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagementSystem.ViewModels
{
    public class HomeVM:ViewModelBase
    {
        private readonly ProductService _productService;
        private readonly CustomerService _customerService;
        private readonly OrderRepository _orderRepository;
        

        public int totalProducts { get; set; }
        public int totalCustomers { get; set; }
        public int totalOrders { get; set; }
        public decimal totalRevenue { get; set; }
        public ObservableCollection<TopCustomerDto> TopCustomers { get; set; }


        public HomeVM()
        {
            var dbContext = new RetailDbContext();
            _productService = new ProductService();
            _customerService = new CustomerService();
            _orderRepository = new OrderRepository(dbContext);
            totalProducts = _productService.GetProductsCount();
            totalCustomers= _customerService.GetCustomersCount();
            totalOrders=_orderRepository.GetAllOrders().Count;
            totalRevenue= Math.Round(_orderRepository.GetAllOrders().Sum(o => o.Subtotal),2);

            var topCustomersList = _customerService.GetTopCustomers(5);
            TopCustomers = new ObservableCollection<TopCustomerDto>(topCustomersList);

            OnPropertyChanged(nameof(totalProducts));
            OnPropertyChanged(nameof(totalCustomers));
            OnPropertyChanged(nameof(totalOrders));
            OnPropertyChanged(nameof(totalRevenue));
            OnPropertyChanged(nameof(TopCustomers));
        }

    }
}
