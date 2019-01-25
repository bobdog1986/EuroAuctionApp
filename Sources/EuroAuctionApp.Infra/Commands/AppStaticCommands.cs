using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;

namespace EuroAuctionApp.Infra.Commands
{
    public static class AppStaticCommands
    {
        public static CompositeCommand ShowFlyoutCommand = new CompositeCommand();

        public static CompositeCommand MainRegionNavigateCommand = new CompositeCommand();

        public static CompositeCommand BrowseOnWebCommand = new CompositeCommand();
    }

    public interface IAppStaticCommands
    {
        CompositeCommand ShowFlyoutCommand { get; }

        CompositeCommand MainRegionNavigateCommand { get; }

        CompositeCommand BrowseOnWebCommand { get; }
    }

    public class AppStaticCommandsProxy : IAppStaticCommands
    {
        public CompositeCommand ShowFlyoutCommand => AppStaticCommands.ShowFlyoutCommand;

        public CompositeCommand MainRegionNavigateCommand => AppStaticCommands.MainRegionNavigateCommand;

        public CompositeCommand BrowseOnWebCommand => AppStaticCommands.BrowseOnWebCommand;
    }
}
