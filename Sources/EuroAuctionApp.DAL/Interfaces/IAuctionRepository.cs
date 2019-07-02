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

        //Task InsertOrUpdateRecordRangeAsync(IList<AuctionRecord> auctionRecords);

        //ICollection<AuctionRecord> GetSymbolRecordsByDateRange(string symbolName, DateTime startDate, DateTime endDate);

        //Task<ICollection<AuctionRecord>> GetSymbolRecordsByDateRangeAsync(string symbolName, DateTime startDate, DateTime endDate);

        //ICollection<AuctionRecord> GetSymbolRecordsFromStartDate(string symbolName, DateTime startDate, int count);

        //Task<ICollection<AuctionRecord>> GetSymbolRecordsFromStartDateAsync(string symbolName, DateTime startDate, int count);

        //ICollection<AuctionRecord> GetSymbolRecordsFromEndDate(string symbolName, DateTime endDate, int count , bool includeEnd=false);

        //Task<ICollection<AuctionRecord>> GetSymbolRecordsFromEndDateAsync(string symbolName, DateTime endDate, int count, bool includeEnd= false);

        //ICollection<AuctionRecord> GetLastestSymbolRecords(string symbolName, int count);

        //Task<ICollection<AuctionRecord>> GetLastestSymbolRecordsAsync(string symbolName, int count);

        //ICollection<AuctionRecord> GetMarketRecordsOfDate(string marketCode, DateTime auctionDate);

        //Task<ICollection<AuctionRecord>> GetMarketRecordsOfDateAsync(string marketCode, DateTime auctionDate);

        //AuctionRecord GetAuctionRecord(string symbolName, DateTime auctionDate);

        //ICollection<Market> GetAllMarkets();

        //ICollection<Symbol> GetAllSymbols();

        //ICollection<AuctionRecord> GetAllRecords();


    }
}
