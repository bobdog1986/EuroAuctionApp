using CsvHelper.Configuration.Attributes;

namespace EuroAuctionApp.DAL.Models
{
    public class QuoteLineData
    {
        [Name("Symbol")]
        public string Symbol { get; set; }

        [Name("Last Price")]
        [Default(0)]
        public double LastPrice { get; set; }

        [Name("Volume")]
        [Default(0)]
        public int Volume { get; set; }
    }
}