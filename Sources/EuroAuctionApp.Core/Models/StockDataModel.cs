using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace EuroAuctionApp.CoreViews.Models
{
    public class AuctionDataModel:BindableBase
    {
        private string symbol;
        public string Symbol
        {
            get { return symbol; }
            set
            {
                SetProperty(ref symbol, value);
                OnSymbolChanged();
            }
        }

        private string market;
        public string Market
        {
            get { return market; }
            set { SetProperty(ref market, value); }
        }

        private double last5Price;
        public double Last5Price
        {
            get { return last5Price; }
            set { SetProperty(ref last5Price, value); }
        }

        private double closePrice;
        public double ClosePrice
        {
            get { return closePrice; }
            set { SetProperty(ref closePrice, value); }
        }

        private double auctionPrice;
        public double AuctionPrice
        {
            get { return auctionPrice; }
            set { SetProperty(ref auctionPrice, value); }
        }

        private int totalVolumeAt25;
        public int TotalVolumeAt25
        {
            get { return totalVolumeAt25; }
            set { SetProperty(ref totalVolumeAt25, value); }
        }

        private int totalVolumeAtClose;
        public int TotalVolumeAtClose
        {
            get { return totalVolumeAtClose; }
            set { SetProperty(ref totalVolumeAtClose, value); }
        }

        private int totalVolumeAtAuction;
        public int TotalVolumeAtAuction
        {
            get { return totalVolumeAtAuction; }
            set { SetProperty(ref totalVolumeAtAuction, value); }
        }

        private int volumeInLast5;
        public int VolumeInLast5
        {
            get { return volumeInLast5; }
            set { SetProperty(ref volumeInLast5, value); }
        }

        private int volumeInAuction;
        public int VolumeInAuction
        {
            get { return volumeInAuction; }
            set { SetProperty(ref volumeInAuction, value); }
        }

        private double closeProfitInPercent;
        public double CloseProfitInPercent
        {
            get { return closeProfitInPercent; }
            set { SetProperty(ref closeProfitInPercent, value); }
        }

        private double closeProfitInBP;
        public double CloseProfitInBP
        {
            get { return closeProfitInBP; }
            set { SetProperty(ref closeProfitInBP, value); }
        }

        private double last5ProfitInPercent;
        public double Last5ProfitInPercent
        {
            get { return last5ProfitInPercent; }
            set { SetProperty(ref last5ProfitInPercent, value); }
        }

        private double last5ProfitInBP;
        public double Last5ProfitInBP
        {
            get { return last5ProfitInBP; }
            set { SetProperty(ref last5ProfitInBP, value); }
        }

        private double avgProfitInPercent;
        public double AvgProfitInPercent
        {
            get { return avgProfitInPercent; }
            set { SetProperty(ref avgProfitInPercent, value); }
        }

        private double avgProfitInBP;
        public double AvgProfitInBP
        {
            get { return avgProfitInBP; }
            set { SetProperty(ref avgProfitInBP, value); }
        }

        private double auctionVolumePercent;
        public double AuctionVolumePercent
        {
            get { return auctionVolumePercent; }
            set { SetProperty(ref auctionVolumePercent, value); }
        }

        private double last5VolumePercent;
        public double Last5VolumePercent
        {
            get { return last5VolumePercent; }
            set { SetProperty(ref last5VolumePercent, value); }
        }
        
        public void Calculate()
        {
            VolumeInLast5 = TotalVolumeAtClose - TotalVolumeAt25;
            VolumeInAuction = TotalVolumeAtAuction - TotalVolumeAtClose;
            AuctionVolumePercent = VolumeInAuction * 100.0 / TotalVolumeAtAuction;
            Last5VolumePercent = VolumeInLast5 * 100.0 / TotalVolumeAtAuction;

            CloseProfitInPercent = 100.0 * (AuctionPrice - ClosePrice) / AuctionPrice;
            Last5ProfitInPercent = 100.0 * (AuctionPrice - Last5Price) / AuctionPrice;
            AvgProfitInPercent = 100.0 * (AuctionPrice - (ClosePrice + Last5Price) * 0.5) / AuctionPrice;

            CloseProfitInBP = CloseProfitInPercent * 100;
            Last5ProfitInBP = Last5ProfitInPercent * 100;
            AvgProfitInBP = AvgProfitInPercent * 100;

            //round
            AuctionVolumePercent = Math.Round(AuctionVolumePercent, 1);
            Last5VolumePercent = Math.Round(Last5VolumePercent, 1);

            CloseProfitInPercent = Math.Round(CloseProfitInPercent, 3);
            Last5ProfitInPercent = Math.Round(Last5ProfitInPercent, 3);
            AvgProfitInPercent = Math.Round(AvgProfitInPercent, 3);

            CloseProfitInBP = Math.Round(CloseProfitInBP, 1);
            Last5ProfitInBP = Math.Round(Last5ProfitInBP, 1);
            AvgProfitInBP = Math.Round(AvgProfitInBP, 1);

        }

        void OnSymbolChanged()
        {
            if(string.IsNullOrEmpty(Market))
            {
                Market = Symbol.Split('.')[1];
            }
        }
    }
}
