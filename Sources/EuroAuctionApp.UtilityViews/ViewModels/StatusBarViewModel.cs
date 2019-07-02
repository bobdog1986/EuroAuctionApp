using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EuroAuctionApp.Infra.Base;
using EuroAuctionApp.Infra.Events;

namespace EuroAuctionApp.UtilityViews.ViewModels
{
    public class StatusBarViewModel:ViewModelBase
    {
        public StatusBarViewModel()
        {
            EventAggregator.GetEvent<StatusMessageEvent>().Subscribe(OnStatusMessageReceived);
        }

        private void OnStatusMessageReceived(string message)
        {
            this.StatusBarMessage = DateTime.Now.ToString() + " : " + message;
        }

        private string statusBarMessage;
        public string StatusBarMessage
        {
            get { return statusBarMessage; }
            set { SetProperty(ref statusBarMessage, value); }
        }
    }
}
