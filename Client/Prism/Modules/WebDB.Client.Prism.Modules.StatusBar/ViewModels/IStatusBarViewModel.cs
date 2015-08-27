using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDB.Client.Prism.Core;

namespace WebDB.Client.Prism.Modules.StatusBar.ViewModels
{
    public interface IStatusBarViewModel : IViewModel
    {
        string LastMessage { get; set; }
    }
}
