using CsvHelper.Configuration.Attributes;

namespace EuroAuctionApp.CSV.Models
{
    public class QuoteLineData
    {
        [Name("Symbol")]
        public string Symbol { get; set; }

        [Name("Last Price")]
        [Default(double.NaN)]
        public double LastPrice { get; set; }

        [Name("Volume")]
        [Default(0)]
        public int Volume { get; set; }

        //[Name("ClosePrice")]
        //[Default(0)]
        //public double ClosePrice { get; set; }

        //[Name("Open Price")]
        //[Default(0)]
        //public double OpenPrice { get; set; }

        //[Name("High Price")]
        //[Default(0)]
        //public double HighPrice { get; set; }

        //[Name("Low")]
        //[Default(0)]
        //public double LowPrice { get; set; }

        //public double BidPrice { get; set; }

        //public int BidSize { get; set; }
     
        //public double AskPrice { get; set; }

        //public int AskSize { get; set; }
    }
}