using RetailManagementSystem.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RetailManagementSystem.ViewModels
{
    public class OrderVM: ViewModelBase
    {

        public OrderVM()
        {
            ShowWindowCommand = new RelayCommand(_ => ShowAddOrderWindow());
        }
        public ICommand ShowWindowCommand { get; }
        private void ShowAddOrderWindow()
        {
            var addWindow = new AddOrder
            {
                DataContext = this
            };
            addWindow.ShowDialog();
        }

       
    }
}
