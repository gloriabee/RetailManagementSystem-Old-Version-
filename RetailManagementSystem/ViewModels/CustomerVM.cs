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

        public Customer SelectedCustomer { get; set; }

        // Commands

        public ICommand ShowWindowCommand { get; }
        public ICommand AddCustomerCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCustomerCommand { get; }
        public ICommand RefreshCommand { get; }

        public CustomerVM()
        {
            _unitOfWork = new UnitOfWork(new RetailDbContext());

            ShowWindowCommand = new RelayCommand(_ => ShowAddCustomerWindow());
            AddCustomerCommand = new RelayCommand(async param => await AddCustomerAsync(param));
            UpdateCommand = new RelayCommand(async param => await EditCustomerAsync(param), _ => SelectedCustomer != null);
            DeleteCustomerCommand = new RelayCommand(async _ => await DeleteCustomerAsync(), _ => SelectedCustomer != null);
            RefreshCommand = new RelayCommand(async _ => await LoadPagedCustomersAsync());

            Pagination.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(Pagination.CurrentPage) || e.PropertyName == nameof(Pagination.PageSize))
                    await LoadPagedCustomersAsync();
            };

            _ = LoadPagedCustomersAsync();
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
            if (SelectedCustomer == null)
            {
                MessageBox.Show("Please select a customer to edit.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                SelectedCustomer.Username = Username ?? SelectedCustomer.Username;
                SelectedCustomer.Email = Email ?? SelectedCustomer.Email;
                SelectedCustomer.Phone = Phone ?? SelectedCustomer.Phone;
                SelectedCustomer.Address = Address ?? SelectedCustomer.Address;
                SelectedCustomer.Country = Country ?? SelectedCustomer.Country;

                await _unitOfWork.Customers.UpdateAsync(SelectedCustomer);
                _unitOfWork.Complete();

                await LoadPagedCustomersAsync();
                MessageBox.Show("Customer updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating customer: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task DeleteCustomerAsync()
        {
            if (SelectedCustomer == null)
            {
                MessageBox.Show("Please select a customer to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Are you sure you want to delete '{SelectedCustomer.Username}'?",
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    await _unitOfWork.Customers.DeleteAsync(SelectedCustomer);
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
