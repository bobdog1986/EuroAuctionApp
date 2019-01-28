using EuroAuctionApp.Infra.Helpers;
using EuroAuctionApp.CoreViews.Models;
using EuroAuctionApp.DAL.Interfaces;
using EuroAuctionApp.DAL.Models;
using EuroAuctionApp.Infra.Base;
using EuroAuctionApp.Infra.Events;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;

namespace EuroAuctionApp.CoreViews.ViewModels
{
    public class FileOperationViewModel : ViewModelBase
    {
        private static string format = "yyyy-MM-dd_HH-mm-ss";
        private static string quoteBoard = "QuoteBoard";
        private static string auctionData = "AuctionData";
        private static string appName = "EuroAuctionApp";

        private ICsvService csvHelper;

        public FileOperationViewModel()
        {
            PublishStatusMessage("MainRegionViewModel");

            csvHelper = Resolve<ICsvService>();

            InitCommands();

            InitDisplay();

        }

        private void InitCommands()
        {
            AddQuoteFileCommand = new DelegateCommand(AddQuoteFile);
            AddQuoteFolderCommand = new DelegateCommand(AddQuoteFolder);
            IdentifyQuotesCommand = new DelegateCommand(IdentifyQuoteFiles);

            CalculateQuoteCommand = new DelegateCommand(CalculateQuoteFiles);
            SelectMarketCommand = new DelegateCommand(OnSelectMarket);
            BackupQuotesCommand = new DelegateCommand(MoveQuotesToDefaultFolder);
            CleanQuotesCommand = new DelegateCommand(CleanQuotesData);
            ExploreQuoteFolderCommand = new DelegateCommand(ExploreDefaultQuoteFolder);

            BackupAuctionFileCommand = new DelegateCommand(SaveAuctionDataToDefaultFile);
            CleanAuctionCommand = new DelegateCommand(CleanAuctionData);
            ExportAuctionFileCommand = new DelegateCommand(ExportAuctionDataToFile);
            OpenAuctionFileCommand = new DelegateCommand(OpenAuctionFile);
            ExploreAuctionFolderCommand = new DelegateCommand(ExploreDefaultAuctionFolder);
            WriteToDbCommand = new DelegateCommand(WriteToDb);

            SelectBackupPathCommand = new DelegateCommand(SelectBackupPath);
        }

        public DelegateCommand AddQuoteFileCommand { get; private set; }
        public DelegateCommand AddQuoteFolderCommand { get; private set; }
        public DelegateCommand IdentifyQuotesCommand { get; private set; }
        public DelegateCommand CalculateQuoteCommand { get; private set; }
        public DelegateCommand SelectMarketCommand { get; private set; }
        public DelegateCommand BackupQuotesCommand { get; private set; }
        public DelegateCommand CleanQuotesCommand { get; private set; }
        public DelegateCommand ExploreQuoteFolderCommand { get; private set; }
        public DelegateCommand BackupAuctionFileCommand { get; private set; }
        public DelegateCommand CleanAuctionCommand { get; private set; }
        public DelegateCommand ExportAuctionFileCommand { get; private set; }
        public DelegateCommand OpenAuctionFileCommand { get; private set; }
        public DelegateCommand WriteToDbCommand { get; private set; }
        public DelegateCommand ExploreAuctionFolderCommand { get; private set; }
        public DelegateCommand SelectBackupPathCommand { get; private set; }

        private void AddQuoteFolder()
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

                    var files = Directory.GetFiles(path, "QuoteBoardExport_*.csv");

