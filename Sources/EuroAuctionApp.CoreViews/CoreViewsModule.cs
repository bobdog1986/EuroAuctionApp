using EuroAuctionApp.Infra.Base;
using EuroAuctionApp.Infra.Constants;
using EuroAuctionApp.CoreViews.Views;
using Prism.Ioc;
using EuroAuctionApp.CoreViews.Helpers;

namespace EuroAuctionApp.CoreViews
{
    public class CoreViewsModule : ModuleBase
    {
        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            RegionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(MainRegionView));
            //RegionManager.RegisterViewWithRegion(RegionNames.RightWindowCommandsRegion, typeof(RightWindowCommandsView));
            //RegionManager.RegisterViewWithRegion(RegionNames.StatusBarRegion, typeof(StatusBarView));
            //RegionManager.RegisterViewWithRegion(RegionNames.MenuRegion, typeof(MenuView));
            //containerRegistry.RegisterForNavigation<AboutView>();
            //containerRegistry.RegisterForNavigation<HelpView>();
        }

        public override void OnInitialized(IContainerProvider containerProvider)
        {
            //
            //ColorSettingHelper.UpdateSettings();
        }
    }
}
