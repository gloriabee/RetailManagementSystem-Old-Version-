

using RetailManagementSystem.Models;
using RetailManagementSystem.Services;
using RetailManagementSystem.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace RetailManagementSystem.ViewModels
{
    public class ProductVM : ViewModelBase
    {
        private readonly ProductService _productService;

        // Product fields
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
       


        public ProductVM()
        {
            ProductColumns = new ObservableCollection<DataGridColumn>
    {
        new DataGridTextColumn { Header = "ID", Binding = new Binding("Id") },
        new DataGridTextColumn { Header = "Product Name", Binding = new Binding("ProductName") },
        new DataGridTextColumn { Header = "Description", Binding = new Binding("Description") },
        new DataGridTextColumn { Header = "Category", Binding = new Binding("Category") },
        new DataGridTextColumn { Header = "Price", Binding = new Binding("Price") }
    };

            _productService = new ProductService();
            MyProducts = new ObservableCollection<Product>();

        

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
                    LoadProducts();
                }
            });

            RefreshCommand = new RelayCommand(_ => LoadProducts());
            ShowWindowCommand = new RelayCommand(_ => ShowAddProductWindow());
            AddProductCommand = new RelayCommand(AddProduct);
            UpdateCommand = new RelayCommand(UpdateProduct);
            DeleteCommand = new RelayCommand(DeleteProduct);
            DeleteAllCommand = new RelayCommand(_ => DeleteSelectedProducts());

            LoadProducts();
        }

        // Observable collection for DataGrid
        private ObservableCollection<Product> _myProducts;
        public ObservableCollection<Product> MyProducts
        {
            get => _myProducts;
            set { _myProducts = value; OnPropertyChanged(); }
        }


        private ObservableCollection<DataGridColumn> _productColumns;
        public ObservableCollection<DataGridColumn> ProductColumns
        {
            get => _productColumns;
            set
            {
                _productColumns = value;
                OnPropertyChanged();
            }

        }

        // Filter text
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
                    CurrentPage = 1;
                    LoadProducts();
                }
            }
        }

        // Pagination
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
                    LoadProducts();
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

        // Commands
        public ICommand FirstPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PrevPageCommand { get; }
        public ICommand LastPageCommand { get; }

        public ICommand ChangePageSizeCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ShowWindowCommand { get; }

        public ICommand AddProductCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand DeleteAllCommand { get; }

        // Load data from ProductService
        private void LoadProducts()
        {
            try
            {
                var products = _productService.GetPagedProducts(FilterText, CurrentPage, PageSize);
                TotalCount = _productService.GetTotalCount(FilterText);

                MyProducts.Clear();
                foreach (var product in products)
                    MyProducts.Add(product);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}");
            }
        }

        private void GoToFirstPage() => CurrentPage = 1;
        private void GoToNextPage() { if (CurrentPage < TotalPages) CurrentPage++; }
        private void GoToPrevPage() { if (CurrentPage > 1) CurrentPage--; }
        private void GoToLastPage() => CurrentPage = TotalPages;

        private void ShowAddProductWindow()
        {
            var addWindow = new AddProduct
            {
                DataContext = this
            };
            addWindow.ShowDialog();
        }

        private void AddProduct(object parameter)
        {
            try
            {
                var product = new Product
                {
                    ProductName = ProductName,
                    Category = Category,
                    Price = Price,
                    Description = Description
                };

                _productService.Add(product);

                // Clear form fields
                ProductName = string.Empty;
                Category = string.Empty;
                Price = 0;
                Description = string.Empty;

                OnPropertyChanged(nameof(ProductName));
                OnPropertyChanged(nameof(Category));
                OnPropertyChanged(nameof(Price));

                OnPropertyChanged(nameof(Description));

                LoadProducts();

                MessageBox.Show("Product added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Failed to add product: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateProduct(object parameter)
        {
            if (parameter is not Product product)
                return;

            var editVM = new ProductVM
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Category = product.Category,
                Price = product.Price,
                Description = product.Description
            };

            var editWindow = new EditProduct
            {
                DataContext = editVM
            };

            if (editWindow.ShowDialog() == true)
            {
                product.Id = editVM.Id;
                product.ProductName = editVM.ProductName;
                product.Category = editVM.Category;
                product.Price = editVM.Price;
                product.Description = editVM.Description;

                try
                {
                    _productService.Update(product);
                    MessageBox.Show("Product updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating product: {ex.Message}");
                }
            }
        }

        private void DeleteProduct(object parameter)
        {
            if (parameter is Product product)
            {
                var result = MessageBox.Show($"Are you sure to delete {product.ProductName}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _productService.Delete(product.Id);
                        LoadProducts();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting product: {ex.Message}");
                    }
                }
            }
        }

        private void DeleteSelectedProducts()
        {
            var selectedProducts = MyProducts.Where(p => p.IsSelected).ToList();
            if (!selectedProducts.Any())
            {
                MessageBox.Show("No products selected.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show($"Are you sure to delete {selectedProducts.Count} selected products?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (var p in selectedProducts)
                        _productService.Delete(p.Id);

                    LoadProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting products: {ex.Message}");
                }
            }
        }

        public void SelectAllRows(bool select)
        {
            foreach (var p in MyProducts)
                p.IsSelected = select;
        }

        


            
    }
}
