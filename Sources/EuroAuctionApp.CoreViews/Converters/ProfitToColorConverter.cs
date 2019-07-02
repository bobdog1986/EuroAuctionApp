using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using EuroAuctionApp.Infra.Helpers;
using EuroAuctionApp.Infra.Constants;
using EuroAuctionApp.CoreViews.Helpers;

namespace EuroAuctionApp.CoreViews.Converters
{
    public class ProfitToColorConverter:IValueConverter
    {
        private static double upProfit3;
        private static double upProfit2;
        private static double upProfit;
        private static double downProfit;
        private static double downProfit2;
        private static double downProfit3;

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
                double profit = double.Parse(value.ToString());


                upProfit3 = (double)ColorSettingHelper.ValuePairs[KeyNames.UpProfit3ValueKey];
                upProfit2 = (double)ColorSettingHelper.ValuePairs[KeyNames.UpProfit2ValueKey];
                upProfit = (double)ColorSettingHelper.ValuePairs[KeyNames.UpProfitValueKey];
                downProfit = (double)ColorSettingHelper.ValuePairs[KeyNames.DownProfitValueKey];
                downProfit2 = (double)ColorSettingHelper.ValuePairs[KeyNames.DownProfit2ValueKey];
                downProfit3 = (double)ColorSettingHelper.ValuePairs[KeyNames.DownProfit3ValueKey];

                upColor3 = (string)ColorSettingHelper.ValuePairs[KeyNames.UpProfit3ColorKey];
                upColor2 = (string)ColorSettingHelper.ValuePairs[KeyNames.UpProfit2ColorKey];
                upColor = (string)ColorSettingHelper.ValuePairs[KeyNames.UpProfitColorKey];
                normalColor = (string)ColorSettingHelper.ValuePairs[KeyNames.NormalProfitColorKey];
                downColor = (string)ColorSettingHelper.ValuePairs[KeyNames.DownProfitColorKey];
                downColor2 = (string)ColorSettingHelper.ValuePairs[KeyNames.DownProfit2ColorKey];
                downColor3 = (string)ColorSettingHelper.ValuePairs[KeyNames.DownProfit3ColorKey];

                if (profit >= upProfit3)
                {
                    return upColor3;
                }
                else if(profit >= upProfit2)
                {
                    return upColor2;
                }
                else if (profit > upProfit)
                {
                    return upColor;
                }
                else if (profit <= downProfit3)
                {
                    return downColor3;
                }
                else if (profit <= downProfit2)
                {
                    return downColor2;
                }
                else if (profit <= downProfit)
                {
                    return downColor;
                }
                else
                {
                    return normalColor;
                }
            }
            catch(Exception ex)
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
