using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SQLite.CodeFirst;
using System.Collections.Generic;

namespace EuroAuctionApp.DAL.Entites
{
    public class Market
    {
        public Market()
        {
            Symbols = new HashSet<Symbol>();
            AuctionRecords = new HashSet<AuctionRecord>();
        }

        [Key]
        [Autoincrement]
        public int MarketId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        public bool IsStar { get; set; }

        public bool IsHidden { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string CreatedDateString { get; set; }

        public string Currency { get; set; }

        public virtual ICollection<Symbol> Symbols { get; set; }

        public virtual ICollection<AuctionRecord> AuctionRecords { get; set; }
    }
}