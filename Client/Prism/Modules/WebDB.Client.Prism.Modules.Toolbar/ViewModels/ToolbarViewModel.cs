using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDB.Client.Prism.Core;
using WebDB.Client.Prism.Modules.Toolbar.Views;
using WebDB.Messages;

namespace WebDB.Client.Prism.Modules.Toolbar.ViewModels
{
    [ImplementPropertyChanged]
    public class ToolbarViewModel : BindableBase, IToolbarViewModel
    {
        public DelegateCommand SearchCommand { get; set; }
        public DateTime LastSaveTime { get; set; }
        public string TitleMessage { get; set; }

        private IEventAggregator _eventAggregator;
        public ToolbarViewModel(IToolbarView view, IEventAggregator eventAggregator)
           // :base(view)
        {
            _eventAggregator = eventAggregator;
            View = view;
            View.DataContext = this;
            SearchCommand = new DelegateCommand(Search, CanSave);
            _eventAggregator.GetEvent<GetEntityTypesEvent>().Subscribe(SetEntityTypes);
        }

        private void SetEntityTypes(AkkaGetEntityTypesResponse message)
        {
            EntityTypes = message.EntityTypes;
        }

        private bool CanSave()
        {
            return true;
        }

        private void Search()
        {
            _eventAggregator.GetEvent<SearchEvent>().Publish(new Tuple<string, string>(SelectedEntityType, SearchText));
        }

        public IView View { get; set; }

        public string SearchText { get; set; }

        public List<string> EntityTypes { get; set; }
        public string SelectedEntityType { get; set; }
    }
}
