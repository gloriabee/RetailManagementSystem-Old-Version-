using LiveCharts;
using LiveCharts.Wpf;
using RetailManagementSystem.DTOs;
using RetailManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

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

        // Chart Properties 
        public SeriesCollection seriescollection { get; set; }
        public string[] labels { get; set; }
        public Func<double, string> formatter { get; set; }


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


            // Top 5 categories by products (chart)
            var topCategories = _productService.GetTopCustomers(5); // you can rename this to GetTopCategories()
            LoadTopCategoriesChart(topCategories);


            OnPropertyChanged(nameof(totalProducts));
            OnPropertyChanged(nameof(totalCustomers));
            OnPropertyChanged(nameof(totalOrders));
            OnPropertyChanged(nameof(totalRevenue));
            OnPropertyChanged(nameof(TopCustomers));
            OnPropertyChanged(nameof(seriescollection));
            OnPropertyChanged(nameof(labels));
            OnPropertyChanged(nameof(formatter));
        }

        private void LoadTopCategoriesChart(List<TopCategoryDto> topCategories)
        {
            if (topCategories == null || !topCategories.Any())
                return;

            labels = topCategories.Select(c => c.Category).ToArray();
            var values = topCategories.Select(c => (double)c.TotalProducts).ToArray();

            seriescollection = new SeriesCollection
            {
                new RowSeries
                {
                    Title = "Products Count",
                    Values = new ChartValues<double>(values),
                    Fill = new SolidColorBrush(Color.FromRgb(173, 196, 255)), // light blue
                    DataLabels = true
                }
            };

            formatter = value => value.ToString("N0");
        }
    }
}
