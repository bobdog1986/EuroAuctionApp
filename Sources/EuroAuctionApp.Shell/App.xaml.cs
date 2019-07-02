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
using Prism.Logging;
using EuroAuctionApp.Infra.Logging;
using Akavache;
using MahApps.Metro.Controls.Dialogs;

namespace EuroAuctionApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    internal partial class App : PrismApplication
    {
        static App()
        {
            BlobCache.ApplicationName = KeyNames.AppName;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton(typeof(Window), typeof(MainWindow));            
            containerRegistry.RegisterInstance<IAppStaticCommands>(Container.Resolve<AppStaticCommandsProxy>());
            containerRegistry.RegisterInstance<IShowFlyout>(Container.Resolve<ShowFlyoutService>());
            containerRegistry.RegisterInstance<ILocalizerService>(new LocalizerService());
            containerRegistry.RegisterInstance<ILoggerFacade>(Container.Resolve<NLogger>());
            containerRegistry.RegisterInstance<IAppAssemblyInfo>(Container.Resolve<AppAssemblyService>());
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<Window>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);

            moduleCatalog.AddModule(typeof(CSV.CSVModule));
            moduleCatalog.AddModule(typeof(DAL.DALModule));
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

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            BlobCache.Shutdown().Wait();
        }
    }
}
