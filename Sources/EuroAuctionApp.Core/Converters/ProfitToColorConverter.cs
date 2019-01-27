using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EuroAuctionApp.CoreViews.Converters
{
    public class ProfitToColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double profit = double.Parse(value.ToString());

            if (profit >= 0.5)
            {
                return "Green";
            }
            else if (profit > 0.1)
            {
                return "LightGreen";
            }
            else if(profit <= -0.5)
            {
                return "Red";
            }
            else if (profit <= -0.1)
            {
                return "LightPink";

            }
            else
            {
                return string.Empty;
            }

            //return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return "";
        }
    }
}
