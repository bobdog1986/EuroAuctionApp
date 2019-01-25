using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Regions;
using EuroAuctionApp.Infra.Interfaces;
using Prism.Commands;
using System.Windows.Input;
using EuroAuctionApp.Infra.Commands;

namespace EuroAuctionApp.Infra.Services
{
    public class BrowseOnWebService:IBrowseOnWeb
    {
        IRegionManager _regionManager;

        public ICommand BrowserOnWebCommand { get; private set; }

        public BrowseOnWebService(IRegionManager regionManager, IAppStaticCommands applicationCommands)
        {
            _regionManager = regionManager;

            BrowserOnWebCommand = new DelegateCommand<string>(Open, CanOpen);
            applicationCommands.BrowseOnWebCommand.RegisterCommand(BrowserOnWebCommand);
        }

        public void Open(string link)
        {
            System.Diagnostics.Process.Start(link);
        }

        public bool CanOpen(string link)
        {
            return !string.IsNullOrEmpty(link);
        }

        public void Raise(string link)
        {
            if (!string.IsNullOrEmpty(link))
            {
                BrowserOnWebCommand.Execute(link);
            }
        }
    }
}
