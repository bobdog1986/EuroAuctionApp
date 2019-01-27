using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EuroAuctionApp.Infra.Base;
using EuroAuctionApp.Infra.Events;
using System.Collections.ObjectModel;
using System.Data;
using Prism.Commands;
using System.Windows.Forms;
using System.IO;
using EuroAuctionApp.DAL.Models;
using EuroAuctionApp.DAL.Interfaces;
using CsvHelper;
using EuroAuctionApp.CoreViews.Models;
using EuroAuctionApp.CoreViews.Helpers;

namespace EuroAuctionApp.CoreViews.ViewModels
{
    public class FileOperationViewModel:ViewModelBase
    {
        private ICsvService csvHelper;
        public FileOperationViewModel()
        {
            PublishStatusMessage("MainRegionViewModel");

            OpenTradingFileCommand = new DelegateCommand(OpenLast5File);
            OpenCloseFileCommand= new DelegateCommand(OpenCloseFile);
            OpenAuctionFileCommand= new DelegateCommand(OpenAuctionFile);

            OpenFolderCommand = new DelegateCommand(OpenFolder);
            ReadFolderCommand = new DelegateCommand(ReadFolder);
            
            ShowButtonCommand = new DelegateCommand(Show);
            SortFilesCommand = new DelegateCommand(Sort);
            SelectMarketCommand = new DelegateCommand(OnSelectMarket);
            csvHelper = Resolve<ICsvService>();

            InitDisplay();
        }

        public DelegateCommand OpenTradingFileCommand { get; private set; }
        public DelegateCommand OpenCloseFileCommand { get; private set; }
        public DelegateCommand OpenAuctionFileCommand { get; private set; }
        public DelegateCommand OpenFolderCommand { get; private set; }
        public DelegateCommand ReadFolderCommand { get; private set; }      
        public DelegateCommand ShowButtonCommand { get; private set; }
        public DelegateCommand SortFilesCommand { get; private set; }
        public DelegateCommand SelectMarketCommand { get; private set; }


        void OpenFolder()
        {
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();

                dialog.SelectedPath = Settings.DefaultFolder;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    //ReadTradingCsvFile(dialog.FileName);

                    var path = dialog.SelectedPath;
                    PublishStatusMessage(path);

                    var files= Directory.GetFiles(path, "QuoteBoardExport_*.csv");

                    if (files != null && files.Count() > 0)
                    {
                        QuoteFileCollection = QuoteFileCollection??new ObservableCollection<QuoteFileModel>();

                        foreach (var file in files)
                        {
                            QuoteFileCollection.Add(new QuoteFileModel(file));
                        }
                    }

                    Settings.DefaultFolder = dialog.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                LogError("DoSaveButtonClick() : ex = " + ex.Message);
            }
        }

        void ReadFolder()
        {
            if (QuoteFileCollection != null && QuoteFileCollection.Count > 0)
            {
                foreach(var quote in QuoteFileCollection)
                {
                    if (quote.Is25File)
                    {
                        ReadLast5CsvFile(quote.FileName);
                    }
                    else if (quote.IsCloseFile)
                    {
                        ReadCloseCsvFile(quote.FileName);
                    }
                    else if (quote.IsAuctionFile)
                    {
                        ReadAuctionCsvFile(quote.FileName);
                    }
                    else
                    {
                        //
                    }
                }
            }
        }

