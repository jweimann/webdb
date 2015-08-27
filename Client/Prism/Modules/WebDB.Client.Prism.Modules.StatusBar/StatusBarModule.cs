using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using WebDB.Client.Prism.Modules.StatusBar.Views;
using WebDB.Client.Prism.Modules.StatusBar.ViewModels;
using WebDB.Client.Prism.Core;

namespace WebDB.Client.Prism.Modules.StatusBar
{
    public class StatusBarModule : IModule
    {
        IUnityContainer _container;
        IRegionManager _regionManager;

        public StatusBarModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }
        public void Initialize()
        {
            _container.RegisterType<IStatusBarView, StatusBarView>();
            _container.RegisterType<IStatusBarViewModel, StatusBarViewModel>();

            var vm = _container.Resolve<IStatusBarViewModel>();
            vm.LastMessage = "No Messages...";
            IRegion region = _regionManager.Regions[RegionNames.StatusbarRegion];
            region.Add(vm.View);
        }
    }
}
