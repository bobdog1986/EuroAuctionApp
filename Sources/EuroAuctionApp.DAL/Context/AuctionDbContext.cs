using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using EuroAuctionApp.DAL.Entites;
using System.Data.SQLite;
using SQLite;
using EuroAuctionApp.DAL.Configuration;

namespace EuroAuctionApp.DAL.Context
{
    public class AuctionDbContext:DbContext
    {
        private static readonly string dbPath = @"D:\EuroAuctionApp Data\auctiondb.sqlite";

        public AuctionDbContext()
            : this(dbPath)
        {
            Configure();
        }

        public AuctionDbContext(string filename)
            : base(new SQLiteConnection()
            {
                ConnectionString =
                    new SQLiteConnectionStringBuilder()
                    { DataSource = filename, ForeignKeys = true }
                    .ConnectionString
            }, true)
        {
            Configure();
        }

        private void Configure()
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ModelConfiguration.Configure(modelBuilder);
            var initializer = new AuctionDbInitializer(modelBuilder);
            Database.SetInitializer(initializer);
        }

        public virtual DbSet<Market> Markets { get; set; }
        public virtual DbSet<Symbol> Symbols { get; set; }
        public virtual DbSet<AuctionRecord> AuctionRecords { get; set; }
    }
}
