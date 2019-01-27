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

        private void SetRegionManager(IRegionManager regionManager, DependencyObject regionTarget, string regionName)
        {
            RegionManager.SetRegionName(regionTarget, regionName);
            RegionManager.SetRegionManager(regionTarget, regionManager);
        }
    }
}
