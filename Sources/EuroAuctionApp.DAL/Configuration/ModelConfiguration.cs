using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using EuroAuctionApp.DAL.Entites;

namespace EuroAuctionApp.DAL.Configuration
{
    public static class ModelConfiguration
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            ConfigureAllEntities(modelBuilder);
        }

        private static void ConfigureAllEntities(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Market>().HasMany(o => o.Symbols).WithOptional(s => s.Market).HasForeignKey(s => s.MarketId);
            modelBuilder.Entity<Market>().HasMany(o => o.AuctionRecords).WithOptional(r => r.Market).HasForeignKey(r => r.MarketId);
            modelBuilder.Entity<Symbol>().HasMany(o => o.AuctionRecords).WithOptional(r => r.Symbol).HasForeignKey(r => r.SymbolId);
        }
    }
}
