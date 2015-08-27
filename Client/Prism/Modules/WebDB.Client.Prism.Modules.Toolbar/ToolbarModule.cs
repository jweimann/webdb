using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDB.Client.Prism.Core;
using WebDB.Client.Prism.Modules.Toolbar.ViewModels;
using WebDB.Client.Prism.Modules.Toolbar.Views;

namespace WebDB.Client.Prism.Modules.Toolbar
{
    public class ToolbarModule : IModule
    {
        IUnityContainer _container;
        IRegionManager _regionManager;

        public ToolbarModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<IToolbarView, ToolbarView>();
            _container.RegisterType<IToolbarViewModel, ToolbarViewModel>();
            _container.RegisterType<IContentView, ContentViewA>();
            _container.RegisterType<IContentViewViewModel, ContentAViewViewModel>();

            //IRegion region = _regionManager.Regions["ToolbarRegion"];
            //region.Add(_container.Resolve<ToolbarView>());
            //region.Add(_container.Resolve<ToolbarView>());
            //region.Add(_container.Resolve<ToolbarView>());
            //region.Add(_container.Resolve<ToolbarView>());

            var toolbarVm = _container.Resolve<IToolbarViewModel>();
            toolbarVm.TitleMessage = "Save All";
            IRegion toolbarRegion = _regionManager.Regions[RegionNames.ToolbarRegion];
            toolbarRegion.Add(toolbarVm.View);
            //_regionManager.RegisterViewWithRegion(RegionNames.ToolbarRegion, typeof(IToolbarViewModel));
            //_regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(IContentView));

            /*
            var vm = _container.Resolve<IContentViewViewModel>();
            vm.Message = "First View";
            IRegion region = _regionManager.Regions[RegionNames.ContentRegion];
            region.Add(vm.View);

            var vm2 = _container.Resolve<IContentViewViewModel>();
            vm2.Message = "Second View";
            

            //region.Deactivate(vm.View);
            region.Add(vm2.View);
            */
        }
    }
}
