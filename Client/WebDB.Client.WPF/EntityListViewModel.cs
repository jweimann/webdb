using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using WebDB.Client.Core;
using WebDB.Common;
using WebDB.Model;

namespace WebDB.Client.WPF
{
    public class EntityListViewModel<TModel> where TModel : class, INotifyPropertyChanged, IModelObject
    {
        public bool Dirty { get; private set; }
        public ObservableCollection<TModel> Collection { get; private set; }
        private List<TModel> _updatedItems;

        public void SetCollection(List<TModel> collection)
        {
            Collection = new ObservableCollection<TModel>(collection);
            Collection.CollectionChanged += Collection_CollectionChanged;
            SetBindings();
        }

        private void Collection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //e.NewItems
        }

        private void SetBindings()
        {
            foreach (var issue in Collection)
            {
                issue.IsChanged = false;
                issue.PropertyChanged += Item_PropertyChanged;
            }
        }
        
        public async void Save()
        {
            _updatedItems = new List<TModel>();
            for (int i = 0; i < Collection.Count; i++)
            {
                var issue = Collection[i];
          
                if (issue.IsChanged)
                {
                    WebDBClient<TModel> client = new WebDBClient<TModel>();
                    long id = issue.GetId();

                    // Check if ID is set.  If not, it's a new item and we do a post instead of a put.
                    // Add the Post method to the client.
                    if (id > 0)
                        Collection[i] = await client.Put(issue);
                    else
                        Collection[i] = await client.Post(issue);

                    _updatedItems.Add(Collection[i]);
                }
            }
        }

        internal async void Undo()
        {
            WebDBClient<TModel> client = new WebDBClient<TModel>();
            foreach (var item in _updatedItems)
            {
                var index = Collection.IndexOf(Collection.FirstOrDefault(t => t.GetId() == item.GetId()));
                Collection[index] = await client.Undo(item);
            }
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
            //throw new NotImplementedException();
        }
    }
}
