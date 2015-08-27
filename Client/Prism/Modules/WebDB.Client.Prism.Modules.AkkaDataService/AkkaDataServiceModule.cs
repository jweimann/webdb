using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDB.Client.Prism.Interfaces;

namespace WebDB.Client.Prism.Modules.AkkaDataService
{
    public class AkkaDataServiceModule : IModule
    {
        IUnityContainer _container;
        IRegionManager _regionManager;

        IAkkaDataService _service;

        public AkkaDataServiceModule(IUnityContainer contianer, IRegionManager regionManager)
        {
            _container = contianer;
            _regionManager = regionManager;
        }
        public void Initialize()
        {
            _container.RegisterType<IAkkaDataService, AkkaDataService>(new ContainerControlledLifetimeManager());
            _service = _container.Resolve<IAkkaDataService>();
        }
    }
}
