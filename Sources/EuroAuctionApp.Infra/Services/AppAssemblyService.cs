using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EuroAuctionApp.Infra.Interfaces;
using System.Reflection;

namespace EuroAuctionApp.Infra.Services
{
    public class AppAssemblyService:IAppAssemblyInfo
    {
        private static readonly string appName = "EuroAuctionApp";

        public string AppName
        {
            get
            {
                return appName;
            }
        }

        public string Product
        {
            get
            {
                return Assembly.Load(appName).GetCustomAttribute<AssemblyProductAttribute>().Product;
            }
        }

        public string Copyright
        {
            get
            {
                return Assembly.Load(appName).GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
            }
        }

        public string Version
        {
            get
            {
                return Assembly.Load(appName).GetCustomAttribute<AssemblyFileVersionAttribute>().Version;

            }
        }

        public string Description
        {
            get
            {
                return Assembly.Load(appName).GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
            }
        }
    }
}
