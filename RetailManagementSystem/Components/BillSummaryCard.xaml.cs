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
    /// Interaction logic for BillSummaryCard.xaml
    /// </summary>
    public partial class BillSummaryCard : UserControl
    {
        public BillSummaryCard()
        {
            InitializeComponent();
        }



        public ObservableCollection<OrderProductDto> OrderedProducts
        {
            get { return (ObservableCollection<OrderProductDto>)GetValue(OrderedProductsProperty); }
            set { SetValue(OrderedProductsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrderedProductsProperty =
            DependencyProperty.Register("OrderedProducts", typeof(ObservableCollection<OrderProductDto>), typeof(BillSummaryCard), new PropertyMetadata(null));




        public decimal SubTotal
        {
            get { return (decimal)GetValue(SubTotalProperty); }
            set { SetValue(SubTotalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SubTotal.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SubTotalProperty =
            DependencyProperty.Register("SubTotal", typeof(decimal), typeof(BillSummaryCard), new PropertyMetadata(0m));



    }
}
