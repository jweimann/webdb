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
using WebDB.Client.Prism.Modules.EntityGrid.ViewModels;
using Xceed.Wpf.DataGrid;

namespace WebDB.Client.Prism.Modules.EntityGrid.Views
{
    /// <summary>
    /// Interaction logic for RelatedEntitiesView.xaml
    /// </summary>
    public partial class RelatedEntitiesView : UserControl, IRelatedEntitiesView
    {
        IEntityGridViewModel _context;
        public RelatedEntitiesView()
        {
            InitializeComponent();
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var dc = this.DataContext;
            _context = this.DataContext as IEntityGridViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var selectedItem = (DataContext as IEntityGridViewModel).SelectedItem;

            if (selectedItem == null)
                return;

                Type type = selectedItem.GetType();
            var relationshipProperties = type.GetProperties()
             .Where(t =>
                     t.Name != "Relationships" &&
                     t.GetGetMethod().IsVirtual &&
                     t.PropertyType.IsGenericType &&
                     t.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
             .ToList();

            foreach (var property in relationshipProperties)
            {
                //Binding binding = new Binding() { Path = new PropertyPath("SelectedItem." + property.Name), Source = this.DataContext };

                TextBlock textBlock = new TextBlock() { Text = property.Name };
                //BindingOperations.SetBinding(textBlock, TextBlock.TextProperty, binding);

                 
                relatedEntitiesStackPanel.Children.Add(textBlock);

                Binding binding = new Binding() { Path = new PropertyPath("SelectedItem." + property.Name), Source = this.DataContext };

                DataGridControl listView = new DataGridControl();
                var value = property.GetValue(selectedItem) as System.Collections.IEnumerable;
                //listView.ItemsSource = value;
                BindingOperations.SetBinding(listView, DataGrid.ItemsSourceProperty, binding);

                relatedEntitiesStackPanel.Children.Add(listView);
            }
        }
    }
}
