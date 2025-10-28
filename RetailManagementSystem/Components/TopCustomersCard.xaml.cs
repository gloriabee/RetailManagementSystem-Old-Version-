using RetailManagementSystem.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RetailManagementSystem.Components
{
    /// <summary>
    /// Interaction logic for TopCustomersCard.xaml
    /// </summary>
    public partial class TopCustomersCard : UserControl
    {
        public TopCustomersCard()
        {
            InitializeComponent();
        }



        public ObservableCollection<TopCustomerDto> TopCustomers
        {
            get { return (ObservableCollection<TopCustomerDto>)GetValue(TopCustomersProperty); }
            set { SetValue(TopCustomersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopCustomersProperty =
            DependencyProperty.Register("TopCustomers", typeof(ObservableCollection<TopCustomerDto>), typeof(TopCustomersCard), new PropertyMetadata(null));


    }
}
