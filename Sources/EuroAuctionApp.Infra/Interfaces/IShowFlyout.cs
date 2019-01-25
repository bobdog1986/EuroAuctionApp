using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroAuctionApp.Infra.Interfaces
{
    public interface IShowFlyout
    {
        void ShowFlyout(string flyoutName);

        bool CanShowFlyout(string flyoutName);

        void Raise(string flyoutName);
    }
}
