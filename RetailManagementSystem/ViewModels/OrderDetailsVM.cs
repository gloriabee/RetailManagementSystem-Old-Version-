using RetailManagementSystem.DTOs;
using RetailManagementSystem.Interfaces;
using System.Collections.ObjectModel;

namespace RetailManagementSystem.ViewModels
{
    public class OrderDetailsVM : ViewModelBase
    {
        private readonly OrderRepository _orderRepository;

        public OrderDetailsDto Order { get; }

        private ObservableCollection<OrderProductDto> _orderedProducts;
        public ObservableCollection<OrderProductDto> OrderedProducts
        {
            get => _orderedProducts;
            set
            {
                _orderedProducts = value;
                OnPropertyChanged(nameof(OrderedProducts));
            }
        }

        private decimal _subTotal;
        public decimal SubTotal
        {
            get => _subTotal;
            set
            {
                _subTotal = value;
                OnPropertyChanged(nameof(SubTotal));
            }
        }

        public OrderDetailsVM(OrderDetailsDto order)
        {
            Order = order;

            var dbContext = new RetailDbContext();
            _orderRepository = OrderRepository.Create();

            LoadOrderedProducts(order.OrderId);
        }

        private void LoadOrderedProducts(int orderId)
        {
            var products = _orderRepository.GetOrderProducts(orderId);

            OrderedProducts = new ObservableCollection<OrderProductDto>(products);

            SubTotal = OrderedProducts.Sum(p => p.Total);
        }
    }
}
