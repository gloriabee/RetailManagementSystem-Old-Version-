using RetailManagementSystem.DTOs;
using System.Windows.Input;

namespace RetailManagementSystem.ViewModels
{
    public class OrderDetailsVM : ViewModelBase
    {
  

        public OrderDetailsDto Order { get; }
       

        public OrderDetailsVM(OrderDetailsDto order)
        {
            Order = order;
            
            
        }

        
    }
}
