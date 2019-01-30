using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;
using SQLitePCL;
using SQLite;

namespace EuroAuctionApp.DAL.Models
{
    public class AuctionLineData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime AuctionDate { get; set; }

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
    }
}
