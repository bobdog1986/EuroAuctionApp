using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EuroAuctionApp.Infra.Base;
using Prism.Ioc;
using EuroAuctionApp.CSV.Interfaces;
using EuroAuctionApp.CSV.Services;

namespace EuroAuctionApp.CSV
{
    public class CSVModule : ModuleBase
    {
        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var service = Resolve<CsvService>();
            containerRegistry.RegisterInstance<ICsvService>(service);
        }

        public override void OnInitialized(IContainerProvider containerProvider)
        {
            LogInfo("CSVModule.OnInitialized()");
        }
    }
}
