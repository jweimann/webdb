using Microsoft.Practices.Prism.Mvvm;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WebDB.Client.Prism.Core;
using WebDB.Client.Prism.Modules.EntityGrid.ViewModels;
using WebDB.Client.Prism.Modules.EntityGrid.Views;
using Xceed.Wpf.DataGrid;

namespace WebDB.Client.Prism.Modules.EntityGrid.Views
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class EntityGridView : UserControl, IView, IEntityGridView
    {
        public EntityGridView()
        {
            InitializeComponent();
        }

        private void UserControl_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            //Binding binding = new Binding() { Path = new PropertyPath("Items"), Source = this.DataContext };

            //DataGrid listView = new DataGrid() { AutoGenerateColumns = true };// { AutoCreateColumns = true };
            ////BindingOperations.SetBinding(listView, DataGrid.ItemsSourceProperty, binding);
            //listView.ItemsSource = (DataContext as EntityGridViewModel).Items;
            //stackPanel.Children.Add(listView);


            //var source = dataGridControl.ItemsSource;
            //dataGridControl.ItemsSource = null;
            //dataGridControl.ItemsSource = source;
        }
    }
}
