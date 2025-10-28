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
    /// Interaction logic for OrderInfoCard.xaml
    /// </summary>
    public partial class OrderInfoCard : UserControl
    {
        public OrderInfoCard()
        {
            InitializeComponent();
        }


        public int OrderId
        {
            get { return (int)GetValue(OrderIdProperty); }
            set { SetValue(OrderIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrderIdProperty =
            DependencyProperty.Register("OrderId", typeof(int), typeof(OrderInfoCard), new PropertyMetadata(0));


        public DateTime OrderDate
        {
            get { return (DateTime)GetValue(OrderDateProperty); }
            set { SetValue(OrderDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrderDateProperty =
            DependencyProperty.Register("OrderDate", typeof(DateTime), typeof(OrderInfoCard), new PropertyMetadata(DateTime.UtcNow));

    }
}
