using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EuroAuctionApp.Infra.Base;
using EuroAuctionApp.Infra.Constants;
using EuroAuctionApp.Infra.Interfaces;
using EuroAuctionApp.Infra.Events;
using Prism.Commands;
using EuroAuctionApp.UtilityViews.Views;
using Prism.Unity;

namespace EuroAuctionApp.UtilityViews.ViewModels
{
    public class MenuViewModel:ViewModelBase
    {
        private readonly ILocalizerService localizerService;

        private static readonly string aboutKey = "About";
        private static readonly string helpKey = "Help";

        public MenuViewModel()
        {
            MainPageCommand = new DelegateCommand(ClickMainPageMenu);
            AboutCommand = new DelegateCommand(ClickAboutMenu);
            HelpCommand = new DelegateCommand(ClickHelpMenu);

            localizerService = Resolve<ILocalizerService>();
        }

        public DelegateCommand MainPageCommand { get; private set; }
        public DelegateCommand AboutCommand { get; private set; }
        public DelegateCommand HelpCommand { get; private set; }

        private void ClickMainPageMenu()
        {
            RegionManager.RequestNavigate(RegionNames.MainRegion, ViewNames.MainRegionView);
        }

        private void ClickAboutMenu()
        {

        }

        private void ClickHelpMenu()
        {
 
        }
    }
}
