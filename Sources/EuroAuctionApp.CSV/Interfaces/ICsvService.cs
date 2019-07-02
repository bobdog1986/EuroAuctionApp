using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EuroAuctionApp.CSV.Models;

namespace EuroAuctionApp.CSV.Interfaces
{
    public interface ICsvService
    {
        IList<QuoteLineData> ReadQuoteAllLines(string filename);
    }
}
