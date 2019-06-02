using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EuroAuctionApp.Infra.Base;
using System.Reflection;
using EuroAuctionApp.Infra.Interfaces;

namespace EuroAuctionApp.UtilityViews.ViewModels
{
    public class AboutViewModel:ViewModelBase
    {
        private readonly IAppAssemblyInfo appAssemblyInfo;

        public AboutViewModel(IAppAssemblyInfo appAssemblyInfo)
        {
            this.appAssemblyInfo = appAssemblyInfo;
            GetMainAssemblyInfo();
        }

        private void GetMainAssemblyInfo()
        {
            try
            {
                Product = appAssemblyInfo.Product;
                Copyright = appAssemblyInfo.Copyright;
                Version = appAssemblyInfo.Version;
                Description = appAssemblyInfo.Description;
            }
            catch
            {
                ;
            }
        }

        private string appName;
        public string AppName
        {
            get { return appName; }
            set { SetProperty(ref appName, value); }
        }

        private string product;
        public string Product
        {
            get { return product; }
            set { SetProperty(ref product, value); }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        private string copyright;
        public string Copyright
        {
            get { return copyright; }
            set { SetProperty(ref copyright, value); }
        }

        private string version;
        public string Version
        {
            get { return version; }
            set { SetProperty(ref version, value); }
        }
    }
}
