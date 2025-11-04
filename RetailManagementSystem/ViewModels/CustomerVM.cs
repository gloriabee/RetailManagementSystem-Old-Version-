using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using RetailManagementSystem.DTOs;
using RetailManagementSystem.Models;
using RetailManagementSystem.Services;
using RetailManagementSystem.Views;

namespace RetailManagementSystem.ViewModels
{
    public class CustomerVM : ViewModelBase
    {
        private readonly CustomerService _customerService;

        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }

       


        public CustomerVM()
        {
            _customerService = new CustomerService();
            MyCustomers = new ObservableCollection<Customer>();
            //TopCustomers= new ObservableCollection<TopCustomerDto>();

            // Initialize commands
            FirstPageCommand = new RelayCommand(_ => GoToFirstPage(), _ => CurrentPage > 1);
            LastPageCommand = new RelayCommand(_ => GoToLastPage(), _ => CurrentPage < TotalPages);
            NextPageCommand = new RelayCommand(_ => GoToNextPage(), _ => CurrentPage < TotalPages);
            PrevPageCommand = new RelayCommand(_ => GoToPrevPage(), _ => CurrentPage > 1);
            ChangePageSizeCommand = new RelayCommand(param =>
            {
                if (int.TryParse(param?.ToString(), out int newSize))
                {
                    PageSize = newSize;
                    CurrentPage = 1;
                    LoadCustomers();
                }
            });
            RefreshCommand = new RelayCommand(_ => LoadCustomers());
            ShowWindowCommand = new RelayCommand(_ => ShowAddCustomerWindow());
            AddCustomerCommand = new RelayCommand(AddCustomer);
            UpdateCommand = new RelayCommand(UpdateCustomer);
            DeleteCommand = new RelayCommand(DeleteCustomer);
            DeleteAllCommand = new RelayCommand(_ => DeleteSelectedCustomers());
            LoadCustomers();
            //LoadTopCustomers();
        }

        // Observable collection bound to DataGrid
        private ObservableCollection<Customer> _myCustomers;
        public ObservableCollection<Customer> MyCustomers
        {
            get => _myCustomers;
            set { _myCustomers = value; OnPropertyChanged(); }
        }

        // Filter text (auto-refresh when changed)
        private string _filterText;
        public string FilterText
        {
            get => _filterText;
            set
            {
                if (_filterText != value)
                {
                    _filterText = value;
                    OnPropertyChanged();
                    CurrentPage = 1; // Reset to first page on new search
                    LoadCustomers();
                }
            }
        }

        // Pagination fields
        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged();
                    LoadCustomers();
                }
            }
        }

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set { _pageSize = value; OnPropertyChanged(); }
        }

        private int _totalCount;
        public int TotalCount
        {
            get => _totalCount;
            set
            {
                _totalCount = value;
                OnPropertyChanged(nameof(TotalPages));
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); }
        }

        private bool _allSelected;
        public bool AllSelected
        {
            get => _allSelected;
            set
            {
                _allSelected = value;
                OnPropertyChanged(nameof(AllSelected));
                SelectAllRows(_allSelected);
            }
        }

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public ICommand FirstPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PrevPageCommand { get; }
        public ICommand LastPageCommand { get; }

        public ICommand ChangePageSizeCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ShowWindowCommand { get; }

        public ICommand AddCustomerCommand { get; }

        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        public ICommand DeleteAllCommand { get; }

        // Load data from CustomerService
        private void LoadCustomers()
        {
            try
            {
                var customers = _customerService.GetPagedCustomers(FilterText, CurrentPage, PageSize);
                TotalCount = _customerService.GetTotalCount(FilterText);

                MyCustomers.Clear();
                foreach (var customer in customers)
                    MyCustomers.Add(customer);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customers: {ex.Message}");
            }
        }

        private void GoToFirstPage()
        {

            CurrentPage = 1;
        }


        private void GoToNextPage()
        {
            if (CurrentPage < TotalPages)
                CurrentPage++;
        }

        private void GoToPrevPage()
        {
            if (CurrentPage > 1)
                CurrentPage--;
        }

        private void GoToLastPage()
        {
            CurrentPage = TotalPages;
        }

        private void ShowAddCustomerWindow()
        {

            var addWindow = new AddCustomer
            {
                DataContext = this
            };

            addWindow.ShowDialog();

        }

        private void AddCustomer(object parameter)
        {
            try
            {
                var customer = new Customer
                {
                    Username = Username,
                    Email = Email,
                    Phone = Phone,
                    Address = Address,
                    Country = Country
                };

                _customerService.Add(customer);

                // Clear form fields
                Username = string.Empty;
                Email = string.Empty;
                Phone = string.Empty;
                Address = string.Empty;
                Country = string.Empty;

                OnPropertyChanged(nameof(Username));
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(Phone));
                OnPropertyChanged(nameof(Address));
                OnPropertyChanged(nameof(Country));



                // Refresh customer list
                LoadCustomers();

                // Show success message
                MessageBox.Show("Customer added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to add customer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void UpdateCustomer(object parameter)
        {
            if (parameter is not Customer customer)
                return;

            // Create an edit VM with existing values
            var editVM = new CustomerVM
            {
                Id = customer.Id,
                Username = customer.Username,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address,
                Country = customer.Country
            };

            // Set it as the DataContext for the edit window
            var editWindow = new EditCustomer
            {
                DataContext = editVM
            };

            if (editWindow.ShowDialog() == true)
            {
                customer.Id = editVM.Id;
                customer.Username = editVM.Username;
                customer.Email = editVM.Email;
                customer.Phone = editVM.Phone;
                customer.Address = editVM.Address;
                customer.Country = editVM.Country;

                try
                {

                    _customerService.Update(customer);
                    MessageBox.Show("Customer updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadCustomers();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating customer: {ex.Message}");
                }
            }
        }


        private void DeleteCustomer(object parameter)
        {
            if (parameter is Customer customer)
            {
                var result = MessageBox.Show($"Are you sure to delete {customer.Username}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _customerService.Delete(customer.Id);
                        LoadCustomers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting customer: {ex.Message}");
                    }
                }
            }
        }


        private void DeleteSelectedCustomers()
        {
            var selectedCustomers = MyCustomers.Where(c => c.IsSelected).ToList();
            if (!selectedCustomers.Any())
            {
                MessageBox.Show("No customers selected.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var result = MessageBox.Show($"Are you sure to delete {selectedCustomers.Count} selected customers?",
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (var c in selectedCustomers)
                    {
                        _customerService.Delete(c.Id);

                    }

                    LoadCustomers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting customers: {ex.Message}");
                }
            }
        }

        public void SelectAllRows(bool select)
        {
            foreach (var c in MyCustomers)
                c.IsSelected = select;
        }
    }
}
