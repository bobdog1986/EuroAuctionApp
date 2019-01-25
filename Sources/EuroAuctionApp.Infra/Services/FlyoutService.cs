using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EuroAuctionApp.Infra.Interfaces;
using EuroAuctionApp.Infra.Commands;
using EuroAuctionApp.Infra.Constants;
using Prism.Regions;
using Prism.Commands;
using System.Windows.Input;
using MahApps.Metro.Controls;

namespace EuroAuctionApp.Infra.Services
{
    public class ShowFlyoutService:IShowFlyout
    {
        IRegionManager _regionManager;

        public ICommand ShowFlyoutCommand { get; private set; }

        public ShowFlyoutService(IRegionManager regionManager, IAppStaticCommands applicationCommands)
        {
            _regionManager = regionManager;

            ShowFlyoutCommand = new DelegateCommand<string>(ShowFlyout, CanShowFlyout);
            applicationCommands.ShowFlyoutCommand.RegisterCommand(ShowFlyoutCommand);
        }

        public void ShowFlyout(string flyoutName)
        {
            var region = _regionManager.Regions[RegionNames.ShellSettingsFlyoutRegion];

            if (region != null)
            {
                var flyout = region.Views.Where(v => v is IFlyoutView && ((IFlyoutView)v).FlyoutName.Equals(flyoutName)).FirstOrDefault() as Flyout;

                if (flyout != null)
                {
                    flyout.IsOpen = !flyout.IsOpen;
                }
            }
        }

        public bool CanShowFlyout(string flyoutName)
        {
            return true;
        }

        public void Raise(string flyoutName)
        {
            ShowFlyoutCommand.Execute(flyoutName);
        }
    }
}