                    if (files != null && files.Count() > 0)
                    {
                        QuoteFileCollection = QuoteFileCollection ?? new ObservableCollection<QuoteFileModel>();

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

        private void AddQuoteFile()
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

        //private static string format = "yyyy-MM-dd_HH-mm-ss";

        private void IdentifyQuoteFiles()
        {
            SetQuoteDatePickerToFirst();

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

        private void SetQuoteDatePickerToFirst()
        {
            if (QuoteFileCollection != null && QuoteFileCollection.Count > 0)
            {
                QuoteDatePickerDate = QuoteFileCollection[0].DateTime;
            }
        }

        private void ImportAllQuoteFileData()
        {
            if (QuoteFileCollection != null && QuoteFileCollection.Count > 0)
            {
                AuctionDatePickerDate = QuoteFileCollection[0].DateTime;

                foreach (var quote in QuoteFileCollection)
                {
                    if (quote.Is25File)
                    {
                        ReadLast5CsvFileData(quote.FileName);
                    }
                    else if (quote.IsCloseFile)
                    {
                        ReadCloseCsvFileData(quote.FileName);
                    }
                    else if (quote.IsAuctionFile)
                    {
                        ReadAuctionCsvFileData(quote.FileName);
                    }
                    else
                    {
                        //
                    }
                }
            }
        }

        private DateTime GetQuoteFileDateTime(string filename)
        {
            return QuoteFileModel.ParseDateTime(filename);
        }


        private void ReadLast5CsvFileData(string filename)
        {
            if (File.Exists(filename))
            {
                var records = csvHelper.ReadQuoteAllLines(filename);

                var dateTime = GetQuoteFileDateTime(filename);

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

                        old.DateTime = dateTime;
                    }
                }
            }
        }

