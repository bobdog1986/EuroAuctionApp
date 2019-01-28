using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EuroAuctionApp.DAL.Models;

namespace EuroAuctionApp.DAL.Interfaces
{
    public interface ICsvService
    {
        IList<QuoteLineData> ReadQuoteAllLines(string filename);

        void WriteAuctionFile(string destFileName, IList<AuctionLineData> auctionLines);
        IList<AuctionLineData> ReadAuctionAllLines(string filename);

        //void TrimInvalidCells(string filename);
    }
}
