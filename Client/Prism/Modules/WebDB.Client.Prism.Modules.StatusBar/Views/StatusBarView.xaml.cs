using System.Windows.Controls;
using WebDB.Client.Prism.Core;
using WebDB.Client.Prism.Modules.StatusBar.ViewModels;

namespace WebDB.Client.Prism.Modules.StatusBar.Views
{
    /// <summary>
    /// Interaction logic for StatusBarView.xaml
    /// </summary>
    public partial class StatusBarView : UserControl, IStatusBarView
    {
        public StatusBarView()
        {
            InitializeComponent();
        }
        
        public IViewModel ViewModel
        {
            get
            {
                return (IStatusBarViewModel)DataContext;
            }

            set
            {
                DataContext = value;
            }
        }
    }
}
