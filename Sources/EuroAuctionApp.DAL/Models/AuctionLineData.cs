using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace EuroAuctionApp.DAL.Models
{
    public class AuctionLineData
    {
        [Name("Symbol")]
        public string Symbol { get; set; }

        [Name("Market")]
        public string Market { get; set; }

        [Name("Last5Price")]
        [Default(0)]
        public double Last5Price { get; set; }

        [Name("ClosePrice")]
        [Default(0)]
        public double ClosePrice { get; set; }

        [Name("AuctionPrice")]
        [Default(0)]
        public double AuctionPrice { get; set; }

        [Name("TotalVolumeAt25")]
        [Default(0)]
        public int TotalVolumeAt25 { get; set; }

        [Name("TotalVolumeAtClose")]
        [Default(0)]
        public int TotalVolumeAtClose { get; set; }

        [Name("TotalVolumeAtAuction")]
        [Default(0)]
        public int TotalVolumeAtAuction { get; set; }

        [Name("VolumeInLast5")]
        [Default(0)]
        public int VolumeInLast5 { get; set; }

        [Name("VolumeInAuction")]
        [Default(0)]
        public int VolumeInAuction { get; set; }

        [Name("CloseProfitInPercent")]
        [Default(0)]
        public double CloseProfitInPercent { get; set; }

        [Name("CloseProfitInBP")]
        [Default(0)]
        public double CloseProfitInBP { get; set; }

        [Name("Last5ProfitInPercent")]
        [Default(0)]
        public double Last5ProfitInPercent { get; set; }

        [Name("Last5ProfitInBP")]
        [Default(0)]
        public double Last5ProfitInBP { get; set; }

        [Name("AvgProfitInPercent")]
        [Default(0)]
        public double AvgProfitInPercent { get; set; }

        [Name("AvgProfitInBP")]
        [Default(0)]
        public double AvgProfitInBP { get; set; }

        [Name("AuctionVolumePercent")]
        [Default(0)]
        public double AuctionVolumePercent { get; set; }

        [Name("Last5VolumePercent")]
        [Default(0)]
        public double Last5VolumePercent { get; set; }
    }
}
