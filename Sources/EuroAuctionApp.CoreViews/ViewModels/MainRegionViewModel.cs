using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EuroAuctionApp.Infra.Base;
using EuroAuctionApp.Infra.Events;
using Enterwell.Clients.Wpf.Notifications;

namespace EuroAuctionApp.CoreViews.ViewModels
{
    public class MainRegionViewModel:ViewModelBase
    {
        public MainRegionViewModel()
        {
            LogInfo("FileOperationViewModel");

            InitCommands();
            InitDisplay();
            SubscribeEvents();
        }

        private void InitCommands()
        {

        }

        private void InitDisplay()
        {

        }

        private void SubscribeEvents()
        {
            //EventAggregator.GetEvent<NotificationInfoEvent>().Subscribe((info) => OnNotificationInfoHandler(info));
        }

        private void OnNotificationInfoHandler(string info)
        {
            this.Manager
                .CreateMessage()
                .Accent("Blue")
                .Background("Gray")
                .HasBadge("Info")
                .HasMessage(info)
                .Dismiss().WithButton("OK", button => { })
                .Queue();
        }

        public INotificationMessageManager Manager { get; } = new NotificationMessageManager();

    }
}
