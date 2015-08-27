using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDB.Client.Prism.Core;
using WebDB.Client.Prism.Modules.EntityGrid.ViewModels;
using WebDB.Client.Prism.Modules.EntityGrid.Views;
using WebDB.Messages;

namespace WebDB.Client.Prism.Modules.EntityGrid.Services
{
    public class EntitySearchResultService
    {
        private IUnityContainer _container;
        private IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;
        public EntitySearchResultService(IUnityContainer container,
            IEventAggregator eventAggregator, 
            IRegionManager regionManager)
        {
            _container = container;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

            _eventAggregator.GetEvent<SearchResultsEvent>().Subscribe(HandleSearchResults,
                 ThreadOption.UIThread,
                 false);
        }
        private void HandleSearchResults(AkkaSearchResults results)
        {
            _regionManager.RegisterViewWithRegion("ChildRegion", typeof(RelatedEntitiesView));

            IRegion region = _regionManager.Regions[RegionNames.ContentRegion];
            var vm = _container.Resolve<IEntityGridViewModel>();
            vm.SetItems(results.Results);
            vm.SetViewName(results.EntityType);
            
            region.Add(vm.View, null, true);
            

            //IRegion childRegion = regionManagerForChildren.Regions["ChildRegion"];
            //childRegion.Add(vm.View);
        }
    }
}
