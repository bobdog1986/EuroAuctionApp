using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EuroAuctionApp.CoreViews.Converters
{
    public class DoubleToKMConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double data = double.Parse(value.ToString());
            return data>1000000? data.ToString("#,##0,,M") : data.ToString("#,##0,K");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return "";
        }
    }
}
