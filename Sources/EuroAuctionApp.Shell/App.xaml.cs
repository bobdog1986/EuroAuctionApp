using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Prism.Unity;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using EuroAuctionApp.Infra.Constants;
using EuroAuctionApp.UtilityViews.Views;
using EuroAuctionApp.Infra.Services;
using EuroAuctionApp.Infra.Interfaces;
using EuroAuctionApp.Infra.Commands;

namespace EuroAuctionApp.Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    internal partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton(typeof(Window), typeof(MainWindow));
            containerRegistry.RegisterInstance<IAppStaticCommands>(Container.Resolve<AppStaticCommandsProxy>());
            containerRegistry.RegisterInstance<IShowFlyout>(Container.Resolve<ShowFlyoutService>());
            containerRegistry.RegisterInstance<ILocalizerService>(new LocalizerService());
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<Window>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);

            moduleCatalog.AddModule(typeof(UtilityViews.UtilityViewsModule));
            moduleCatalog.AddModule(typeof(CoreViews.CoreViewsModule));
        }

        protected override void InitializeShell(Window shell)
        {
            base.InitializeShell(shell);
            
            var regionManager = Container.Resolve<IRegionManager>();
            //register at first to make ui effect when start up
            regionManager.RegisterViewWithRegion(RegionNames.ShellSettingsFlyoutRegion, typeof(ShellSettingsFlyoutView));
        }

    }
}
