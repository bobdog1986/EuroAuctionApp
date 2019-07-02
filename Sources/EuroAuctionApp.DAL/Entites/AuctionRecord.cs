using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SQLite.CodeFirst;
using System.ComponentModel.DataAnnotations.Schema;

namespace EuroAuctionApp.DAL.Entites
{
    public class AuctionRecord
    {
        [Key]
        [Autoincrement]
        public int AuctionRecordId { get; set; }

        [Required]
        public string SymbolName { get; set; }

        public DateTime AuctionDate { get; set; }

        public string AuctionDateString { get; set; }

        public int? VolumeAtLast { get; set; }
        public int? VolumeAtClose { get; set; }
        public int? VolumeAtAuction { get; set; }

        public double? LastPrice { get; set; }
        public double? ClosePrice { get; set; }
        public double? AuctionPrice { get; set; }

        //may not contain
        public double? LastDayClosePrice { get; set; }
        public double? TodayHighPrice { get; set; }
        public double? TodayLowPrice { get; set; }
        public double? TodayOpenPrice { get; set; }

        ////Calculated values
        //public double? LastProfitPercent { get; set; }
        //public double? CloseProfitPercent { get; set; }
        //public double? AvgProfitPercent { get; set; }

        //public double? LastProfitBP { get; set; }
        //public double? CloseProfitBP { get; set; }
        //public double? AvgProfitBP { get; set; }

        //public int? LastTradedVolume { get; set; }
        //public int? AuctionTradedVolume { get; set; }
        //public double? LastTradedVolumeProportion { get; set; }
        //public double? AuctionTradedVolumeProportion { get; set; }

        public int? SymbolId { get; set; }
        [ForeignKey("SymbolId")]
        public virtual Symbol Symbol { get; set; }

        public int? MarketId { get; set; }
        [ForeignKey("MarketId")]
        public virtual Market Market { get; set; }
    }
}
