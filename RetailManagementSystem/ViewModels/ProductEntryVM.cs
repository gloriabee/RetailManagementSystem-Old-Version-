using RetailManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagementSystem.ViewModels
{
    public class ProductEntryVM : ViewModelBase
    {
        private readonly ProductService _productService;

        public ProductEntryVM()
        {
            _productService = new ProductService();
            LoadProductIds();
        }

        private void LoadProductIds()
        {
            var ids = _productService.GetProductIds();
            ProductIds = new ObservableCollection<int>(ids);
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

        private int _selectedProductId;
        public int SelectedProductId
        {
            get => _selectedProductId;
            set
            {
                _selectedProductId = value;
                OnPropertyChanged(nameof(SelectedProductId));
                UpdateProductName();
                UpdateProductUnitPrice();


            }
        }

        private void UpdateTotalPrice()
        {
            Total = Quantity * decimal.Parse(SelectedProductUnitPrice);
        }

        private void UpdateProductName()
        {
            SelectedProductName = _productService.GetProductNameById(SelectedProductId);
        }

        private void UpdateProductUnitPrice()
        {
            SelectedProductUnitPrice = _productService.GetProductPriceById(SelectedProductId).ToString();
        }

        private string _selectedProductName;
        public string SelectedProductName
        {
            get => _selectedProductName;
            set
            {
                _selectedProductName = value;
                OnPropertyChanged(nameof(SelectedProductName));
            }
        }

        private string _selectedProductUnitPrice;
        public string SelectedProductUnitPrice
        {
            get => _selectedProductUnitPrice;
            set
            {
                _selectedProductUnitPrice = value;
                OnPropertyChanged(nameof(SelectedProductUnitPrice));
            }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
                UpdateTotalPrice();
            }
        }

        private decimal _total;
        public decimal Total
        {
            get => _total;
            set
            {
                _total = value;
                OnPropertyChanged(nameof(Total));
            }
        }
    }
}