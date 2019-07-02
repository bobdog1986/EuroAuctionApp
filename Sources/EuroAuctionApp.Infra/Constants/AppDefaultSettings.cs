using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroAuctionApp.Infra.Constants
{
    public static class AppDefaultSettings
    {
        public static Dictionary<string, object> ValuePairs;

        static AppDefaultSettings()
        {
            ValuePairs = new Dictionary<string, object>()
            {
                {KeyNames.AccentColorKey,"Cyan" },
                {KeyNames.LanguageColorKey,"zh-CN" },

                {KeyNames.LastQuoteFolderKey,"" },
                {KeyNames.BackupPathKey,@"D:\EuroAuctionApp Data" },

                //profit
                {KeyNames.UpProfit3ColorKey,"DarkGreen" },
                {KeyNames.UpProfit2ColorKey,"Green" },
                {KeyNames.UpProfitColorKey,"LightGreen" },
                {KeyNames.NormalProfitColorKey,"White" },
                {KeyNames.DownProfitColorKey,"Pink" },
                {KeyNames.DownProfit2ColorKey,"Red" },
                {KeyNames.DownProfit3ColorKey,"DarkRed" },

                {KeyNames.UpProfit3ValueKey,0.8 },
                {KeyNames.UpProfit2ValueKey,0.4 },
                {KeyNames.UpProfitValueKey,0.2 },
                {KeyNames.DownProfitValueKey,-0.2 },
                {KeyNames.DownProfit2ValueKey,-0.4 },
                {KeyNames.DownProfit3ValueKey,-0.8 },

                //auction
                {KeyNames.UpAuction3ColorKey,"DarkGreen" },
                {KeyNames.UpAuction2ColorKey,"Green" },
                {KeyNames.UpAuctionColorKey,"LightGreen" },
                {KeyNames.NormalAuctionColorKey,"White" },
                {KeyNames.DownAuctionColorKey,"Pink" },
                {KeyNames.DownAuction2ColorKey,"Red" },
                {KeyNames.DownAuction3ColorKey,"DarkRed" },

                {KeyNames.UpAuction3ValueKey,2.0 },
                {KeyNames.UpAuction2ValueKey,1.6 },
                {KeyNames.UpAuctionValueKey,0.8 },
                {KeyNames.DownAuctionValueKey,-0.8 },
                {KeyNames.DownAuction2ValueKey,-1.6 },
                {KeyNames.DownAuction3ValueKey,-2.0 },

            };
        }
    }
}
