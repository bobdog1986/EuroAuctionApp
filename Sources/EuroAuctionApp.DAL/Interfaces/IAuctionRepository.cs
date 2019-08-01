using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EuroAuctionApp.DAL.Entites;

namespace EuroAuctionApp.DAL.Interfaces
{
    public interface IAuctionRepository
    {
        void InsertOrUpdateRecord(AuctionRecord auctionRecord);

        //void InsertOrUpdateRecordRange(IList<AuctionRecord> auctionRecords);

        //ICollection<AuctionRecord> GetSymbolRecordsByDateRange(string symbolName, DateTime startDate, DateTime endDate);

        //ICollection<AuctionRecord> GetSymbolRecordsFromStartDate(string symbolName, DateTime startDate, int count);

        //ICollection<AuctionRecord> GetSymbolRecordsFromEndDate(string symbolName, DateTime endDate, int count , bool includeEnd=false);

        //ICollection<AuctionRecord> GetLastestSymbolRecords(string symbolName, int count);

        //ICollection<AuctionRecord> GetMarketRecordsOfDate(string marketCode, DateTime auctionDate);

        IList<AuctionRecord> GetMarketRecordsByDateRange(string marketCode, DateTime startDate, DateTime endDate);

        //AuctionRecord GetAuctionRecord(string symbolName, DateTime auctionDate);

        IList<Market> GetAllMarkets();

        //ICollection<Symbol> GetAllSymbols();

        //ICollection<AuctionRecord> GetAllRecords();


    }
}
