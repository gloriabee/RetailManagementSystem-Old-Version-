using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetailManagementSystem.WPF.Utils;
using System.Windows.Input;

namespace RetailManagementSystem.WPF.ViewModels
{
    public class NavigationVM:ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand HomeCommand { get; set; }

        public ICommand CustomersCommand { get; set; }

        public ICommand ProductsCommand { get; set; }

        private void Home(object obj) => CurrentView = new HomeVM();
        private void Customer(object obj) => CurrentView = new CustomerVM();
        private void Product(object obj) => CurrentView = new ProductVM();

        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            CustomersCommand = new RelayCommand(Customer);
            ProductsCommand=new RelayCommand(Product);


            // Startup Page
            CurrentView = new HomeVM();
        }
    }
}
