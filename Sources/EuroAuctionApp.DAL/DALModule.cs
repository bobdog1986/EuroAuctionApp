using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EuroAuctionApp.Infra.Base;
using EuroAuctionApp.Infra.Constants;
using Prism.Ioc;
using EuroAuctionApp.DAL.Interfaces;
using EuroAuctionApp.DAL.Services;

namespace EuroAuctionApp.DAL
{
    public class DALModule:ModuleBase
    {
        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var service = Resolve<CsvService>();
            containerRegistry.RegisterInstance<ICsvService>(service);
        }

        public override void OnInitialized(IContainerProvider containerProvider)
        {
            LogInfo("DALModule.OnInitialized()");
        }
    }
}
