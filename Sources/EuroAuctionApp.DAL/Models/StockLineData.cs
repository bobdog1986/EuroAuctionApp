using CsvHelper.Configuration.Attributes;

namespace EuroAuctionApp.DAL.Models
{
    public class StockLineData
    {
        [Name("Symbol")]
        public string Symbol { get; set; }

        [Name("Last Price")]
        public string LastPrice { get; set; }

        [Name("Volume")]
        public string Volume { get; set; }
    }
}