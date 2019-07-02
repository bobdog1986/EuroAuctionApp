using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SQLite.CodeFirst;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EuroAuctionApp.DAL.Entites
{
    public class Symbol
    {
        [Key]
        [Autoincrement]
        public int SymbolId { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string CreatedDateString { get; set; }

        public decimal? LastPrice { get; set; }

        public int? LastDayVolume { get; set; }

        public bool IsStar { get; set; }

        public bool IsHidden { get; set; }

        public bool IsPriceTooHigh { get; set; }

        public bool IsPriceTooLow { get; set; }

        public bool IsVolumeTooHigh { get; set; }

        public bool IsVolumeTooLow { get; set; }

        public int? MarketId { get; set; }

        [ForeignKey("MarketId")]
        public virtual Market Market { get; set; }

        public virtual ICollection<AuctionRecord> AuctionRecords { get; set; }
    }
}
