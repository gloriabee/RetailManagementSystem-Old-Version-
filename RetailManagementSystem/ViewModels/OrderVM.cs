using RetailManagementSystem.Components;
using RetailManagementSystem.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace RetailManagementSystem.ViewModels
{
    public class OrderVM : ViewModelBase
    {
        private readonly CustomerService _customerService;
        private readonly ProductService _productService;

        public OrderVM()
        {
            _customerService = new CustomerService();
            _productService = new ProductService();

            // Initialize collections
            CustomerIds = new ObservableCollection<int>();
            ProductIds = new ObservableCollection<int>();
            ProductEntries = new ObservableCollection<ProductEntryVM>();

            // Load data
            LoadCustomerIds();
            LoadProductIds();

            // Initialize commands
            ShowWindowCommand = new RelayCommand(_ => ShowAddOrderWindow());
            AddProductCommand = new RelayCommand(_ => AddProduct());

            // Add default product entry
            ProductEntries.Add(new ProductEntryVM
            {
                ProductIds = ProductIds
            });
        }

        // ==== Commands ====
        public ICommand ShowWindowCommand { get; }
        public ICommand AddProductCommand { get; }

        // ==== Data Collections ====
        private ObservableCollection<int> _customerIds;
        public ObservableCollection<int> CustomerIds
        {
            get => _customerIds;
            set
            {
                _customerIds = value;
                OnPropertyChanged(nameof(CustomerIds));
            }
        }

        private ObservableCollection<int> _productIds;
        public ObservableCollection<int> ProductIds
        {
            get => _productIds;
            set
            {
                _productIds = value;
                OnPropertyChanged(nameof(ProductIds));
            }
        }

        private ObservableCollection<ProductEntryVM> _productEntries;
        public ObservableCollection<ProductEntryVM> ProductEntries
        {
            get => _productEntries;
            set
            {
                _productEntries = value;
                OnPropertyChanged(nameof(ProductEntries));
            }
        }

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
            set
            {
                _selectedCustomerName = value;
                OnPropertyChanged(nameof(SelectedCustomerName));
               
            }
        }



        private void UpdateCustomerName()
        {
            SelectedCustomerName = _customerService.GetCustomerNameById(SelectedCustomerId);
        }

        // ==== Methods ====

        private void ShowAddOrderWindow()
        {
            var addWindow = new AddOrder
            {
                DataContext = this
            };
            addWindow.ShowDialog();
        }

        private void AddProduct()
        {
            ProductEntries.Add(new ProductEntryVM
            {
                ProductIds = ProductIds
            });
        }

        private void LoadCustomerIds()
        {
            var ids = _customerService.GetCustomerIds();
            CustomerIds = new ObservableCollection<int>(ids);
        }

        private void LoadProductIds()
        {
            var ids = _productService.GetProductIds();
            ProductIds = new ObservableCollection<int>(ids);
        }
    }
}
