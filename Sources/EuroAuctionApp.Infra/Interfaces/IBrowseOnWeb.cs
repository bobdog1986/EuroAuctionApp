using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroAuctionApp.Infra.Interfaces
{
    interface IBrowseOnWeb
    {
        void Open(string link);

        bool CanOpen(string link);

        void Raise(string link);
    }
}
