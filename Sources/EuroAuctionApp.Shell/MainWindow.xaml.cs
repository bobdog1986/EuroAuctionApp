using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using Prism.Regions;
using EuroAuctionApp.Infra.Constants;
using EuroAuctionApp.Infra.Helpers;

namespace EuroAuctionApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow(IRegionManager regionManager)
        {
            InitializeComponent();

            this.SourceInitialized += OnSourceInitialized;

            // The RegionManager.RegionName attached property XAML-Declaration doesn't work for this scenario (object declarated outside the logical tree)
            // theses objects are not part of the logical tree, hence the parent that has the region manager to use (the Window) cannot be found using LogicalTreeHelper.FindParent 
            // therefore the regionManager is never found and cannot be assigned automatically by Prism.  This means we have to handle this ourselves
            if (regionManager != null)
            {
                SetRegionManager(regionManager, this.leftWindowCommandsRegion, RegionNames.LeftWindowCommandsRegion);
                SetRegionManager(regionManager, this.rightWindowCommandsRegion, RegionNames.RightWindowCommandsRegion);
                SetRegionManager(regionManager, this.flyoutsControlRegion, RegionNames.ShellSettingsFlyoutRegion);
            }

        }

        private void OnSourceInitialized(object sender, EventArgs e)
        { 
            this.Top = Settings.MainWindowTop;
            this.Left = Settings.MainWindowLeft;
            this.Height = Settings.MainWindowHeight>0? Settings.MainWindowHeight: SystemParameters.PrimaryScreenHeight;
            this.Width = Settings.MainWindowWidth > 0 ? Settings.MainWindowWidth : SystemParameters.PrimaryScreenWidth;
        }

        private void SetRegionManager(IRegionManager regionManager, DependencyObject regionTarget, string regionName)
        {
            RegionManager.SetRegionName(regionTarget, regionName);
            RegionManager.SetRegionManager(regionTarget, regionManager);
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.MainWindowTop = this.Top;
            Settings.MainWindowLeft= this.Left;
            Settings.MainWindowHeight = this.Height;
            Settings.MainWindowWidth = this.Width;
        }
    }
}
