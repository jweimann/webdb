using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDB.Client.Prism.Core;

namespace WebDB.Client.Prism.Modules.Toolbar
{
    public class ContentAViewViewModel : IContentViewViewModel 
    {
        public IView View { get; set; }

        public string Message { get; set; }

        //public ContentAViewViewModel() { }
        public ContentAViewViewModel(IContentView view)
        {
            View = view;
            View.DataContext = this;
        }
    }
}
