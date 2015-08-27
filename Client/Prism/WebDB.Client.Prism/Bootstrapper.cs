using Microsoft.Practices.Prism.UnityExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Modularity;
using WebDB.Client.Prism.Modules.Toolbar;
using Microsoft.Practices.Prism.Regions;
using System.Windows.Controls;
using WebDB.Client.Prism.Core;
using WebDB.Client.Prism.Modules.EntityGrid;
using WebDB.Client.Prism.Modules.StatusBar;
using WebDB.Client.Prism.Modules.DataService;
using WebDB.Client.Prism.Modules.AkkaDataService;

namespace WebDB.Client.Prism
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }
        protected override void InitializeShell()
        {
            base.InitializeShell();

            App.Current.MainWindow = (Window)Shell;
            App.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            Type toolbarModuleType = typeof(ToolbarModule);
            ModuleCatalog.AddModule(new ModuleInfo()
            {
                ModuleName = toolbarModuleType.Name,
                ModuleType = toolbarModuleType.AssemblyQualifiedName,
                InitializationMode = InitializationMode.WhenAvailable
            });

            Type entityGridModuleType = typeof(EntityGridModule);
            ModuleCatalog.AddModule(new ModuleInfo()
            {
                ModuleName = entityGridModuleType.Name,
                ModuleType = entityGridModuleType.AssemblyQualifiedName,
                InitializationMode = InitializationMode.WhenAvailable
            });

            Type statusBarModuleType = typeof(StatusBarModule);
            ModuleCatalog.AddModule(new ModuleInfo()
            {
                ModuleName = statusBarModuleType.Name,
                ModuleType = statusBarModuleType.AssemblyQualifiedName,
                InitializationMode = InitializationMode.WhenAvailable
            });

            //Type dataServiceModuleType = typeof(DataServiceModule);
            //ModuleCatalog.AddModule(new ModuleInfo()
            //{
            //    ModuleName = dataServiceModuleType.Name,
            //    ModuleType = dataServiceModuleType.AssemblyQualifiedName,
            //    InitializationMode = InitializationMode.WhenAvailable
            //});

            Type akkaDataServiceModuleType = typeof(AkkaDataServiceModule);
            ModuleCatalog.AddModule(new ModuleInfo()
            {
                ModuleName = akkaDataServiceModuleType.Name,
                ModuleType = akkaDataServiceModuleType.AssemblyQualifiedName,
                InitializationMode = InitializationMode.WhenAvailable
            });
        }

        //protected override Microsoft.Practices.Prism.Modularity.IModuleCatalog CreateModuleCatalog()
        //{
        //    ModuleCatalog catalog = new ModuleCatalog();
        //    catalog.AddModule(typeof(ToolbarModule));
        //    return catalog;
        //}

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings();
            mappings.RegisterMapping(typeof(StackPanel), Container.Resolve<StackPanelRegionAdapter>());
            return mappings;
        }
    }
}