        private void ReadCloseCsvFileData(string filename)
        {
            if (File.Exists(filename))
            {
                var records = csvHelper.ReadQuoteAllLines(filename);
                var dateTime = GetQuoteFileDateTime(filename);

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
                        old.DateTime = dateTime;
                    }
                }
            }
        }

        private void ReadAuctionCsvFileData(string filename)
        {
            if (File.Exists(filename))
            {
                var records = csvHelper.ReadQuoteAllLines(filename);
                var dateTime = GetQuoteFileDateTime(filename);

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
                        old.DateTime = dateTime;
                    }
                }
            }
        }

        private void CalculateQuoteFiles()
        {
            AuctionDataCollection = new ObservableCollection<AuctionDataModel>();

            ImportAllQuoteFileData();

            CalculateAuctionData();
        }

        private void CalculateAuctionData()
        {
            foreach (var data in AuctionDataCollection)
            {
                data.Calculate();
            }

            UpdateFilterStockDataCollection();
        }

        private void InitBackupRootPath()
        {
            if (string.IsNullOrEmpty(Settings.BackupPath))
            {
                Settings.BackupPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            }

            SelectedBackupPath = Settings.BackupPath;
        }

        private string GetQuoteFileRootPath()
        {
            return Path.Combine(Settings.BackupPath, appName, quoteBoard);
        }

        private string GetQuoteFileSavePath(DateTime dateTime)
        {
            string year = dateTime.Year.ToString();
            string yearAndMonth = dateTime.ToString("yyyy-MM");
            return Path.Combine(GetQuoteFileRootPath(), year, yearAndMonth, dateTime.ToString("yyyy-MM-dd"));
        }

        private string GetAuctionFileRootPath()
        {
            return Path.Combine(Settings.BackupPath, appName, auctionData);
        }

        private string GetAuctionFileSavePath(DateTime dateTime)
        {
            string year = dateTime.Year.ToString();
            string yearAndMonth = dateTime.ToString("yyyy-MM");
            return Path.Combine(GetAuctionFileRootPath(), year, yearAndMonth);
        }

        private string GetAuctionFileSaveName(DateTime dateTime)
        {
            return auctionData + "_" + dateTime.ToString(format) + ".csv";
        }

        private void MoveQuotesToDefaultFolder()
        {
            if (QuoteDatePickerDate == null)
            {
                PublishStatusMessage("select Quote picker");
                return;
            }

            string targetPath = GetQuoteFileSavePath(QuoteDatePickerDate.Value);
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            PublishStatusMessage(targetPath);

            foreach (var quote in QuoteFileCollection)
            {
                MoveFile(quote.FileName, Path.Combine(targetPath, quote.ShortName));
            }
        }

        private void MoveFile(string source, string dest)
        {
            try
            {
                File.Copy(source, dest, true);
                File.Delete(source);
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
                PublishStatusMessage(ex.Message);
            }
        }

        private void ExploreDefaultQuoteFolder()
        {
            string targetPath = GetQuoteFileRootPath();
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            System.Diagnostics.Process.Start(targetPath);
        }

        private void ExploreDefaultAuctionFolder()
        {
            string targetPath = GetAuctionFileRootPath();
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            System.Diagnostics.Process.Start(targetPath);
        }
        

        private void CleanQuotesData()
        {
            QuoteFileCollection.Clear();
        }

        private void SaveAuctionDataToDefaultFile()
        {
            if (AuctionDatePickerDate == null)
            {
                PublishStatusMessage("select auction picker");
                return;
            }

            string targetPath = GetAuctionFileSavePath(AuctionDatePickerDate.Value);
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            string targetfilename = GetAuctionFileSaveName(AuctionDatePickerDate.Value);

            PublishStatusMessage(targetPath);

            string filename = Path.Combine(targetPath, targetfilename);

            WriteAuctionDataToFile(filename);
        }

        private void CleanAuctionData()
        {
            AuctionDataCollection.Clear();

            UpdateFilterStockDataCollection();
        }

        private void ExportAuctionDataToFile()
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
                    dialog.FileName = GetAuctionFileSaveName(DateTime.Now);
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    WriteAuctionDataToFile(dialog.FileName);
                }
            }
            catch (Exception ex)
            {
                LogError("ExportAuctionFile() : ex = " + ex.Message);
            }
        }

        private void WriteAuctionDataToFile(string filename)
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
                };
                records.Add(line);
            }

            if (records.Count > 0)
            {
                csvHelper.WriteAuctionFile(filename, records);
            }
        }

        private void OpenAuctionFile()
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

        private void ParseAuctionDateTimePick(string filename)
        {
            string shortName = Path.GetFileName(filename);
            if (!string.IsNullOrEmpty(shortName))
            {
                string timeString = shortName.Substring(auctionData.Length+1, format.Length);
                AuctionDatePickerDate =DateTime.ParseExact(timeString, format, CultureInfo.InvariantCulture);
            }
            else
            {
                AuctionDatePickerDate= DateTime.Now;
            }
        }

        private void ImportAuctionFile(string filename)
        {
            if (File.Exists(filename))
            {
                ParseAuctionDateTimePick(filename);

                var records = csvHelper.ReadAuctionAllLines(filename);

                if (records != null && records.Count > 0)
                {
                    CleanAuctionData();

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
                        }
                    }

                    CalculateAuctionData();
                }
            }
        }
        void SelectBackupPath()
        {
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();

                dialog.SelectedPath = SelectedBackupPath;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Settings.BackupPath = dialog.SelectedPath;

                    OnBackupPathChanged();
                }
            }
            catch (Exception ex)
            {
                LogError("DoSaveButtonClick() : ex = " + ex.Message);
            }
        }
        private void WriteToDb()
        {
            PublishStatusMessage("not completed");
        }

        private void InitDisplay()
        {
            InitBackupRootPath();

            AuctionDataCollection = new ObservableCollection<AuctionDataModel>();
            QuoteFileCollection = new ObservableCollection<QuoteFileModel>();
            MarketCollection = new ObservableCollection<string>();
        }

        private void PublishStatusMessage(string message)
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

        private string selectedBackupPath;

        public string SelectedBackupPath
        {
            get { return selectedBackupPath; }
            set
            {
                SetProperty(ref selectedBackupPath, value);
            }
        }
        void OnBackupPathChanged()
        {
            SelectedBackupPath = Settings.BackupPath;
            PublishStatusMessage($"Backup path change to {Settings.BackupPath}");
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

        private void UpdateMarketList()
        {
            MarketCollection = new ObservableCollection<string>(AuctionDataCollection.Select(o => o.Market).Distinct());
        }

        private void OnSelectMarket()
        {
            UpdateFilterStockDataCollection();
        }

        private void UpdateFilterStockDataCollection()
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