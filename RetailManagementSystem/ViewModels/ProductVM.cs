
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
    public class ProductVM : ViewModelBase
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
                _ = LoadPagedProductsAsync();
            }
        }

  

        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();

        // Fields for input/edit
        private string _productName;
        public string ProductName
        {
            get => _productName;
            set { _productName = value; OnPropertyChanged(nameof(ProductName)); }
        }


        private string _productDescription;
        public string ProductDescription
        {
            get => _productDescription;
            set { _productDescription = value; OnPropertyChanged(nameof(ProductDescription)); }
        }

        private string _productCategory;
        public string ProductCategory
        {
            get => _productCategory;
            set { _productCategory = value; OnPropertyChanged(nameof(ProductCategory)); }
        }

        private decimal _productPrice;
        public decimal ProductPrice
        {
            get => _productPrice;
            set { _productPrice = value; OnPropertyChanged(nameof(ProductPrice)); }
        }
        private string _selectedCategoryName;
        public string SelectedCategoryName
        {
            get => _selectedCategoryName;
            set
            {
                _selectedCategoryName = value;
                OnPropertyChanged(nameof(SelectedCategoryName));
            }
        }
        public Product SelectedProduct { get; set; }
        public ObservableCollection<string> Categories { get; set; } 

        // Commands 
        public ICommand ShowWindowCommand { get; }
        public ICommand AddProductCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand RefreshCommand { get; }


        public ProductVM()
        {
           _unitOfWork= new UnitOfWork(new RetailDbContext());
            Categories = new ObservableCollection<string>(_unitOfWork.Products.GetAllCategories());
            ShowWindowCommand = new RelayCommand(_ => ShowAddProductWindow());
            AddProductCommand = new RelayCommand(async param => await AddProductAsync(param));
            EditProductCommand = new RelayCommand(async _ => await EditProductAsync(), _ => SelectedProduct != null);
            DeleteProductCommand = new RelayCommand(async _ => await DeleteProductAsync(), _ => SelectedProduct != null);
            RefreshCommand = new RelayCommand(async _ => await LoadPagedProductsAsync());

            Pagination.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(Pagination.CurrentPage) || e.PropertyName == nameof(Pagination.PageSize))
                    await LoadPagedProductsAsync();
            };

            _ = LoadPagedProductsAsync();
        }

       

        private void ShowAddProductWindow()
        {
            try
            {
                var addWindow = new AddProduct { DataContext = this };
                addWindow.ShowDialog();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error opening Add Product window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadPagedProductsAsync()
        {
            try
            {
                var (pagedProducts, toalCount) = await _unitOfWork.Products.GetPagedProductsAsync(
                    Pagination.CurrentPage,
                    Pagination.PageSize,
                    FilterText
                    );

                Products = new ObservableCollection<Product>(pagedProducts);
                Pagination.TotalCount = toalCount;
                OnPropertyChanged(nameof(Products));
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private async Task AddProductAsync(object parameter)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ProductName))
                {
                    MessageBox.Show("Product Name is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var newProduct = new Product
                {
                    ProductName = ProductName,
                    Description = ProductDescription,
                    Category= SelectedCategoryName,
                    Price=ProductPrice
                };

                await _unitOfWork.Products.AddAsync(newProduct);
                _unitOfWork.Complete();

                ClearInputs();
                await LoadPagedProductsAsync();

                MessageBox.Show("Product added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error adding product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

      
        private async Task EditProductAsync()
        {
           if(SelectedProduct == null)
            {
                MessageBox.Show("Please select a product to edit.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                SelectedProduct.ProductName= ProductName ?? SelectedProduct.ProductName;
                SelectedProduct.Description = ProductDescription ?? SelectedProduct.Description;
                SelectedProduct.Category = ProductCategory ?? SelectedProduct.Category;
                SelectedProduct.Price = ProductPrice;

                await _unitOfWork.Products.UpdateAsync(SelectedProduct);
                _unitOfWork.Complete();

                await LoadPagedProductsAsync();
                MessageBox.Show("Product updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            catch(Exception ex)
            {
                MessageBox.Show($"Error updating product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

       
        private async Task DeleteProductAsync()
        {
            if (SelectedProduct == null)
            {
                MessageBox.Show("Please select a product to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Are you sure you want to delete '{SelectedProduct.ProductName}'?",
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if(confirm== MessageBoxResult.Yes)
            {
                try
                {
                    await _unitOfWork.Products.DeleteAsync(SelectedProduct);
                    _unitOfWork.Complete();
                    await LoadPagedProductsAsync();
                    MessageBox.Show("Product deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"Error deleting product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

           
        }


        private void ClearInputs()
        {
            ProductName = string.Empty;
            ProductDescription=string.Empty;
            SelectedCategoryName = string.Empty;
            ProductPrice = 0;
        }


    }
}