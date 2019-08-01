using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using EuroAuctionApp.DAL.Entites;
using EuroAuctionApp.DAL.Context;
using EuroAuctionApp.DAL.Interfaces;
using EuroAuctionApp.DAL.Helpers;

namespace EuroAuctionApp.DAL.Services
{
    public class AuctionRepository: IAuctionRepository
    {
        private static readonly string dateFormatString = "yyyy-MM-dd";

        public void InsertOrUpdateRecord(AuctionRecord auctionRecord)
        {
            if (auctionRecord == null)
                throw new ArgumentNullException("auctionRecord");

            using(var db=new AuctionDbContext())
            {
                var record = db.AuctionRecords.FirstOrDefault(
                    r => r.SymbolName == auctionRecord.SymbolName && 
                    r.AuctionDateNumber==auctionRecord.AuctionDateNumber
                    //r.AuctionDateString == auctionRecord.AuctionDateString
                    );

                if (record == null)
                {
                    //create new record
                    if (string.IsNullOrEmpty(auctionRecord.SymbolName))
                        throw new ArgumentNullException("symbol name");

                    var symbol = db.Symbols.FirstOrDefault(s => s.Name == auctionRecord.SymbolName);
                    string code = auctionRecord.SymbolName.Split('.')[1];
                    var market = db.Markets.FirstOrDefault(m => m.Code == code);

                    if (market == null)
                    {
                        market = new Market()
                        {
                            Code=code,
                            Name=DbHelper.GetMarketNameByCode(code),
                            Currency=DbHelper.GetCurrencyByCode(code),
                            CreatedDateTime = DateTime.Now,
                            CreatedDateString = DateTime.Now.ToString(dateFormatString),
                        };

                        db.Markets.Add(market);
                    }

                    if (symbol == null)
                    {
                        symbol = new Symbol()
                        {
                            Name = auctionRecord.SymbolName,
                            Market = market,
                            CreatedDateTime = DateTime.Now,
                            CreatedDateString = DateTime.Now.ToString(dateFormatString),
                        };
                        db.Symbols.Add(symbol);
                    }

                    auctionRecord.Symbol = symbol;
                    auctionRecord.Market = market;

                    db.AuctionRecords.Add(auctionRecord);
                }
                else
                {
                    //update exist record
                    record.AuctionPrice = auctionRecord.AuctionPrice;
                    record.ClosePrice = auctionRecord.ClosePrice;
                    record.LastPrice = auctionRecord.LastPrice;

                    record.VolumeAtAuction = auctionRecord.VolumeAtAuction;
                    record.VolumeAtClose = auctionRecord.VolumeAtClose;
                    record.VolumeAtLast = auctionRecord.VolumeAtLast;
                }

                db.SaveChanges();
            }
        }

        public IList<Market> GetAllMarkets()
        {
            using (var db = new AuctionDbContext())
            {
                return db.Markets.ToList();
            }
        }

        public IList<AuctionRecord> GetMarketRecordsByDateRange(string marketCode, DateTime startDate, DateTime endDate)
        {
            using (var db = new AuctionDbContext())
            {
                int startDateNumber = startDate.Year * 10000 + startDate.Month * 100 + startDate.Day;
                int endDateNumber = endDate.Year * 10000 + endDate.Month * 100 + endDate.Day;

                var records = db.AuctionRecords
                    .Where(o => o.Market.Code == marketCode &&
                           o.AuctionDateNumber >= startDateNumber && o.AuctionDateNumber <= endDateNumber);
                return records.ToList();
                    
            }
        }

    }
}
