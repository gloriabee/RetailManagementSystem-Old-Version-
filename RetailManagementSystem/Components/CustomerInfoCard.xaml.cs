using System;
using System.Collections.Generic;
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
    /// Interaction logic for CustomerInfoCard.xaml
    /// </summary>
    public partial class CustomerInfoCard : UserControl
    {
        public CustomerInfoCard()
        {
            InitializeComponent();
        }



        public string CustomerName
        {
            get { return (string)GetValue(CustomerNameProperty); }
            set { SetValue(CustomerNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CustomerName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomerNameProperty =
            DependencyProperty.Register("CustomerName", typeof(string), typeof(CustomerInfoCard), new PropertyMetadata(string.Empty));



        public string CustomerEmail
        {
            get { return (string)GetValue(CustomerEmailProperty); }
            set { SetValue(CustomerEmailProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CustomerEmail.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomerEmailProperty =
            DependencyProperty.Register("CustomerEmail", typeof(string), typeof(CustomerInfoCard), new PropertyMetadata(string.Empty));



        public string CustomerPhone
        {
            get { return (string)GetValue(CustomerPhoneProperty); }
            set { SetValue(CustomerPhoneProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CustomerPhone.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomerPhoneProperty =
            DependencyProperty.Register("CustomerPhone", typeof(string), typeof(CustomerInfoCard), new PropertyMetadata(string.Empty));



        public string CustomerAddress
        {
            get { return (string)GetValue(CustomerAddressProperty); }
            set { SetValue(CustomerAddressProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CustomerAddress.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomerAddressProperty =
            DependencyProperty.Register("CustomerAddress", typeof(string), typeof(CustomerInfoCard), new PropertyMetadata(string.Empty));



        public string CustomerCountry
        {
            get { return (string)GetValue(CustomerCountryProperty); }
            set { SetValue(CustomerCountryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomerCountryProperty =
            DependencyProperty.Register("CustomerCountry", typeof(string), typeof(CustomerInfoCard), new PropertyMetadata(string.Empty));






    }
}
