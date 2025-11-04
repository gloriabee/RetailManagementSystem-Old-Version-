using System.Windows;
using System.Windows.Controls;
using RetailManagementSystem.ViewModels;

namespace RetailManagementSystem.Views
{
    public partial class Customers : UserControl
    {
        public Customers()
        {
            InitializeComponent();
        }

        //private void HeaderCheckBox_Checked(object sender, RoutedEventArgs e)
        //{
        //    if(DataContext is CustomerVM vm)
        //    {
        //        vm.SelectAllRows(true);
        //    }
        //}

        //private void HeaderCheckBox_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    if (DataContext is CustomerVM vm)
        //    {
        //        vm.SelectAllRows(false);
        //    }
        //}
    }
}
