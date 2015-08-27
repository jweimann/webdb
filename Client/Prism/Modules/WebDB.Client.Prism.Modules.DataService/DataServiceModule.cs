using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using WebDB.Client.Prism.Interfaces;

namespace WebDB.Client.Prism.Modules.DataService
{
    public class DataServiceModule : IModule
    {
        IUnityContainer _container;
        IRegionManager _regionManager;

        IWebDBRemoteDataService _service;

        public DataServiceModule(IUnityContainer contianer, IRegionManager regionManager)
        {
            _container = contianer;
            _regionManager = regionManager;
        }
        public void Initialize()
        {
            _container.RegisterType<IWebDBRemoteDataService, WebDBRemoteDataService>(new ContainerControlledLifetimeManager());
            _service = _container.Resolve<IWebDBRemoteDataService>();
        }
    }
}