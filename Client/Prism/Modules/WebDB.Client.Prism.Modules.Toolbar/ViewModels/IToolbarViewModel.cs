using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDB.Client.Prism.Core;

namespace WebDB.Client.Prism.Modules.Toolbar.ViewModels
{
    public interface IToolbarViewModel : IViewModel
    {
        DelegateCommand SearchCommand { get; set; }
        string SearchText { get; set; }
        DateTime LastSaveTime { get; set; }
        string TitleMessage { get; set; }
        List<string> EntityTypes { get; set; }
        string SelectedEntityType { get; set; }
    }
}
