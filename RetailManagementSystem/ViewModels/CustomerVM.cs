using RetailManagementSystem.Components;
using RetailManagementSystem.DTOs;
using RetailManagementSystem.Interfaces;
using RetailManagementSystem.Models;
using RetailManagementSystem.Repositories;
using RetailManagementSystem.Views;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Input;

namespace RetailManagementSystem.ViewModels
{
    public class CustomerVM : ViewModelBase
    {
        private readonly UnitOfWork _unitOfWork;
        public PaginationVM Pagination { get; set; } = new PaginationVM();

        private string _filterText;
        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged(nameof(FilterText));
                _ = LoadPagedCustomersAsync();
            }
        }

        public ObservableCollection<Customer> Customers { get; set; } = new ObservableCollection<Customer>();

        // Fields for input/edit
        private int _id;
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        private string _username;
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        private string _phone;
        public string Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(nameof(Phone)); }
        }

        private string _address;
        public string Address
        {
            get => _address;
            set { _address = value; OnPropertyChanged(nameof(Address)); }
        }

        private string _country;
        public string Country
        {
            get => _country;
            set { _country = value; OnPropertyChanged(nameof(Country)); }
        }

        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }

        private bool _isAllItemsSelected;
        public bool IsAllItemsSelected
        {
            get => _isAllItemsSelected;
            set
            {
                if (_isAllItemsSelected != value)
                {
                    _isAllItemsSelected= value;
                    OnPropertyChanged(nameof(IsAllItemsSelected));

                    foreach (var customer in Customers)
                    {
                        customer.IsSelected = value;
                    }
                }
              
            }
        }

        // Commands

        public ICommand ShowWindowCommand { get; }
        public ICommand AddCustomerCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand SaveCommand {  get; }
        public ICommand DeleteCommand { get; }
        public ICommand MultipleDeleteCommand { get; }
        public ICommand RefreshCommand { get; }

        public CustomerVM()
        {
            _unitOfWork = new UnitOfWork(new RetailDbContext());

            ShowWindowCommand = new RelayCommand(_ => ShowAddCustomerWindow());
            AddCustomerCommand = new RelayCommand(async param => await AddCustomerAsync(param));
            EditCommand = new RelayCommand(async param => await ShowEditCustomerWindow(param));
            SaveCommand = new RelayCommand(async param => await EditCustomerAsync(param));
            DeleteCommand = new RelayCommand(async param => await DeleteCustomerAsync(param));
            MultipleDeleteCommand = new RelayCommand(async _ => await DeleteSelectedCustomerAsync(), _ => Customers.Any(c => c.IsSelected));
            RefreshCommand = new RelayCommand(async _ => await LoadPagedCustomersAsync());

            Pagination.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(Pagination.CurrentPage) || e.PropertyName == nameof(Pagination.PageSize))
                    await LoadPagedCustomersAsync();
            };

            _ = LoadPagedCustomersAsync();
        }

        private async Task DeleteSelectedCustomerAsync()
        {
            var selectedCustomers = Customers.Where(c => c.IsSelected).ToList();

            if(!selectedCustomers.Any())
            {
                MessageBox.Show("No Customers selected", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure to delete {selectedCustomers.Count} selected customers?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
                );

            if(result == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (var c in selectedCustomers)
                    {
                        await _unitOfWork.Customers.DeleteAsync(c);
                    }

                    _unitOfWork.Complete();
                    await LoadPagedCustomersAsync();
                    MessageBox.Show("Customers deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting customers: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

           
        }

        private void ShowAddCustomerWindow()
        {
            try
            {
                var addWindow = new AddCustomer { DataContext = this };
                addWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Add Order window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ShowEditCustomerWindow(object parameter)
        {
            if(parameter is not Customer customer)
                return;
            var editVM = new CustomerVM
            {
                Id = customer.Id,
                Username = customer.Username,
                Email =customer.Email,
                Phone = customer.Phone,
                Address = customer.Address,
                Country = customer.Country

            };
            var editWindow = new EditCustomer { DataContext = editVM };
            editWindow.ShowDialog();

            await LoadPagedCustomersAsync();
        }


        private async Task LoadPagedCustomersAsync()
        {
            try
            {
                var (pagedCustomers, totalCount) = await _unitOfWork.Customers.GetPagedCustomersAsync(
                    Pagination.CurrentPage,
                    Pagination.PageSize,
                    FilterText
                );

                Customers = new ObservableCollection<Customer>(pagedCustomers);
                Pagination.TotalCount = totalCount;
                OnPropertyChanged(nameof(Customers));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customers: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task AddCustomerAsync(object parameter)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Username))
                {
                    MessageBox.Show("Username is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newCustomer = new Customer
                {
                    Username = Username,
                    Email = Email,
                    Phone = Phone,
                    Address = Address,
                    Country = Country
                };

                await _unitOfWork.Customers.AddAsync(newCustomer);
                _unitOfWork.Complete();

                ClearInputs();
                await LoadPagedCustomersAsync();

                MessageBox.Show("Customer added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding customer: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task EditCustomerAsync(object parameter)
        {

                try
                {
                var updatedCustomer = new Customer
                {
                    Id = Id,
                    Username = Username,
                    Email = Email,
                    Phone = Phone,
                    Address = Address,
                    Country = Country
                };
                    await _unitOfWork.Customers.UpdateAsync(updatedCustomer);
                    _unitOfWork.Complete();
                    MessageBox.Show("Customer updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating customer: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
              
        

        private async Task DeleteCustomerAsync(object parameter)
        {
            if (parameter is not Customer customer)
            {
                MessageBox.Show("Please select a customer to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Are you sure you want to delete '{customer.Username}'?",
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    await _unitOfWork.Customers.DeleteAsync(customer);
                    _unitOfWork.Complete();
                    await LoadPagedCustomersAsync();
                    MessageBox.Show("Customer deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting customer: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ClearInputs()
        {
            Username = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
            Address = string.Empty;
            Country = string.Empty;
        }
    }
}
