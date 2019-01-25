using EuroAuctionApp.Infra.Base;
using EuroAuctionApp.Infra.Constants;
using EuroAuctionApp.UtilityViews.Views;
using Prism.Ioc;

namespace EuroAuctionApp.UtilityViews
{
    public class UtilityViewsModule : ModuleBase
    {
        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            RegionManager.RegisterViewWithRegion(RegionNames.LeftWindowCommandsRegion, typeof(LeftWindowCommandsView));
            RegionManager.RegisterViewWithRegion(RegionNames.RightWindowCommandsRegion, typeof(RightWindowCommandsView));
            RegionManager.RegisterViewWithRegion(RegionNames.StatusBarRegion, typeof(StatusBarView));
            RegionManager.RegisterViewWithRegion(RegionNames.MenuRegion, typeof(MenuView));
            containerRegistry.RegisterForNavigation<AboutView>();
            containerRegistry.RegisterForNavigation<HelpView>();
        }

        public override void OnInitialized(IContainerProvider containerProvider)
        {
            //
        }
    }
}