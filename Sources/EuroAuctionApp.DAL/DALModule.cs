using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EuroAuctionApp.Infra.Base;
using Prism.Ioc;
using EuroAuctionApp.DAL.Services;
using EuroAuctionApp.DAL.Interfaces;

namespace EuroAuctionApp.DAL
{
    public class DALModule:ModuleBase
    {
        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IAuctionRepository>(new AuctionRepository());
        }

        public override void OnInitialized(IContainerProvider containerProvider)
        {
            LogInfo("DALModule.OnInitialized()");
        }

    }
}
