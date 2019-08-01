using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EuroAuctionApp.Infra.Constants;
using EuroAuctionApp.CoreViews.Helpers;
using System.Windows.Data;

namespace EuroAuctionApp.CoreViews.Converters
{
    public class AuctionToColorConverter : IValueConverter
    {
        private static double upAuction3;
        private static double upAuction2;
        private static double upAuction;
        private static double downAuction;
        private static double downAuction2;
        private static double downAuction3;

        private static string upColor3;
        private static string upColor2;
        private static string upColor;
        private static string normalColor;
        private static string downColor;
        private static string downColor2;
        private static string downColor3;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                double Auction = double.Parse(value.ToString());

                upAuction3 = (double)ColorSettingHelper.ValuePairs[KeyNames.UpAuction3ValueKey];
                upAuction2 = (double)ColorSettingHelper.ValuePairs[KeyNames.UpAuction2ValueKey];
                upAuction = (double)ColorSettingHelper.ValuePairs[KeyNames.UpAuctionValueKey];
                downAuction = (double)ColorSettingHelper.ValuePairs[KeyNames.DownAuctionValueKey];
                downAuction2 = (double)ColorSettingHelper.ValuePairs[KeyNames.DownAuction2ValueKey];
                downAuction3 = (double)ColorSettingHelper.ValuePairs[KeyNames.DownAuction3ValueKey];

                upColor3 = (string)ColorSettingHelper.ValuePairs[KeyNames.UpAuction3ColorKey];
                upColor2 = (string)ColorSettingHelper.ValuePairs[KeyNames.UpAuction2ColorKey];
                upColor = (string)ColorSettingHelper.ValuePairs[KeyNames.UpAuctionColorKey];
                normalColor = (string)ColorSettingHelper.ValuePairs[KeyNames.NormalAuctionColorKey];
                downColor = (string)ColorSettingHelper.ValuePairs[KeyNames.DownAuctionColorKey];
                downColor2 = (string)ColorSettingHelper.ValuePairs[KeyNames.DownAuction2ColorKey];
                downColor3 = (string)ColorSettingHelper.ValuePairs[KeyNames.DownAuction3ColorKey];

                if (Auction >= upAuction3)
                {
                    return upColor3;
                }
                else if (Auction >= upAuction2)
                {
                    return upColor2;
                }
                else if (Auction > upAuction)
                {
                    return upColor;
                }
                else if (Auction <= downAuction3)
                {
                    return downColor3;
                }
                else if (Auction <= downAuction2)
                {
                    return downColor2;
                }
                else if (Auction <= downAuction)
                {
                    return downColor;
                }
                else
                {
                    return normalColor;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return "";
        }
    }
}