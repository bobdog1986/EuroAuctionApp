using Prism.Mvvm;
using System;

namespace EuroAuctionApp.CoreViews.Models
{
    public class AuctionDataModel : BindableBase
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

        private DateTime auctionDateTime;

        public DateTime AuctionDateTime
        {
            get { return auctionDateTime; }
            set { SetProperty(ref auctionDateTime, value); }
        }

        private string auctionDateString;
        public string AuctionDateString
        {
            get { return auctionDateString; }
            set { SetProperty(ref auctionDateString, value); }
        }

        private int auctionDateNumber;
        public int AuctionDateNumber
        {
            get { return auctionDateNumber; }
            set { SetProperty(ref auctionDateNumber, value); }
        }

        private string marketCode;

        public string MarketCode
        {
            get { return marketCode; }
            set { SetProperty(ref marketCode, value); }
        }

        private double lastPrice;

        public double LastPrice
        {
            get { return lastPrice; }
            set { SetProperty(ref lastPrice, value); }
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

        private int volumeAtLast;

        public int VolumeAtLast
        {
            get { return volumeAtLast; }
            set { SetProperty(ref volumeAtLast, value); }
        }

        private int volumeAtClose;

        public int VolumeAtClose
        {
            get { return volumeAtClose; }
            set { SetProperty(ref volumeAtClose, value); }
        }

        private int volumeAtAuction;

        public int VolumeAtAuction
        {
            get { return volumeAtAuction; }
            set { SetProperty(ref volumeAtAuction, value); }
        }

        private int lastTradedVolume;

        public int LastTradedVolume
        {
            get { return lastTradedVolume; }
            set { SetProperty(ref lastTradedVolume, value); }
        }

        private int auctionTradedVolume;

        public int AuctionTradedVolume
        {
            get { return auctionTradedVolume; }
            set { SetProperty(ref auctionTradedVolume, value); }
        }

        private double closeProfitPercent;

        public double CloseProfitPercent
        {
            get { return closeProfitPercent; }
            set { SetProperty(ref closeProfitPercent, value);  }
        }

        private double lastProfitPercent;

        public double LastProfitPercent
        {
            get { return lastProfitPercent; }
            set { SetProperty(ref lastProfitPercent, value); }
        }

        private double avgProfitPercent;

        public double AvgProfitPercent
        {
            get { return avgProfitPercent; }
            set { SetProperty(ref avgProfitPercent, value); }
        }

        private double auctionTradedVolumeProportion;

        public double AuctionTradedVolumeProportion
        {
            get { return auctionTradedVolumeProportion; }
            set { SetProperty(ref auctionTradedVolumeProportion, value); }
        }

        private double lastTradedVolumeProportion;

        public double LastTradedVolumeProportion
        {
            get { return lastTradedVolumeProportion; }
            set { SetProperty(ref lastTradedVolumeProportion, value); }
        }

        public void Calculate()
        {
            LastTradedVolume = VolumeAtClose - VolumeAtLast;
            AuctionTradedVolume = VolumeAtAuction - VolumeAtClose;

            LastTradedVolumeProportion = LastTradedVolume * 100.0 / VolumeAtAuction;
            AuctionTradedVolumeProportion = AuctionTradedVolume * 100.0 / VolumeAtAuction;
            //round
            AuctionTradedVolumeProportion = Math.Round(AuctionTradedVolumeProportion, 1);
            LastTradedVolumeProportion = Math.Round(LastTradedVolumeProportion, 1);

            LastProfitPercent = 100.0 * (AuctionPrice - LastPrice) / AuctionPrice;
            CloseProfitPercent = 100.0 * (AuctionPrice - ClosePrice) / AuctionPrice;
            AvgProfitPercent = 100.0 * (AuctionPrice - (ClosePrice + LastPrice) * 0.5) / AuctionPrice;
            //round
            LastProfitPercent = Math.Round(LastProfitPercent, 3);
            CloseProfitPercent = Math.Round(CloseProfitPercent, 3);
            AvgProfitPercent = Math.Round(AvgProfitPercent, 3);
        }

        private void OnSymbolChanged()
        {
            if (string.IsNullOrEmpty(MarketCode))
            {
                MarketCode = Symbol.Split('.')[1];
            }
        }
    }
}