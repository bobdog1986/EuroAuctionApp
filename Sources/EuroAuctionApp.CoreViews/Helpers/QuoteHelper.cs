using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroAuctionApp.CoreViews.Helpers
{
    public static class FormatterHelper
    {
        /// <summary>
        /// "QuoteBoardExport_"
        /// </summary>
        public static readonly string QuotePrefix = "QuoteBoardExport_";

        /// <summary>
        /// "yyyy-MM-dd_HH-mm-ss", 2019-01-26_00-32-37
        /// </summary>
        public static readonly string QuoteTimeFormat = "yyyy-MM-dd_HH-mm-ss";

        public static readonly string ShortDateFormat = "yyyy-MM-dd";
    }
}
