using RetailManagementSystem.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace RetailManagementSystem.ViewModels
{
    public class NavigationVM : ViewModelBase
    {
        private object _currentView = new HomeVM();
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand HomeCommand { get; set; }

        public ICommand CustomersCommand { get; set; }

        public ICommand ProductsCommand { get; set; }

        public ICommand OrdersCommand { get; set; }

        private void Home(object obj) => CurrentView = new HomeVM();
        private void Customer(object obj) => CurrentView = new CustomerVM();
        private void Product(object obj) => CurrentView = new ProductVM();
        private void Order(object obj) => CurrentView = new OrderVM();

        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            CustomersCommand = new RelayCommand(Customer);
            ProductsCommand = new RelayCommand(Product);
            OrdersCommand = new RelayCommand(Order);


            // Startup Page
            CurrentView = new HomeVM();
        }
    }
}