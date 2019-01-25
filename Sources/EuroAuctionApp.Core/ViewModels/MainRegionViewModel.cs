using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EuroAuctionApp.Infra.Base;
using EuroAuctionApp.Infra.Events;

namespace EuroAuctionApp.CoreViews.ViewModels
{
    public class MainRegionViewModel:ViewModelBase
    {
        public MainRegionViewModel()
        {
            PublishStatusMessage("MainRegionViewModel");
        }

        void PublishStatusMessage(string message)
        {
            EventAggregator.GetEvent<StatusMessageEvent>().Publish(message);
        }
    }
}
