using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using PropertyChanged;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WebDB.Client.Prism.Core;
using WebDB.Client.Prism.Modules.EntityGrid.Views;
using WebDB.Messages;
using WebDB.Model;
using WebDB.Common;

namespace WebDB.Client.Prism.Modules.EntityGrid.ViewModels
{
    [ImplementPropertyChanged]
    public class EntityGridViewModel : BindableBase, IEntityGridViewModel
    {
        private IEventAggregator _eventAggregator;

        public ObservableCollection<object> Items { get; set; }

        public IModelObject SelectedItem { get; set; }// = "None";
        public ICommand SelectItemCommand { get; set; }
        public ICommand OpenDetailsCommand { get; set; }
        public bool IsLoading { get; set; } = true;

        public EntityGridViewModel(IEntityGridView view, IEventAggregator eventAggregator)
        {
            Items = new ObservableCollection<object>()
            {
                "Item1",
                "Item2",
                "Item3"
            };

            ViewName = "Unknown";
            _eventAggregator = eventAggregator;
            View = view;
            View.DataContext = this;
            SaveCommand = new DelegateCommand(Save, CanSave);
            SelectItemCommand = new DelegateCommand<object[]>(SelectItem);
            OpenDetailsCommand = new DelegateCommand<object>(OpenDetails);

            _eventAggregator.GetEvent<NotifySubscribersOfEntityChangeEvent>().Subscribe(HandleEntityChangedEvent);
        }

        private void OpenDetails(object obj)
        {
            
        }

        private void HandleEntityChangedEvent(NotifySubscribersOfEntityChange message)
        {
            var modifiedId = message.EntityId;
            foreach(var item in Items)
            {
                ModelObjectBase modelObject = item as ModelObjectBase;
                if (modelObject.Id == modifiedId)
                {
                    modelObject.IsChanged = true;
                }
            }
        }

        private void SelectItem(object[] items)
        {
            SelectedItem = items?.FirstOrDefault() as IModelObject;
        }

        private bool CanSave()
        {
            return true;
        }

        public IView View { get; set; }

        public DelegateCommand SaveCommand { get; set; }

        public string ViewName { get; set; }

        private void Save()
        {
            foreach (var item in Items.Cast<IModelObject>().Where(t => t.IsChanged))
            {
                _eventAggregator.GetEvent<EntityUpdatedEvent>().Publish(new UpdateEntityRequest(item, ViewName));
            }
        }

        public void SetViewName(string viewName)
        {
            this.ViewName = viewName;
        }

        public void SetItems(List<object> results)
        {
            Items = new ObservableCollection<object>(results);

            //EntityGridView viewCast = (View as EntityGridView);

            //List<Issue> issues = new List<Issue>();
            //issues.AddRange(results.Cast<Issue>());
            //viewCast.dataGridControl.ItemsSource = Items;
            //viewCast.dataGridControl.ItemsSource = issues;// new List<Tuple<string, string>> { new Tuple<string, string>("C1", "C2") };

            //viewCast.devDataGrid.ItemsSource = Items;
            //foreach(var property in )
            //viewCast.dataGridControl.Columns.Add(new Xceed.Wpf.DataGrid.Column(fieldName, displayMemberBinding))

            foreach (var itemUncast in Items)//.Cast<IModelObject>())
            {
                IModelObject item = itemUncast as IModelObject;
                item.Relationships = new List<List<object>>();
                var type = item.GetType();

                var relationshipProperties = item.GetNavigationProperties();

                

                foreach (var relatedProperty in relationshipProperties)
                {
                    if (relatedProperty.PropertyType.IsGenericType && relatedProperty.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
                    {
                        var relatedItem = relatedProperty.GetValue(item);
                        //List<object> something = new List<object>(relatedItem);
                        var hash = relatedItem as IEnumerable;
                        var relatedItemList = new List<object>();// hash.ToList<PollIssue>();
                        foreach(var existing in hash)
                        {
                            relatedItemList.Add(existing);
                        }
                        //relatedItemList.Add(new PollIssue() { Id = 1 });
                        //relatedItemList.Add(new PollIssue() { Id = 2 });
                        //relatedItemList.Add(new PollIssue() { Id = 3 });
                        item.Relationships.Add(relatedItemList);
                    }
                }
                item.IsChanged = false;
            }

            SelectedItem = Items.FirstOrDefault() as IModelObject;

            IsLoading = false;
        }
    }
}