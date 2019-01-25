using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Modularity;
using Prism.Regions;
using CommonServiceLocator;
using Prism.Logging;
using Unity;
using Prism.Ioc;

namespace EuroAuctionApp.Infra.Base
{
    public abstract class ModuleBase : IModule
    {
        public ModuleBase()
        {
            this.UnityContainer = ServiceLocator.Current.GetInstance<IUnityContainer>();
            this.RegionManager = ServiceLocator.Current.GetInstance<IRegionManager>();
            this.logger = ServiceLocator.Current.GetInstance<ILoggerFacade>();
        }

        public abstract void OnInitialized(IContainerProvider containerProvider);

        public abstract void RegisterTypes(IContainerRegistry containerRegistry);

        public IUnityContainer UnityContainer { get; }

        public IRegionManager RegionManager { get; }

        private ILoggerFacade logger;

        public void LogInfo(string message)
        {
            logger.Log(message, Category.Info, Priority.Low);
        }

        public void LogWarn(string message)
        {
            logger.Log(message, Category.Warn, Priority.Low);
        }

        public void LogError(string message)
        {
            logger.Log(message, Category.Exception, Priority.Low);
        }

        public T Resolve<T>()
        {
            return UnityContainer.Resolve<T>();
        }
    }
}
