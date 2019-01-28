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

            InitCommands();
            
            csvHelper = Resolve<ICsvService>();

            InitDisplay();
        }

        private void InitCommands()
        {
            AddQuoteFileCommand = new DelegateCommand(AddQuoteFile);
            AddQuoteFolderCommand = new DelegateCommand(AddQuoteFolder);
            IdentifyQuotesCommand = new DelegateCommand(IdentifyQuoteFiles);

            CalculateQuoteCommand = new DelegateCommand(CalculateQuoteFiles);
            SelectMarketCommand = new DelegateCommand(OnSelectMarket);
            SaveQuotesCommand = new DelegateCommand(SaveQuotes);
            CleanQuotesCommand = new DelegateCommand(CleanQuotes);
            OpenSaveQuoteFolderCommand = new DelegateCommand(OpenSaveQuoteFolder);

            SaveAuctionFileCommand = new DelegateCommand(SaveAuctionFile);
            CleanAuctionCommand = new DelegateCommand(CleanAuction);
            ExportAuctionFileCommand = new DelegateCommand(ExportAuctionFile);
            OpenAuctionFileCommand = new DelegateCommand(OpenAuctionFile);
            WriteToDbCommand = new DelegateCommand(WriteToDb);
        }



        public DelegateCommand AddQuoteFileCommand { get; private set; }
        public DelegateCommand AddQuoteFolderCommand { get; private set; }
        public DelegateCommand IdentifyQuotesCommand { get; private set; }
        public DelegateCommand CalculateQuoteCommand { get; private set; }
        public DelegateCommand SelectMarketCommand { get; private set; }
        public DelegateCommand SaveQuotesCommand { get; private set; }
        public DelegateCommand CleanQuotesCommand { get; private set; }
        public DelegateCommand OpenSaveQuoteFolderCommand { get; private set; }
        public DelegateCommand SaveAuctionFileCommand { get; private set; }
        public DelegateCommand CleanAuctionCommand { get; private set; }
        public DelegateCommand ExportAuctionFileCommand { get; private set; }
        public DelegateCommand OpenAuctionFileCommand { get; private set; }
        public DelegateCommand WriteToDbCommand { get; private set; }
       
        void AddQuoteFolder()
        {
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();

                dialog.SelectedPath = Settings.QuoteFolderImportPath;

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

                    Settings.QuoteFolderImportPath = dialog.SelectedPath;

                    IdentifyQuoteFiles();
                }
            }
            catch (Exception ex)
            {
                LogError("DoSaveButtonClick() : ex = " + ex.Message);
            }
        }

        void AddQuoteFile()
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.RestoreDirectory = true;
                dialog.Filter = "CSV Files|*.csv";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string file = dialog.FileName;

                    QuoteFileCollection = QuoteFileCollection ?? new ObservableCollection<QuoteFileModel>();

                    QuoteFileCollection.Add(new QuoteFileModel(file));
                }
            }
            catch (Exception ex)
            {
                LogError("OpenFile() : ex = " + ex.Message);
            }

        }

        void IdentifyQuoteFiles()
        {
            var groups = QuoteFileCollection.ToList().GroupBy(o => o.FirstMarket);

            foreach (var group in groups)
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

        void ImportAllQuoteFileData()
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


        void ReadLast5CsvFile(string filename)
        {
            if (File.Exists(filename))
            {
                var records =csvHelper.ReadQuoteAllLines(filename);

                if (records != null && records.Count > 0)
                {
                    AuctionDataCollection = AuctionDataCollection ?? new ObservableCollection<AuctionDataModel>();

                    foreach (var record in records)
                    {
                        var old = AuctionDataCollection.FirstOrDefault(o => o.Symbol == record.Symbol);
                        if (old == null)
                        {
                            old = new AuctionDataModel()
                            {
                                Symbol = record.Symbol,
                                Last5Price = record.LastPrice,
                                TotalVolumeAt25 = record.Volume
                            };
                            AuctionDataCollection.Add(old);
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
                var records = csvHelper.ReadQuoteAllLines(filename);

                if (records != null && records.Count > 0)
                {
                    AuctionDataCollection = AuctionDataCollection ?? new ObservableCollection<AuctionDataModel>();

                    foreach (var record in records)
                    {
                        var old = AuctionDataCollection.FirstOrDefault(o => o.Symbol == record.Symbol);
                        if (old == null)
                        {
                            old = new AuctionDataModel()
                            {
                                Symbol = record.Symbol,
                                ClosePrice = record.LastPrice,
                                TotalVolumeAtClose = record.Volume
                            };
                            AuctionDataCollection.Add(old);
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
                var records = csvHelper.ReadQuoteAllLines(filename);

                if (records != null && records.Count > 0)
                {
                    AuctionDataCollection = AuctionDataCollection ?? new ObservableCollection<AuctionDataModel>();

                    foreach (var record in records)
                    {
                        var old = AuctionDataCollection.FirstOrDefault(o => o.Symbol == record.Symbol);
                        if (old == null)
                        {
                            old = new AuctionDataModel()
                            {
                                Symbol = record.Symbol,
                                AuctionPrice = record.LastPrice,
                                TotalVolumeAtAuction = record.Volume
                            };
                            AuctionDataCollection.Add(old);
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

        void CalculateQuoteFiles()
        {
            AuctionDataCollection = new ObservableCollection<AuctionDataModel>();

            ImportAllQuoteFileData();

            foreach (var data in AuctionDataCollection)
            {
                data.Calculate();
            }

            UpdateFilterStockDataCollection();
        }

        string GetAppDataPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }

        private static string appName = "EuroAuctionApp";
        private static string quoteBoard = "QuoteBoard";
        private static string auctionData = "AuctionData";

        string GetQuoteBoardPath()
        {
            return Path.Combine(GetAppDataPath(), appName, quoteBoard);
        }

        string GetQuoteBoardSavePath(DateTime dateTime)
        {
            string year = dateTime.Year.ToString();
            string yearAndMonth = dateTime.ToString("yyyy-MM");
            return Path.Combine(GetQuoteBoardPath(), year,yearAndMonth, dateTime.ToString("yyyy-MM-dd"));
        }

        string GetAuctionDataPath()
        {
            return Path.Combine(GetAppDataPath(), appName, auctionData);
        }

        string GetAuctionDataSavePath(DateTime dateTime)
        {
            string year = dateTime.Year.ToString();
            string yearAndMonth = dateTime.ToString("yyyy-MM");
            return Path.Combine(GetAuctionDataPath(), year, yearAndMonth);
        }

        string GetAuctionFileSaveName(DateTime dateTime)
        {
            return auctionData + "_" + dateTime.ToString("yyyy-MM-dd") + ".csv";
        }

        void SaveQuotes()
        {
            if (QuoteDatePickerDate == null)
            {
                PublishStatusMessage("select Quote picker");
                return;
            }

            string targetPath = GetQuoteBoardSavePath(QuoteDatePickerDate.Value);
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            PublishStatusMessage(targetPath);

            foreach (var quote in QuoteFileCollection)
            {
                MoveQuoteFile(quote.FileName, Path.Combine(targetPath, quote.ShortName));
            }
        }

        void MoveQuoteFile(string source,string dest)
        {
            try
            {
                File.Copy(source, dest, true);
                File.Delete(source);
            }
            catch(Exception ex)
            {
                LogError(ex.Message);
                PublishStatusMessage(ex.Message);
            }
        }

        void OpenSaveQuoteFolder()
        {
            string targetPath = GetQuoteBoardPath();
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            System.Diagnostics.Process.Start(targetPath);
        }

        void CleanQuotes()
        {
            QuoteFileCollection.Clear();
        }

        void SaveAuctionFile()
        {
            if (AuctionDatePickerDate == null)
            {
                PublishStatusMessage("select Quote picker");
                return;
            }

            string targetPath = GetAuctionDataSavePath(AuctionDatePickerDate.Value);
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            string targetfilename = GetAuctionFileSaveName(AuctionDatePickerDate.Value);

            PublishStatusMessage(targetPath);

            string filename = Path.Combine(targetPath, targetfilename);

            WriteAuctionFile(filename);
        }

        private void CleanAuction()
        {
            AuctionDataCollection.Clear();

            UpdateFilterStockDataCollection();
        }

        void ExportAuctionFile()
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.RestoreDirectory = true;
                dialog.Filter = "CSV Files|*.csv";
                if (AuctionDatePickerDate != null)
                {
                    dialog.FileName = GetAuctionFileSaveName(AuctionDatePickerDate.Value);
                }
                else
                {
                    dialog.FileName = auctionData + "_" + DateTime.Now.ToString("yyyy-MM-dd") + ".csv";
                }
                
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    WriteAuctionFile(dialog.FileName);
                }
            }
            catch (Exception ex)
            {
                LogError("OpenFile() : ex = " + ex.Message);
            }
        }

        void WriteAuctionFile(string filename)
        {
            List<AuctionLineData> records = new List<AuctionLineData>();

            foreach (var auction in AuctionDataCollection)
            {
                AuctionLineData line = new AuctionLineData()
                {
                    Symbol = auction.Symbol,
                    Market = auction.Market,
                    Last5Price = auction.Last5Price,
                    ClosePrice = auction.ClosePrice,
                    AuctionPrice = auction.AuctionPrice,
                    TotalVolumeAt25 = auction.TotalVolumeAt25,
                    TotalVolumeAtClose = auction.TotalVolumeAtClose,
                    TotalVolumeAtAuction = auction.TotalVolumeAtAuction,
                    VolumeInLast5 = auction.VolumeInLast5,
                    VolumeInAuction = auction.VolumeInAuction,
                    CloseProfitInPercent = auction.CloseProfitInPercent,
                    CloseProfitInBP = auction.CloseProfitInBP,
                    Last5ProfitInPercent = auction.Last5ProfitInPercent,
                    Last5ProfitInBP = auction.Last5ProfitInBP,
                    AvgProfitInPercent = auction.AvgProfitInPercent,
                    AvgProfitInBP = auction.AvgProfitInBP,
                    AuctionVolumePercent = auction.AuctionVolumePercent,
                    Last5VolumePercent = auction.Last5VolumePercent
                };
                records.Add(line);
            }

            if (records.Count > 0)
            {
                csvHelper.WriteAuctionFile(filename, records);
            }
        }

        void OpenAuctionFile()
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.RestoreDirectory = true;
                dialog.Filter = "CSV Files|*.csv";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string filename = dialog.FileName;

                    ImportAuctionFile(filename);
                }
            }
            catch (Exception ex)
            {
                LogError("OpenFile() : ex = " + ex.Message);
            }
        }

        void ImportAuctionFile(string filename)
        {
            if (File.Exists(filename))
            {
                var records = csvHelper.ReadAuctionAllLines(filename);

                if (records != null && records.Count > 0)
                {
                    CleanAuction();

                    foreach (var record in records)
                    {
                        var old = AuctionDataCollection.FirstOrDefault(o => o.Symbol == record.Symbol);
                        if (old == null)
                        {
                            old = new AuctionDataModel()
                            {
                                Symbol = record.Symbol,
                                Market = record.Market,
                                Last5Price = record.Last5Price,
                                ClosePrice = record.ClosePrice,
                                AuctionPrice = record.AuctionPrice,
                                TotalVolumeAt25 = record.TotalVolumeAt25,
                                TotalVolumeAtClose = record.TotalVolumeAtClose,
                                TotalVolumeAtAuction = record.TotalVolumeAtAuction,
                                VolumeInLast5 = record.VolumeInLast5,
                                VolumeInAuction = record.VolumeInAuction,
                                CloseProfitInPercent = record.CloseProfitInPercent,
                                CloseProfitInBP = record.CloseProfitInBP,
                                Last5ProfitInPercent = record.Last5ProfitInPercent,
                                Last5ProfitInBP = record.Last5ProfitInBP,
                                AvgProfitInPercent = record.AvgProfitInPercent,
                                AvgProfitInBP = record.AvgProfitInBP,
                                AuctionVolumePercent = record.AuctionVolumePercent,
                                Last5VolumePercent = record.Last5VolumePercent
                            };
                            AuctionDataCollection.Add(old);
                        }
                        else
                        {
                            old.Symbol = record.Symbol;
                            old.Market = record.Market;
                            old.Last5Price = record.Last5Price;
                            old.ClosePrice = record.ClosePrice;
                            old.AuctionPrice = record.AuctionPrice;
                            old.TotalVolumeAt25 = record.TotalVolumeAt25;
                            old.TotalVolumeAtClose = record.TotalVolumeAtClose;
                            old.TotalVolumeAtAuction = record.TotalVolumeAtAuction;
                            old.VolumeInLast5 = record.VolumeInLast5;
                            old.VolumeInAuction = record.VolumeInAuction;
                            old.CloseProfitInPercent = record.CloseProfitInPercent;
                            old.CloseProfitInBP = record.CloseProfitInBP;
                            old.Last5ProfitInPercent = record.Last5ProfitInPercent;
                            old.Last5ProfitInBP = record.Last5ProfitInBP;
                            old.AvgProfitInPercent = record.AvgProfitInPercent;
                            old.AvgProfitInBP = record.AvgProfitInBP;
                            old.AuctionVolumePercent = record.AuctionVolumePercent;
                            old.Last5VolumePercent = record.Last5VolumePercent;
                        }
                    }
                }
            }
        }

        private void WriteToDb()
        {

        }

        void InitDisplay()
        {
            AuctionDataCollection = new ObservableCollection<AuctionDataModel>();
            QuoteFileCollection = new ObservableCollection<QuoteFileModel>();
            MarketCollection = new ObservableCollection<string>();
        }

        void PublishStatusMessage(string message)
        {
            EventAggregator.GetEvent<StatusMessageEvent>().Publish(message);
        }

        private ObservableCollection<AuctionDataModel> auctionDataCollection;
        public ObservableCollection<AuctionDataModel> AuctionDataCollection
        {
            get { return auctionDataCollection; }
            set { SetProperty(ref auctionDataCollection, value); }
        }

        private ObservableCollection<AuctionDataModel> filterAuctionDataCollection;
        public ObservableCollection<AuctionDataModel> FilterAuctionDataCollection
        {
            get { return filterAuctionDataCollection; }
            set { SetProperty(ref filterAuctionDataCollection, value); }
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

        private DateTime? quoteDatePickerDate;
        public DateTime? QuoteDatePickerDate
        {
            get { return quoteDatePickerDate; }
            set { SetProperty(ref quoteDatePickerDate, value); }
        }

        private DateTime? auctionDatePickerDate;
        public DateTime? AuctionDatePickerDate
        {
            get { return auctionDatePickerDate; }
            set { SetProperty(ref auctionDatePickerDate, value); }
        }

        void UpdateMarketList()
        {
            MarketCollection = new ObservableCollection<string>(AuctionDataCollection.Select(o => o.Market).Distinct());
        }

        void OnSelectMarket()
        {
            UpdateFilterStockDataCollection();
        }

        void UpdateFilterStockDataCollection()
        {
            UpdateMarketList();

            if (IsEnableFilter)
            {
                if (string.IsNullOrEmpty(SelectedMarket))
                {
                    FilterAuctionDataCollection = AuctionDataCollection;
                }
                else
                {
                    FilterAuctionDataCollection = new ObservableCollection<AuctionDataModel>(AuctionDataCollection.Where(o => o.Market == SelectedMarket));
                }
            }
            else
            {
                FilterAuctionDataCollection = AuctionDataCollection;
            }
        }
    }
}
