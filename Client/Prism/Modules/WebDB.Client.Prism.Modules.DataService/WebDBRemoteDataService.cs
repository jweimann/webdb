using Microsoft.Practices.Prism.PubSubEvents;
using System.Collections.Generic;
using WebDB.Client.Prism.Interfaces;
using WebDB.Client.Prism.Core;
using System;
using System.Linq;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using WebDB.Model;
using WebDB.Client.Core;

namespace WebDB.Client.Prism.Modules.DataService
{
    // there should probably be diff services for diff stuff
    // this is starting as a search service.
    public class WebDBRemoteDataService : IWebDBRemoteDataService
    {
        private IEventAggregator _eventAggregator;
        private IRegionManager _regionManager;
        public WebDBRemoteDataService(IEventAggregator eventAggregator, 
            IRegionManager regionManager)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            //_eventAggregator.GetEvent<EntityUpdatedEvent>().Subscribe(EntityUpdated);
            _eventAggregator.GetEvent<SearchEvent>().Subscribe(HandleSearch);
        }

      

        private async void HandleSearch(Tuple<string,string> searchText)
        {
            try
            {
                WebDBClient<Party> client = new WebDBClient<Party>();
                var parties = await client.Get();
                List<object> result = new List<object>();
                foreach (var party in parties)
                    result.Add(party);
                //List<object> result = new List<object>()
                //{
                //    "SearchResult1",
                //    "SearchResult2",
                //    "SearchResult3"
                //};
                //_eventAggregator.GetEvent<SearchResults>().Publish(result);
            } catch (Exception ex)
            {

            }
        }

        
        public List<object> Get()
        {
            return new List<object>()
            {
                "Test1",
                "Test2"
            };
        }
    }
}
