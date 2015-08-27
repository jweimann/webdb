using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using WebDB.Client.Core;
using WebDB.Model;

namespace WebDB.Client.WPF
{
    /// <summary>
    /// Interaction logic for ListWindow.xaml
    /// </summary>
    public partial class ListWindow : Window
    {
        public ListWindow()
        {
            InitializeComponent();
        }

        //EntityListViewModel<TModel> _viewModel;
        //private async void Grid_Loaded(object sender, RoutedEventArgs e)
        //{
        //    _viewModel = new EntityListViewModel<TModel>();
        //    WebDBClient<TModel> client = new WebDBClient<TModel>();
        //    _viewModel.SetCollection(await client.Get());

        //    this.DataContext = _viewModel;

        //    //Grid.ItemsSource = allIssues;
        //}

        private void Issue_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    _viewModel.Save();
        //}

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    _viewModel.Undo();
        //}
    }
}
