using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RetailManagementSystem.Models;

namespace RetailManagementSystem.Views
{
    public partial class GenericDataGrid : UserControl
    {
        public GenericDataGrid()
        {
            InitializeComponent();
            ColumnsDefinition = new ObservableCollection<DataGridColumn>();
        }

        // 🧾 ItemsSource
        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(GenericDataGrid));

        // 🔍 Filter
        public string FilterText
        {
            get => (string)GetValue(FilterTextProperty);
            set => SetValue(FilterTextProperty, value);
        }
        public static readonly DependencyProperty FilterTextProperty =
            DependencyProperty.Register(nameof(FilterText), typeof(string), typeof(GenericDataGrid));

        // 🧭 Pagination
        public int CurrentPage
        {
            get => (int)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register(nameof(CurrentPage), typeof(int), typeof(GenericDataGrid));

        public int TotalPages
        {
            get => (int)GetValue(TotalPagesProperty);
            set => SetValue(TotalPagesProperty, value);
        }
        public static readonly DependencyProperty TotalPagesProperty =
            DependencyProperty.Register(nameof(TotalPages), typeof(int), typeof(GenericDataGrid));

        // ⚙️ Commands
        public ICommand AddCommand
        {
            get => (ICommand)GetValue(AddCommandProperty);
            set => SetValue(AddCommandProperty, value);
        }
        public static readonly DependencyProperty AddCommandProperty =
            DependencyProperty.Register(nameof(AddCommand), typeof(ICommand), typeof(GenericDataGrid));

        public ICommand DeleteCommand
        {
            get => (ICommand)GetValue(DeleteCommandProperty);
            set => SetValue(DeleteCommandProperty, value);
        }
        public static readonly DependencyProperty DeleteCommandProperty =
            DependencyProperty.Register(nameof(DeleteCommand), typeof(ICommand), typeof(GenericDataGrid));

        public ICommand DeleteAllCommand
        {
            get => (ICommand)GetValue(DeleteAllCommandProperty);
            set => SetValue(DeleteAllCommandProperty, value);
        }

        public static readonly DependencyProperty DeleteAllCommandProperty =
            DependencyProperty.Register(nameof(DeleteAllCommand), typeof(ICommand), typeof(GenericDataGrid));

        public ICommand FirstPageCommand
        {
            get => (ICommand)GetValue(FirstPageCommandProperty);
            set => SetValue(FirstPageCommandProperty, value);
        }
        public static readonly DependencyProperty FirstPageCommandProperty =
            DependencyProperty.Register(nameof(FirstPageCommand), typeof(ICommand), typeof(GenericDataGrid));

        public ICommand PrevPageCommand
        {
            get => (ICommand)GetValue(PrevPageCommandProperty);
            set => SetValue(PrevPageCommandProperty, value);
        }
        public static readonly DependencyProperty PrevPageCommandProperty =
            DependencyProperty.Register(nameof(PrevPageCommand), typeof(ICommand), typeof(GenericDataGrid));

        public ICommand NextPageCommand
        {
            get => (ICommand)GetValue(NextPageCommandProperty);
            set => SetValue(NextPageCommandProperty, value);
        }
        public static readonly DependencyProperty NextPageCommandProperty =
            DependencyProperty.Register(nameof(NextPageCommand), typeof(ICommand), typeof(GenericDataGrid));

        public ICommand LastPageCommand
        {
            get => (ICommand)GetValue(LastPageCommandProperty);
            set => SetValue(LastPageCommandProperty, value);
        }
        public static readonly DependencyProperty LastPageCommandProperty =
            DependencyProperty.Register(nameof(LastPageCommand), typeof(ICommand), typeof(GenericDataGrid));

        // 🏷️ Column definition
        public ObservableCollection<DataGridColumn> ColumnsDefinition
        {
            get => (ObservableCollection<DataGridColumn>)GetValue(ColumnsDefinitionProperty);
            set => SetValue(ColumnsDefinitionProperty, value);
        }
        public static readonly DependencyProperty ColumnsDefinitionProperty =
            DependencyProperty.Register(nameof(ColumnsDefinition), typeof(ObservableCollection<DataGridColumn>), typeof(GenericDataGrid),
                new PropertyMetadata(new ObservableCollection<DataGridColumn>(), OnColumnsChanged));

        private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GenericDataGrid grid)
            {
                grid.SetupColumns();
            }
        }

        private void SetupColumns()
        {
            if (DataGridControl == null) return;

            DataGridControl.Columns.Clear();

            // ✅ Create header checkbox
            var headerCheckBox = new CheckBox
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                IsThreeState = false
            };
            headerCheckBox.Checked += (s, e) => SelectAllRows(true);
            headerCheckBox.Unchecked += (s, e) => SelectAllRows(false);

            // ✅ Checkbox column with single-click behavior
            var checkBoxColumn = new DataGridTemplateColumn
            {
                Width = 40,
                Header = headerCheckBox
            };

            // Create cell template
            var checkBoxFactory = new FrameworkElementFactory(typeof(CheckBox));
            checkBoxFactory.SetBinding(CheckBox.IsCheckedProperty, new System.Windows.Data.Binding("IsSelected")
            {
                Mode = System.Windows.Data.BindingMode.TwoWay,
                UpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.PropertyChanged
            });
            checkBoxFactory.SetValue(CheckBox.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            checkBoxFactory.SetValue(CheckBox.VerticalAlignmentProperty, VerticalAlignment.Center);

            checkBoxColumn.CellTemplate = new DataTemplate { VisualTree = checkBoxFactory };
            DataGridControl.Columns.Add(checkBoxColumn);

            // ✅ Add user-defined columns
            foreach (var col in ColumnsDefinition)
                DataGridControl.Columns.Add(col);

            // ✅ Operations column
            var templateColumn = new DataGridTemplateColumn
            {
                Header = "Operations",
                CellTemplate = (DataTemplate)Resources["OperationsTemplate"]
            };
            DataGridControl.Columns.Add(templateColumn);
        }





        private void SelectAllRows(bool isSelected)
        {
            if (ItemsSource == null) return;

            foreach (var item in ItemsSource)
            {
                var prop = item.GetType().GetProperty("IsSelected");
                if (prop != null && prop.CanWrite)
                    prop.SetValue(item, isSelected);
            }
        }

    }
}
