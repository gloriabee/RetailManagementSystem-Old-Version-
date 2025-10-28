using RetailManagementSystem.DTOs;
using RetailManagementSystem.Interfaces;
using RetailManagementSystem.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RetailManagementSystem.ViewModels
{
    public class OrderDetailsVM : ViewModelBase
    {

        private readonly OrderRepository _orderRepository;
        public OrderDetailsDto Order { get; }
       

        public OrderDetailsVM(OrderDetailsDto order)
        {

            Order = order;

        }

       
    }
}
