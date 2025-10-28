using RetailManagementSystem.Components;
using RetailManagementSystem.DTOs;
using RetailManagementSystem.Models;
using RetailManagementSystem.Services;
using RetailManagementSystem.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Input;

namespace RetailManagementSystem.ViewModels
{
    public class OrderVM : ViewModelBase
    {
        private readonly CustomerService _customerService;
        private readonly ProductService _productService;
        private readonly OrderRepository _orderRepository;

        public OrderVM()
        {
           

            // Initialize collections
            CustomerIds = new ObservableCollection<int>();
            ProductIds = new ObservableCollection<int>();
            ProductEntries = new ObservableCollection<ProductEntryVM>();
            Orders = new ObservableCollection<OrderDetailsDto>();

            // Initialize services
            var dbContext = new RetailDbContext();
            _customerService = new CustomerService();
            _productService = new ProductService();
            _orderRepository = new OrderRepository(dbContext);

            // Initialize commands
            ShowWindowCommand = new RelayCommand(_ => ShowAddOrderWindow());
            AddProductCommand = new RelayCommand(_ => AddProduct());
            AddOrderCommand = new RelayCommand(AddOrder);
            ShowOrderDetailsCommand = new RelayCommand<OrderDetailsDto>(orderCustomer =>
            {
                if (orderCustomer == null) return;
                ShowDetailsScreen(orderCustomer);
            });




            // Load data safely
            SafeLoadData();

            AddProduct();
        }

        private void ShowDetailsScreen(OrderDetailsDto order)
        {
            if (order == null)
            {
                MessageBox.Show("Order not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Create a new Window and host the UserControl
            var detailsWindow = new Window
            {
                Title = "Order Details",
                Content = new Views.OrderDetails
                {
                    DataContext = new OrderDetailsVM(order) // Pass selected order instance
                },
                Width = 800,
                Height = 450,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            detailsWindow.ShowDialog();
        }

        private void SafeLoadData()
        {
            try
            {
                LoadCustomerIds();
                LoadOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Initialization Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ==== Commands ====
        public ICommand ShowWindowCommand { get; }
        public ICommand AddProductCommand { get; }
        public ICommand AddOrderCommand { get; }
        public ICommand ShowOrderDetailsCommand { get; } 


        // ==== Data Collections ====
        public ObservableCollection<int> CustomerIds { get; set; }
        public ObservableCollection<int> ProductIds { get; set; }
        public ObservableCollection<ProductEntryVM> ProductEntries { get; set; }
        public ObservableCollection<OrderDetailsDto> Orders { get; set; }

        // ==== Selected Values ====
        private int _selectedCustomerId;
        public int SelectedCustomerId
        {
            get => _selectedCustomerId;
            set
            {
                _selectedCustomerId = value;
                OnPropertyChanged(nameof(SelectedCustomerId));
                UpdateCustomerName();
            }
        }

        private string _selectedCustomerName;
        public string SelectedCustomerName
        {
            get => _selectedCustomerName;
            set { _selectedCustomerName = value; OnPropertyChanged(nameof(SelectedCustomerName)); }
        }

        private void UpdateCustomerName()
        {
            SelectedCustomerName = _customerService?.GetCustomerNameById(SelectedCustomerId) ?? "Unknown";
        }

        // ==== Methods ====
        private void ShowAddOrderWindow()
        {
            try
            {
                var addWindow = new AddOrder { DataContext = this };
                addWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Add Order window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddProduct()
        {
            ProductEntries.Add(new ProductEntryVM());
        }

        private void LoadCustomerIds()
        {
            var ids = _customerService?.GetCustomerIds() ?? new List<int>();
            CustomerIds = new ObservableCollection<int>(ids);
        }


        private void LoadOrders()
        {
            var allOrders = _orderRepository?.GetAllOrders() ?? new List<OrderDetailsDto>();
            Orders = new ObservableCollection<OrderDetailsDto>(allOrders);
        }

        

        private void AddOrder(object parameter)
        {
            try
            {
                if (ProductEntries.Count == 0)
                {
                    MessageBox.Show("Please add at least one product.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var order = new Order
                {
                    CustomerId = SelectedCustomerId,
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = ProductEntries.Sum(p => p.Total)
                };

                _orderRepository.Add(order);

                foreach (var pe in ProductEntries)
                {
                    var detail = new OrderDetail
                    {
                        OrderId = order.Id,
                        ProductId = pe.SelectedProductId,
                        Quantity = pe.Quantity,
                        UnitPrice = Convert.ToDecimal(pe.SelectedProductUnitPrice)
                    };
                    _orderRepository.AddDetails(detail);
                }

                MessageBox.Show("Order added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadOrders();
            }
            catch (Exception e)
            {
                var error = e.InnerException?.Message ?? e.Message;
                MessageBox.Show($"Error adding order: {error}", "Add Order Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
