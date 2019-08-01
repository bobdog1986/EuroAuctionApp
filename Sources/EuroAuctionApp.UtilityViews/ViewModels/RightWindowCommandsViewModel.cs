using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EuroAuctionApp.Infra.Base;
using EuroAuctionApp.Infra.Interfaces;

namespace EuroAuctionApp.UtilityViews.ViewModels
{
    public class RightWindowCommandsViewModel:ViewModelBase
    {
        private IAppAssemblyInfo appAssemblyInfo;
        public RightWindowCommandsViewModel()
        {
            appAssemblyInfo = Resolve<IAppAssemblyInfo>();

            InitDisplay();
        }

        private string version;
        public string Version
        {
            get { return version; }
            set { SetProperty(ref version, value); }
        }

        private void InitDisplay()
        {
            Version = appAssemblyInfo.Version;
        }
    }
}
