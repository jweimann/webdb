using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDB.Client.Prism.Core;
using WebDB.Model;

namespace WebDB.Client.Prism.Modules.EntityGrid.ViewModels
{
    public interface IEntityGridViewModel : IViewModel
    {
        DelegateCommand SaveCommand { get; set; }
        string ViewName { get; set; }
        void SetViewName(string viewName);
        ObservableCollection<object> Items { get; set; }
        IModelObject SelectedItem { get; set; }

        void SetItems(List<object> results);
    }
}
