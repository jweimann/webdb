using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;
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
using WebDB.Client.Prism.Modules.EntityGrid.Services;
using WebDB.Client.Prism.Modules.EntityGrid.ViewModels;
using WebDB.Client.Prism.Modules.EntityGrid.Views;

namespace WebDB.Client.Prism.Modules.EntityGrid
{
    public class EntityGridModule : IModule
    {
        private IUnityContainer _container;
        private IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;

        public EntityGridModule(IUnityContainer contianer, 
            IEventAggregator eventAggregator,
            IRegionManager regionManager)
        {
            _container = contianer;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<IEntityGridViewModel, EntityGridViewModel>();
            _container.RegisterType<IEntityGridView, EntityGridView>();
            _container.RegisterType<IRelatedEntitiesView, RelatedEntitiesView>();

            _container.RegisterType<EntitySearchResultService>(new ContainerControlledLifetimeManager());

            var vm = _container.Resolve<IEntityGridViewModel>();
            _container.Resolve<EntitySearchResultService>();

            //IRegion region = _regionManager.Regions[RegionNames.ContentRegion];
            //vm.SetViewName("Party");
            //region.Add(vm.View, null, true);
            //region.Activate(vm.View);
            //vm.ViewName = "Confused2";
        }
       
    }
}
