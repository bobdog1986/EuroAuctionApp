using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SQLite.CodeFirst;
using EuroAuctionApp.DAL.Entites;

namespace EuroAuctionApp.DAL.Context
{
    public class AuctionDbInitializer:SqliteDropCreateDatabaseWhenModelChanges<AuctionDbContext>
    {
        public AuctionDbInitializer(DbModelBuilder modelBuilder)
            : base(modelBuilder, typeof(CustomHistory))
        { }

        protected override void Seed(AuctionDbContext context)
        {
            // Here you can seed your core data if you have any.
        }
    }
}
