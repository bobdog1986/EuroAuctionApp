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

        ///// <summary>
        ///// Display in percent, 1 equal 1% up
        ///// </summary>
        //[Name("Last Price / Close Price %")]
        //[Default(0)]
        //public double LastPriceClosePriceRatio { get; set; }

        //[Name("ClosePrice")]
        //[Default(0)]
        //public double ClosePrice { get; set; }

        //[Name("High Price")]
        //[Default(0)]
        //public double HighPrice { get; set; }

        //[Name("Low")]
        //[Default(0)]
        //public double LowPrice { get; set; }

        //[Name("Ask Price - Bid Price")]
        //[Default(0)]
        //public double AskPriceBidPriceDiff { get; set; }

        //[Name("Bid Price")]
        //[Default(0)]
        //public double BidPrice { get; set; }

        //[Name("Bid Size")]
        //[Default(0)]
        //public int BidSize { get; set; }

        //[Name("Ask Price")]
        //[Default(0)]
        //public double AskPrice { get; set; }

        //[Name("Ask Size")]
        //[Default(0)]
        //public int AskSize { get; set; }

        ///// <summary>
        ///// Display in percent, 1 equal 1% up
        ///// </summary>
        //[Name("Open Price / Close Price %")]
        //[Default(0)]
        //public double OpenPriceClosePriceRatio { get; set; }

        //[Name("OpenPrice")]
        //[Default(0)]
        //public double OpenPrice { get; set; }

        //[Name("AlertName")]
        //[Default("")]
        //public string AlertName { get; set; }

        //[Name("High Price - Low Price")]
        //[Default(0)]
        //public double HighPriceLowPriceDiff { get; set; }

        //[Name("Last Price - Close Price")]
        //[Default(0)]
        //public double LastPriceClosePriceDiff { get; set; }

        //[Name("Last Price - High Price")]
        //[Default(0)]
        //public double LastPriceHighPriceDiff { get; set; }

        //[Name("Last Price - Low Price")]
        //[Default(0)]
        //public double LastPriceLowPriceDiff { get; set; }

        //[Name("Last Price - Open Price")]
        //[Default(0)]
        //public double LastPriceOpenPriceDiff { get; set; }

        ///// <summary>
        ///// Display in percent, 1 equal 1% up
        ///// </summary>
        //[Name("Last Price / High Price %")]
        //[Default(0)]
        //public double LastPriceHighPriceRatio { get; set; }

        ///// <summary>
        ///// Display in percent, 1 equal 1% up
        ///// </summary>
        //[Name("Last Price / Low Price %")]
        //[Default(0)]
        //public double LastPriceLowPriceRatio { get; set; }

        //[Name("Market")]
        //public string Market { get; set; }

        //[Name("Open Price - Close Price")]
        //[Default(0)]
        //public double OpenPriceClosePriceDiff { get; set; }

        ///// <summary>
        ///// Display in percent, 1 equal 1% up
        ///// </summary>
        //[Name("Open Price / Last Price %")]
        //[Default(0)]
        //public double OpenPriceLastPriceRatio { get; set; }

        //[Name("Theoretical Auction Price")]
        //[Default(0)]
        //public double TheoreticalAuctionPrice { get; set; }

        //[Name("Theoretical Auction Type")]
        //[Default(0)]
        //public string TheoreticalAuctionType { get; set; }

        //[Name("Theoretical Auction Volume")]
        //[Default(0)]
        //public int TheoreticalAuctionVolume { get; set; }

        //[Name("Timestamp")]
        //public string Timestamp { get; set; }
    }
}