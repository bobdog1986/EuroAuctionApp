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
using EuroAuctionApp.Infra.Interfaces;
using EuroAuctionApp.Infra.Constants;

namespace EuroAuctionApp.UtilityViews.Views
{
    /// <summary>
    /// Interaction logic for ShellSettingsFlyout.xaml
    /// </summary>
    public partial class ShellSettingsFlyoutView : Flyout, IFlyoutView
    {
        public ShellSettingsFlyoutView()
        {
            InitializeComponent();
        }

        public string FlyoutName
        {
            get
            {
                return FlyoutNames.ShellSettingsFlyout;
            }
        }
    }
}
