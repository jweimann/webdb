using System;
using Microsoft.Practices.Prism.PubSubEvents;
using WebDB.Client.Prism.Core;
using WebDB.Client.Prism.Modules.StatusBar.Views;
using PropertyChanged;
using System.ComponentModel;
using Microsoft.Practices.Prism.Mvvm;
using WebDB.Messages;

namespace WebDB.Client.Prism.Modules.StatusBar.ViewModels
{
    [ImplementPropertyChanged]
    public class StatusBarViewModel : IStatusBarViewModel, INotifyPropertyChanged
    {
        private IEventAggregator _eventAggregator;

        public event PropertyChangedEventHandler PropertyChanged;

        public StatusBarViewModel(IStatusBarView view, IEventAggregator eventAggregator)
        {
            LastMessage = "Not Set";
            View = view;
            View.DataContext = this;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<EntityUpdatedEvent>().Subscribe(EntityUpdated);
        }

        private void EntityUpdated(UpdateEntityRequest obj)
        {
            LastMessage = $"{DateTime.Now.ToString()}: Saved";
        }

        private void EntityUpdated(string obj)
        {
            LastMessage = obj;
        }

        public IView View { get; set; }

        public string LastMessage { get; set; }

    }
}