        string OpenFile()
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.RestoreDirectory = true;
                dialog.Filter = "CSV Files|*.csv";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.FileName;
                }
            }
            catch (Exception ex)
            {
                LogError("OpenFile() : ex = " + ex.Message);
            }

            return string.Empty;
        }

        void OpenLast5File()
        {
            string filename = OpenFile();
            if (!string.IsNullOrEmpty(filename))
            {
                ReadLast5CsvFile(filename);
            }
        }

        void OpenCloseFile()
        {
            string filename = OpenFile();
            if (!string.IsNullOrEmpty(filename))
            {
                ReadCloseCsvFile(filename);
            }
        }

        void OpenAuctionFile()
        {
            string filename = OpenFile();
            if (!string.IsNullOrEmpty(filename))
            {
                ReadAuctionCsvFile(filename);
            }
        }


        void ReadLast5CsvFile(string filename)
        {
            if (File.Exists(filename))
            {
                var records =csvHelper.ReadAllLines(filename);

                if (records != null && records.Count > 0)
                {
                    StockDataCollection = StockDataCollection ?? new ObservableCollection<StockDataModel>();

                    foreach (var record in records)
                    {
                        var old = StockDataCollection.FirstOrDefault(o => o.Symbol == record.Symbol);
                        if (old == null)
                        {
                            old = new StockDataModel()
                            {
                                Symbol = record.Symbol,
                                Last5Price = record.LastPrice,
                                TotalVolumeAt25 = record.Volume
                            };
                            StockDataCollection.Add(old);
                        }
                        else
                        {
                            old.Last5Price = record.LastPrice;
                            old.TotalVolumeAt25 = record.Volume;
                        }
                    }
                }
            }
        }

        void ReadCloseCsvFile(string filename)
        {
            if (File.Exists(filename))
            {
                var records = csvHelper.ReadAllLines(filename);

                if (records != null && records.Count > 0)
                {
                    StockDataCollection = StockDataCollection ?? new ObservableCollection<StockDataModel>();

                    foreach (var record in records)
                    {
                        var old = StockDataCollection.FirstOrDefault(o => o.Symbol == record.Symbol);
                        if (old == null)
                        {
                            old = new StockDataModel()
                            {
                                Symbol = record.Symbol,
                                ClosePrice = record.LastPrice,
                                TotalVolumeAtClose = record.Volume
                            };
                            StockDataCollection.Add(old);
                        }
                        else
                        {
                            old.ClosePrice = record.LastPrice;
                            old.TotalVolumeAtClose = record.Volume;
                        }
                    }
                }
            }
        }

        void ReadAuctionCsvFile(string filename)
        {
            if (File.Exists(filename))
            {
                var records = csvHelper.ReadAllLines(filename);

                if (records != null && records.Count > 0)
                {
                    StockDataCollection = StockDataCollection ?? new ObservableCollection<StockDataModel>();

                    foreach (var record in records)
                    {
                        var old = StockDataCollection.FirstOrDefault(o => o.Symbol == record.Symbol);
                        if (old == null)
                        {
                            old = new StockDataModel()
                            {
                                Symbol = record.Symbol,
                                AuctionPrice = record.LastPrice,
                                TotalVolumeAtAuction = record.Volume
                            };
                            StockDataCollection.Add(old);
                        }
                        else
                        {
                            old.AuctionPrice = record.LastPrice;
                            old.TotalVolumeAtAuction = record.Volume;
                        }
                    }
                }
            }
        }

        void Show()
        {
            ReadFolder();

            if (StockDataCollection != null && StockDataCollection.Count > 0)
            {
                foreach(var data in StockDataCollection)
                {                    
                    data.Calculate();
                }

            }
            GetMarketList();
            GetFilterStockDataCollection();
        }

        void Sort()
        {
            //if (QuoteFileCollection.Count == 0) return;

            var groups= QuoteFileCollection.ToList().GroupBy(o => o.FirstMarket);

            foreach(var group in groups)
            {
                var files = group.Select(o => o).ToList();
                if (files.Count == 3)
                {
                    files[0].Is25File = true;
                    files[1].IsCloseFile = true;
                    files[2].IsAuctionFile = true;
                }
            }
        }


        void InitDisplay()
        {
            StockDataCollection = new ObservableCollection<StockDataModel>();
            QuoteFileCollection = new ObservableCollection<QuoteFileModel>();
        }

        void PublishStatusMessage(string message)
        {
            EventAggregator.GetEvent<StatusMessageEvent>().Publish(message);
        }

        private ObservableCollection<StockDataModel> stockDataCollection;
        public ObservableCollection<StockDataModel> StockDataCollection
        {
            get { return stockDataCollection; }
            set { SetProperty(ref stockDataCollection, value); }
        }

        private ObservableCollection<StockDataModel> filterStockDataCollection;
        public ObservableCollection<StockDataModel> FilterStockDataCollection
        {
            get { return filterStockDataCollection; }
            set { SetProperty(ref filterStockDataCollection, value); }
        }
        
        private ObservableCollection<QuoteFileModel> quoteFileCollection;
        public ObservableCollection<QuoteFileModel> QuoteFileCollection
        {
            get { return quoteFileCollection; }
            set { SetProperty(ref quoteFileCollection, value); }
        }
        private string selectedMarket;
        public string SelectedMarket
        {
            get { return selectedMarket; }
            set { SetProperty(ref selectedMarket, value); }
        }

        private bool isEnableFilter;
        public bool IsEnableFilter
        {
            get { return isEnableFilter; }
            set { SetProperty(ref isEnableFilter, value); }
        }
        
        private ObservableCollection<string> marketCollection;
        public ObservableCollection<string> MarketCollection
        {
            get { return marketCollection; }
            set { SetProperty(ref marketCollection, value); }
        }

        void GetMarketList()
        {
            MarketCollection = new ObservableCollection<string>(StockDataCollection.Select(o => o.Market).Distinct());
        }
        void OnSelectMarket()
        {
            GetFilterStockDataCollection();
        }
        void GetFilterStockDataCollection()
        {
            if (IsEnableFilter)
            {
                if (string.IsNullOrEmpty(SelectedMarket))
                {
                    FilterStockDataCollection = StockDataCollection;
                }
                else
                {
                    FilterStockDataCollection = new ObservableCollection<StockDataModel>(StockDataCollection.Where(o => o.Market == SelectedMarket));
                }
            }
            else
            {
                FilterStockDataCollection = StockDataCollection;
            }
        }
    }
}
